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
		if (Input.GetMouseButtonDown(0))
		{
			Logic.DeselectPawn();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            EventType type = EventType.RightClick;
            EventTargetType target = EventTargetType.Any;
            Control.PushEvent(new GameEvent(target, gameObject, type));
        }

		
	}

	void OnGUI(){

	}


}
