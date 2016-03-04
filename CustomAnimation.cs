using UnityEngine;
using System.Collections;


public class AnimationData{

    public float[] keytimes;
    public int[] frames;
}

public static class CustomAnimation {

    public static AnimationData[] idleAnimationData;
    public static AnimationData[] attackAnimationData;
    public static AnimationData[] specialAnimationData;
    public static AnimationData[] standAnimationData;

	public const string IDLE_DOWN = "idle_down";
	public const string IDLE_UP = "idle_up";
	public const string IDLE_SIDE = "idle_side";

	public const string MOVE_DOWN = "move_down";
	public const string MOVE_UP = "move_up";
	public const string MOVE_SIDE = "move_side";

	public const string ATTACK_DOWN = "attack_down";
	public const string ATTACK_UP = "attack_up";
	public const string ATTACK_SIDE = "attack_side";

	public const string PARRY_DOWN = "parry_down";
	public const string PARRY_UP = "parry_up";
	public const string PARRY_SIDE = "parry_side";

	public const string STAND_DOWN = "stand_down";
	public const string STAND_UP = "stand_up";
	public const string STAND_SIDE = "stand_side";

	public const string DAMAGED = "damaged";

	public const string STATE_MOVE_DOWN = "MoveDown";
	public const string STATE_MOVE_UP = "MoveUp";
	public const string STATE_MOVE_SIDE = "MoveSide";

	public const string STATE_IDLE_DOWN = "IdleDown";
	public const string STATE_IDLE_UP = "IdleUp";
	public const string STATE_IDLE_SIDE = "IdleSide";

	public const string STATE_ATTACK_DOWN = "AttackDown";
	public const string STATE_ATTACK_UP = "AttackUp";
	public const string STATE_ATTACK_SIDE = "AttackSide";

	public const string STATE_PARRY_DOWN = "ParryDown";
	public const string STATE_PARRY_UP = "ParryUp";
	public const string STATE_PARRY_SIDE = "ParrySide";

	public const string STATE_DAMAGED = "Damaged";

	public const string STATE_STAND_DOWN = "StandDown";
	public const string STATE_STAND_UP = "StandUp";
	public const string STATE_STAND_SIDE = "StandSide";

    static CustomAnimation(){

        idleAnimationData = new AnimationData[4]; //down, up, side, weak
        attackAnimationData = new AnimationData[3]; //down, up, side
        specialAnimationData = new AnimationData[5]; // parry_down, parry_up, parry_side, damaged, celebrate
        standAnimationData = new AnimationData[3]; //down, up, side

        for (int i = 0 ; i < 3 ; i ++){
			idleAnimationData[i] = new AnimationData();
            idleAnimationData[i].keytimes = new float[2]{0.0f , 0.1f};
            idleAnimationData[i].frames = new int[2]{i<<1 , (i<<1)+1 };
        }
		idleAnimationData [3] = new AnimationData ();
        idleAnimationData[3].keytimes = new float[2]{0.0f, 0.1f};
        idleAnimationData[3].frames = new int[2]{9,10};


        for (int i = 0 ; i < 3 ; i ++){
			attackAnimationData[i] = new AnimationData();
            attackAnimationData[i].keytimes = new float[4]{0.0f , 0.20f , 0.20f , 0.4f};
            int offset = (i << 2);
            attackAnimationData[i].frames = new int[4]{offset,offset+1,offset+2,offset+3};
        }

        for (int i = 0 ; i < 5 ; i ++){
			specialAnimationData[i] = new AnimationData();
            specialAnimationData[i].keytimes = new float[1]{0.2f};
            specialAnimationData[i].frames = new int[1]{i};
        }

        for (int i = 0 ; i < 3 ; i ++){
			standAnimationData[i] = new AnimationData();
            standAnimationData[i].keytimes = new float[1]{0.0f};
            int offset = (i << 2);
            standAnimationData[i].frames = new int[1]{offset};
        }

    }

	public enum Direction
	{
		Down = 0,
		Up = 1,
		Left = 2,
		Right = 3
	}
	public enum SpriteSheetType
	{
		Move = 0,
		Attack = 1,
		Special = 2
	}

    public enum AnimationType
    {
        Idle = 0,
        Move = 1,
        Attack = 2,
        Cast = 3,
        Parry = 4,
        Buffer = 5,
        LevelUp = 6,
        Stand = 7,
        Damaged = 8
    }



	private static string[] spriteSheetsFolders = new string[3] {"MoveSpriteSheets" , "AttackSpriteSheets" , "SpecialSpriteSheets"};
    private static string[] spriteSheetsAbbr = new string[3] {"mov" , "atk" , "spc"};

    public static SpriteSheetType GetSpriteSheetTypeByAnimation(AnimationType type)
    {
        switch (type){
            case AnimationType.Idle:
            case AnimationType.Move:
                return SpriteSheetType.Move;
            case AnimationType.Attack:
            case AnimationType.Cast:
                return SpriteSheetType.Attack;
            case AnimationType.Parry:
            case AnimationType.Buffer:
            case AnimationType.LevelUp:
            case AnimationType.Damaged:
                return SpriteSheetType.Special;
            case AnimationType.Stand:
                return SpriteSheetType.Move;
        }
		return SpriteSheetType.Move;
    }

	public static string GetSpriteSheetName(int heroID, AnimationType type)
	{
		return spriteSheetsFolders[(int)GetSpriteSheetTypeByAnimation(type)] + string.Format("/Unit_{0}_{1}-1" , spriteSheetsAbbr[(int)GetSpriteSheetTypeByAnimation(type)], heroID);
	}

    public static AnimationData GetAnimationData(AnimationType type , Direction dir,  bool isWeak)
    {
		int d = (int)dir;
		if (d == 3)
			d = 2;
        switch (type){
            case AnimationType.Move:
                return idleAnimationData[d];
            case AnimationType.Attack:
                return attackAnimationData[d];
            case AnimationType.Damaged:
                return specialAnimationData[3];
            case AnimationType.Parry:
                return specialAnimationData[d];
            case AnimationType.Stand:
                return standAnimationData[d];
            default:
                return idleAnimationData[d];
        }
    }

}
