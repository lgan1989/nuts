using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

	public GameObject menu;
	private Canvas menuCanvas;
	// Use this for initialization
	void Start () {
		menuCanvas = menu.GetComponent<Canvas> ();
	}

	Vector2 GetGrid(){
		Vector2 mousePosition = Input.mousePosition;
		Vector3 realPosition = Camera.main.ScreenToWorldPoint(new Vector3 (mousePosition.x,mousePosition.y,0));

		Vector3 spritePosition = new Vector3 ();

		spritePosition.x = Mathf.Floor(realPosition.x/0.48f) * 0.48f;
		spritePosition.y = Mathf.Ceil(realPosition.y/0.48f) * 0.48f;
		spritePosition.z = 0;

		gameObject.GetComponent<Transform> ().position = spritePosition;

		return spritePosition;

	}
	
	// Update is called once per frame
	void Update () {

		if (menuCanvas.enabled) {
			enabled = false;
		} else {
			enabled = true;
		}

		GetGrid ();
			
	}

}
