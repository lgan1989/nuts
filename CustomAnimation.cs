using UnityEngine;
using System.Collections;


public class AnimationData{
    
    public float[] keytimes;
    public int[] frames;
}

public static class CustomAnimation {
    
    public static AnimationData[] idleAnimationData;
    public static AnimationData[] attackAnimationData;


    static CustomAnimation(){

        idleAnimationData = new AnimationData[4]; //down, up, side, weak
        attackAnimationData = new AnimationData[3]; //down, up, side

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
            attackAnimationData[i].keytimes = new float[4]{0.0f , 0.25f , 0.35f , 0.4f};
            int offset = (i << 2);
            attackAnimationData[i].frames = new int[4]{offset,offset+1,offset+2,offset+3};
        }

    }

	public enum Direction 
	{
		Down = 0,
		Up = 1,
		Side = 2
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
        StandBy = 7
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
                return SpriteSheetType.Special;
            case AnimationType.StandBy:
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
		int d = 0;
        switch (type){
            case AnimationType.Move:
                d = (int)dir;
                return idleAnimationData[d];
            case AnimationType.Attack:
                d = (int)dir;
                return attackAnimationData[d];
            default:
                return idleAnimationData[d];
        }
    }

}
