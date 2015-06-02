using UnityEngine;
using System.Collections;

public class Cover : MonoBehaviour {

	public float fadeSpeed = 10.0f;
	public float fadeTime = 0.01f;
	public bool fadeIn = true;
	public float fade = 0;
	public Color color;
	public static int zIndex = 4 ;

	// Use this for initialization
	void Start () {
		fade = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeIn && 0.4f - fade > 0.00001f) {
			fade = Mathf.SmoothDamp(fade,0.4f, ref fadeSpeed,fadeTime);
			color.a = fade;
			gameObject.GetComponent<Renderer> ().material.SetColor ("_TintColor" , color);
		}
	
	}

	void OnMouseOver(){

		if (Logic.selectedPawn){
			if (Input.GetMouseButtonDown(0))
			{
				GameObject selector = GameObject.Find("Selection");
			
				Logic.selectedPawn.FindPathAndMoveTo(Selector.gridPosition);
			}
			else if (Input.GetMouseButtonDown(1))
			{
				Logic.selectedPawn = null;
			}
		}
		
	}
}
