using UnityEngine;
using UnityEditor;
using System.Collections;

public class Pawn : MonoBehaviour {

	public int heroID = 0;



	public GameObject pawn;
	// Use this for initialization
	void Start () {
	
		Animator animator = gameObject.GetComponent<Animator> ();
		AnimatorOverrideController temp = new AnimatorOverrideController ();
		temp.runtimeAnimatorController = ((Animator)GameObject.Find ("Template").GetComponent<Animator> ()).runtimeAnimatorController;
		temp ["idle_down"] = MakeAnimationClip (CustomAnimation.AnimationType.Move , CustomAnimation.Direction.Down, false); 
		temp ["attack_down"] = MakeAnimationClip (CustomAnimation.AnimationType.Attack , CustomAnimation.Direction.Down, false);

		animator.runtimeAnimatorController = temp;

	}
	
	// Update is called once per frame
	void Update () {


	
	}

	public void Attack(){

	}

	void OnMouseDown(){

		Canvas canvas = GameObject.Find ("MenuCanvas").GetComponent<Canvas> ();
		Transform panel = canvas.transform.FindChild ("Panel");
		panel.position = Input.mousePosition;
		panel.position = new Vector3 (panel.position.x + 60, panel.position.y - 60, panel.position.z);
		canvas.enabled = true;
		
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
