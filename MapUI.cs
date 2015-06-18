using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MapUI : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseOver(){

		if (EventSystem.current.IsPointerOverGameObject()) return;
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			Logic.DeselectPawn();
		}
		
	}



	void OnGUI(){

	}


}
