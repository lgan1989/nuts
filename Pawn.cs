using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Collections;

public class Pawn : MonoBehaviour {

	public int heroID = 0;
	public Logic.Team team;

    public bool finished = false;

	public GameObject logicContrller;
	public GameObject guiController;


	public static int zIndex = -3;
	public TileGrid gridPosition;

	private ArrayList moveRange;
	private ArrayList attackRange;
	public ArrayList attackRangeGrid;

	public ArrayList MoveRange{
		get{
			return moveRange;
		}
		set{
			moveRange = value;
		}
	}
	public ArrayList AttackRange{
		get{
			return attackRange;
		}
		set{
			attackRange = value;
		}
	}

	private Logic logic;

	private Animator animator;

	public CustomAnimation.Direction faceDirection;

	//main properties
	public AttackType attackType;

	//for attack
	private ArrayList validTarget;

	//movement
	private ArrayList path;
	private bool moving = false;
	private int movingDirection = 0;
	private TileGrid movingGrid = null;
	private float px;
	private float py;
	private int frameCount = 0;

	public enum AttackType{
		Melee1,
		Melee2,
		Range
	};

	void Awake(){


	}

	// Use this for initialization
	void Start () {

		faceDirection = CustomAnimation.Direction.Down;

		animator = gameObject.GetComponent<Animator> ();
		AnimatorOverrideController temp = new AnimatorOverrideController ();
		temp.runtimeAnimatorController = ((Animator)GameObject.Find ("Template").GetComponent<Animator> ()).runtimeAnimatorController;

		temp [CustomAnimation.IDLE_DOWN] = MakeAnimationClip (CustomAnimation.AnimationType.Move , CustomAnimation.Direction.Down, false);
		temp [CustomAnimation.IDLE_UP] = MakeAnimationClip (CustomAnimation.AnimationType.Move , CustomAnimation.Direction.Up, false);
		temp [CustomAnimation.IDLE_SIDE] = MakeAnimationClip (CustomAnimation.AnimationType.Move , CustomAnimation.Direction.Left, false);

		temp [CustomAnimation.MOVE_DOWN] = MakeAnimationClip (CustomAnimation.AnimationType.Move , CustomAnimation.Direction.Down, false);
		temp [CustomAnimation.MOVE_UP] = MakeAnimationClip (CustomAnimation.AnimationType.Move , CustomAnimation.Direction.Up, false);
		temp [CustomAnimation.MOVE_SIDE] = MakeAnimationClip (CustomAnimation.AnimationType.Move , CustomAnimation.Direction.Left, false);

		temp [CustomAnimation.ATTACK_DOWN] = MakeAnimationClip (CustomAnimation.AnimationType.Attack , CustomAnimation.Direction.Down, false);
		temp [CustomAnimation.ATTACK_UP] = MakeAnimationClip (CustomAnimation.AnimationType.Attack , CustomAnimation.Direction.Up, false);
		temp [CustomAnimation.ATTACK_SIDE] = MakeAnimationClip (CustomAnimation.AnimationType.Attack , CustomAnimation.Direction.Left, false);

		temp [CustomAnimation.PARRY_DOWN] = MakeAnimationClip (CustomAnimation.AnimationType.Parry , CustomAnimation.Direction.Down, false);
		temp [CustomAnimation.PARRY_UP] = MakeAnimationClip (CustomAnimation.AnimationType.Parry , CustomAnimation.Direction.Up, false);
		temp [CustomAnimation.PARRY_SIDE] = MakeAnimationClip (CustomAnimation.AnimationType.Parry , CustomAnimation.Direction.Left, false);

		temp [CustomAnimation.STAND_DOWN] = MakeAnimationClip (CustomAnimation.AnimationType.Stand , CustomAnimation.Direction.Down, false);
		temp [CustomAnimation.STAND_UP] = MakeAnimationClip (CustomAnimation.AnimationType.Stand , CustomAnimation.Direction.Up, false);
		temp [CustomAnimation.STAND_SIDE] = MakeAnimationClip (CustomAnimation.AnimationType.Stand , CustomAnimation.Direction.Left, false);

		temp [CustomAnimation.DAMAGED] = MakeAnimationClip (CustomAnimation.AnimationType.Damaged , CustomAnimation.Direction.Down, false);

		animator.runtimeAnimatorController = temp;
		moveRange = new ArrayList();
		px = transform.position.x;
		py = transform.position.y;

		path = new ArrayList();

		logic = logicContrller.GetComponent<Logic>();

		gridPosition = logic.GetGridByPosition( transform.position.x , transform.position.y);

		attackRange = GetAttackRange(attackType);

		attackRangeGrid = new ArrayList();

		logic.GridSetOccupied(gridPosition);

		Logic.pawnList.Add(this);
	}

	ArrayList GetAttackRange(AttackType type){
		ArrayList range = new ArrayList();
		if (type == AttackType.Melee1){
			range.Add( new TileGrid(0,1) );
			range.Add( new TileGrid(1,0) );
			range.Add( new TileGrid(0,-1) );
			range.Add( new TileGrid(-1,0) );
		}
		else if (type == AttackType.Melee2){
			range.Add( new TileGrid(0,1) );
			range.Add( new TileGrid(1,0) );
			range.Add( new TileGrid(0,-1) );
			range.Add( new TileGrid(-1,0) );
			range.Add( new TileGrid(1,1) );
			range.Add( new TileGrid(1,-1) );
			range.Add( new TileGrid(-1,-1) );
			range.Add( new TileGrid(-1,1) );
		}
		else if (type == AttackType.Range){
			range.Add( new TileGrid(0,2) );
			range.Add( new TileGrid(2,0) );
			range.Add( new TileGrid(0,-2) );
			range.Add( new TileGrid(-2,0) );
		}
		return range;
	}

	public void DestroyMoveRange(){
		if (moveRange != null){
			foreach (GameObject go in moveRange){
				Destroy(go);
			}
			moveRange.Clear();
		}
	}

	public void DestroyAttackRange(){
		if (attackRangeGrid != null){
			foreach (GameObject go in attackRangeGrid){
				Destroy(go);
			}
			attackRangeGrid.Clear();
		}
	}

	// Update is called once per frame
	void Update () {


		if (faceDirection == CustomAnimation.Direction.Right){
			transform.localScale = new Vector2(-1, transform.localScale.y);
		}
		else{
			transform.localScale = new Vector2(1, transform.localScale.y);
		}

		if (logic.GetTileType(gridPosition) != TileInfo.TileType.Ground){
			gameObject.GetComponent<Renderer>().material.shader = Shader.Find ("Mask");
		}
		else{
			gameObject.GetComponent<Renderer>().material.shader = Shader.Find ("Sprites/Default");
		}

		if (Logic.selectedPawn != this || Logic.selectedPawn == null){
			DestroyAttackRange();
			DestroyMoveRange();
		}

		if (finished){
			return;
		}

		if (Logic.control.CurrentStatus == Control.ControlStatus.Moving){

			if (moving){

				Vector3 movingPosition = logic.GetPositionByGrid( movingGrid.x , movingGrid.y , Pawn.zIndex);

				px = px + 0.08f * Logic.DIR[movingDirection,0];
				py = py - 0.08f * Logic.DIR[movingDirection,1];

			//	px = Mathf.Lerp(px, movingPosition.x , movingTime);
			//	py = Mathf.Lerp(py, movingPosition.y , movingTime);

				gameObject.transform.position = new Vector3(px , py , Pawn.zIndex);
				frameCount ++;
				if ( frameCount == 6){
					frameCount = 0;
					moving = false;
				}

			}
			else{

				if (path.Count == 0){
					Idle();
				}

				if (path.Count > 0){

					if ( (TileGrid)path[0] == gridPosition && path.Count == 1){
						Logic.control.CurrentStatus = Control.ControlStatus.ShowMenu;
						path.Remove(0);
					}
					else{
						MoveTo( (TileGrid)path[0] );
						gridPosition = (TileGrid)path[0];
						path.RemoveAt(0);
					}
				}
				else{
					if (movingGrid != null){
						px = transform.position.x;
						py = transform.position.y;
						movingGrid = null;

						Logic.control.CurrentStatus = Control.ControlStatus.ShowMenu;

					}

				}
			}
		}
        else{
			if (Logic.control.CurrentStatus == Control.ControlStatus.Idle){
				Idle();
			}
            else if (Logic.control.CurrentStatus == Control.ControlStatus.Finished && this == Logic.selectedPawn) {
                StandBy();
            }
		}

	}


	public void Idle(){
		switch (faceDirection){
		case CustomAnimation.Direction.Down:
			animator.Play(CustomAnimation.STATE_IDLE_DOWN);
			break;
		case CustomAnimation.Direction.Up:
			animator.Play(CustomAnimation.STATE_IDLE_UP);
			break;
		case CustomAnimation.Direction.Left:
		case CustomAnimation.Direction.Right:
			animator.Play(CustomAnimation.STATE_IDLE_SIDE);
			break;
		}
	}

	public void MoveTo(TileGrid destination){

		CustomAnimation.Direction d = Logic.GetDirection( gridPosition , destination);

		faceDirection = d;

		AnimationClip clip = null;
		switch (d)
		{
			case CustomAnimation.Direction.Down:
			animator.Play(CustomAnimation.STATE_MOVE_DOWN);
			break;
			case CustomAnimation.Direction.Up:
			animator.Play(CustomAnimation.STATE_MOVE_UP);
			break;
			case CustomAnimation.Direction.Left:
			case CustomAnimation.Direction.Right:
			animator.Play(CustomAnimation.STATE_MOVE_SIDE);
			break;
		}

		moving = true;
		movingGrid = destination;
		movingDirection = (int)d;

	}


	public void FindPathAndMoveTo(TileGrid destination){
		DestroyMoveRange();
		logic.GridSetEmpty(gridPosition);
		path = logic.FindPath( gridPosition , destination, 6);
		logic.GridSetOccupied(destination);
	}

	public void Attack(Pawn pawn){
		faceDirection = Logic.GetDirection(gridPosition , pawn.gridPosition);
		switch (faceDirection)
		{
		case CustomAnimation.Direction.Down:
			animator.Play(CustomAnimation.STATE_ATTACK_DOWN);
			break;
		case CustomAnimation.Direction.Up:
			animator.Play(CustomAnimation.STATE_ATTACK_UP);
			break;
		case CustomAnimation.Direction.Left:
		case CustomAnimation.Direction.Right:
			animator.Play(CustomAnimation.STATE_ATTACK_SIDE);
			break;
		}
	}

	public void Parry(Pawn pawn){
		faceDirection = Logic.GetDirection(gridPosition , pawn.gridPosition);
		switch (faceDirection)
		{
		case CustomAnimation.Direction.Down:
			animator.Play(CustomAnimation.STATE_PARRY_DOWN);
			break;
		case CustomAnimation.Direction.Up:
			animator.Play(CustomAnimation.STATE_PARRY_UP);
			break;
		case CustomAnimation.Direction.Left:
		case CustomAnimation.Direction.Right:
			animator.Play(CustomAnimation.STATE_PARRY_SIDE);
			break;
		}
	}

	public void Damaged(){
        animator.Play(CustomAnimation.STATE_DAMAGED);
	}

    public void StandBy(){
        finished = true;
		switch (faceDirection)
		{
		case CustomAnimation.Direction.Down:
			animator.Play(CustomAnimation.STATE_STAND_DOWN);
			break;
		case CustomAnimation.Direction.Up:
			animator.Play(CustomAnimation.STATE_STAND_UP);
			break;
		case CustomAnimation.Direction.Left:
		case CustomAnimation.Direction.Right:
			animator.Play(CustomAnimation.STATE_STAND_SIDE);
			break;
		}
        EventType type = EventType.ActionFinished;
		Control.PushEvent( new GameEvent(EventTargetType.None , gameObject , type) );
    }

	void OnMouseDown(){

	//	Canvas canvas = GameObject.Find ("MenuCanvas").GetComponent<Canvas> ();
	//	Transform panel = canvas.transform.FindChild ("Panel");
	//	panel.position = Input.mousePosition;
	//	panel.position = new Vector3 (panel.position.x + 60, panel.position.y - 60, panel.position.z);
	//	canvas.enabled = true;

		if (EventSystem.current.IsPointerOverGameObject()) return;
		/*
		if (Logic.control.CurrentStatus == Control.ControlStatus.Idle)
			Logic.SelectPawn (this);
		else if (Logic.control.CurrentStatus == Control.ControlStatus.ChooseAttackTarget){
			if ( this.IsValidTarget( Logic.selectedPawn , Control.ActionType.Attack ) ){
				Logic.selectedPawn.Attack( this );
			}
		}*/
		EventType type = EventType.LeftClick;
		if (Input.GetMouseButtonDown(1)){
			type = EventType.RightClick;
		}

		EventTargetType target = EventTargetType.None;
		if (team == Logic.Team.Player){
			target = EventTargetType.PlayerPawn;
		}
		else if (team == Logic.Team.Friend){
			target = EventTargetType.FriendPawn;
		}
		else if (team == Logic.Team.Enermy){
			target = EventTargetType.EnemyPawn;
		}

		Control.PushEvent( new GameEvent(target , gameObject , type) );

	}

	public bool IsValidTarget(Pawn pawn , Control.ActionType action){
		if (action == Control.ActionType.Attack){
			return this.team != pawn.team;
		}
		return false;
	}

	Color GetColorByTeam(){
		if (team == Logic.Team.Player)
			return new Color( 0.2f , 0.2f , 1.0f, 0f);
		else{
			return new Color( 0.1f , 0.8f , 0.2f, 0f);;
		}
	}

	public void ShowMoveRange(){
		ArrayList validMove = logic.GetValidMove();

		foreach (TileGrid move in validMove)
		{
			GameObject tile = (GameObject)Instantiate(Resources.Load("TileCover"));

			tile.transform.position = logic.GetPositionByGrid( move.x , move.y , -98);


			tile.GetComponent<Cover>().color = GetColorByTeam();


			moveRange.Add( tile );

		}
	}

	public void ShowAttackRange(){

		ArrayList validRange = logic.GetAttackRange();

		foreach (TileGrid grid in validRange)
		{
			GameObject tile = (GameObject)Instantiate(Resources.Load("UI/AttackFrame"));

			tile.transform.position = logic.GetPositionByGrid( grid.x , grid.y , -99);
			tile.GetComponent<Cover>().color = new Color( 1.0f , 0.2f , 0.2f, 0f);

			attackRangeGrid.Add( tile );

		}

		ArrayList validTarget = logic.GetValidTarget();

		foreach (Pawn target in validTarget)
		{
			GameObject tile = (GameObject)Instantiate(Resources.Load("TileCover"));

			tile.transform.position = logic.GetPositionByGrid( target.gridPosition.x , target.gridPosition.y , -97);
			tile.GetComponent<Cover>().color = new Color( 1.0f , 0.2f , 0.2f, 0f);
			tile.GetComponent<Cover>().transparency = 0.6f;
			tile.GetComponent<BoxCollider2D>().enabled = false;

			attackRangeGrid.Add( tile );

		}

	}

	private AnimationClip MakeAnimationClip(CustomAnimation.AnimationType type , CustomAnimation.Direction dir , bool isWeak){

		AnimationClip animClip = new AnimationClip();

		// First you need to create e Editor Curve Binding
		EditorCurveBinding curveBinding = new EditorCurveBinding();

		// I want to change the sprites of the sprite renderer, so I put the typeof(SpriteRenderer) as the binding type.
		curveBinding.type = typeof(SpriteRenderer);
		// Regular path to the gameobject that will be changed (empty string means root)
		curveBinding.path = "";
		// This is the property name to change the sprite of a sprite renderer
		curveBinding.propertyName = "m_Sprite";

		// An array to hold the object keyframes

        AnimationData data = CustomAnimation.GetAnimationData(type, dir , isWeak);
        int frameNumber = data.frames.Length;

		ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[frameNumber];

		string spriteSheet =  CustomAnimation.GetSpriteSheetName(heroID , type);

		Sprite[] sprites = Resources.LoadAll<Sprite> (spriteSheet);

		for (int i = 0; i < frameNumber; i++)
		{
			keyFrames[i] = new ObjectReferenceKeyframe();
			keyFrames[i].time = data.keytimes[i];
			keyFrames[i].value = sprites[ data.frames[i] ];

		}



		animClip.frameRate = 12;
		animClip.wrapMode = WrapMode.Loop;



		AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings (animClip);
		settings.loopTime = true;

		if (type == CustomAnimation.AnimationType.Attack){
			animClip.wrapMode = WrapMode.Once;
			animClip.frameRate = 12;
			settings.loopTime = false;
		}

		AnimationUtility.SetAnimationClipSettings (animClip, settings);

		AnimationUtility.SetObjectReferenceCurve(animClip, curveBinding, keyFrames);

		return animClip;

	}
}
