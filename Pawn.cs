using UnityEngine;
using UnityEditor;
using System.Collections;

public class Pawn : MonoBehaviour {

	public int heroID = 0;
	public int team;
	
	public GameObject logicContrller;
	public GameObject guiController;

	
	public static int zIndex = -3;
	public TileGrid gridPosition;

	private ArrayList moveRange;
	private Logic logic;

	private Animator animator;

	private CustomAnimation.Direction faceDirection;

	//movement
	private ArrayList path;
	private bool moving = false;
	private int movingDirection = 0;
	private TileGrid movingGrid = null;
	private float px;
	private float py;
	private int frameCount = 0;

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

		temp ["attack_down"] = MakeAnimationClip (CustomAnimation.AnimationType.Attack , CustomAnimation.Direction.Down, false);

		animator.runtimeAnimatorController = temp;
		moveRange = new ArrayList();
		px = transform.position.x;
		py = transform.position.y;

		path = new ArrayList();

		logic = logicContrller.GetComponent<Logic>();


		gridPosition = logic.GetGridByPosition( transform.position.x , transform.position.y);
		logic.GridSetOccupied(gridPosition);

		Logic.pawnList.Add(this);
	}

	void DestroyMoveRange(){
		if (moveRange != null){
			foreach (GameObject go in moveRange){
				Destroy(go);
			}
			moveRange.Clear();
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

		if (Logic.selectedPawn == null || Logic.selectedPawn.gameObject != gameObject ){
			DestroyMoveRange();
		}
	
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
			if (path.Count > 0){
				MoveTo( (TileGrid)path[0] );
				gridPosition = (TileGrid)path[0];
				path.RemoveAt(0);  
				if (path.Count == 0)
					Idle();
			}
			else{	
				if (movingGrid != null){
					px = transform.position.x;
					py = transform.position.y;

					movingGrid = null;
				}

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
	
	public void Attack(){

	}

	void OnMouseDown(){
		 
	//	Canvas canvas = GameObject.Find ("MenuCanvas").GetComponent<Canvas> ();
	//	Transform panel = canvas.transform.FindChild ("Panel");
	//	panel.position = Input.mousePosition;
	//	panel.position = new Vector3 (panel.position.x + 60, panel.position.y - 60, panel.position.z);
	//	canvas.enabled = true;

		DestroyMoveRange();

		Logic.SelectPawn (this);

		logic.pawn = this;

		ArrayList validMove = logic.GetValidMove();

		foreach (TileGrid move in validMove)
		{
			GameObject tile = (GameObject)Instantiate(Resources.Load("TileCover"));
			
			tile.transform.position = logic.GetPositionByGrid( move.x , move.y , -99);
			tile.GetComponent<Cover>().color = new Color( 0.2f , 0.2f , 1.0f, 0f);
			  
			moveRange.Add( tile );

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
	
		AnimationUtility.SetAnimationClipSettings (animClip, settings);
		
		AnimationUtility.SetObjectReferenceCurve(animClip, curveBinding, keyFrames);

		return animClip;

	}
}
