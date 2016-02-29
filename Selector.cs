using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

	private Logic logic;
	public static TileGrid gridPosition;

	void Awake(){
		logic = GetComponent<Logic>();
	
	}


	// Use this for initialization
	void Start () {

	
	}

	Vector2 GetGrid(){
		Vector3 mousePosition = Input.mousePosition;
		Vector3 realPosition = Camera.main.ScreenToWorldPoint(new Vector3 (mousePosition.x,mousePosition.y,mousePosition.z));

		TileGrid grid = logic.GetGridByPosition(realPosition.x , realPosition.y);

		Vector3 spritePosition = logic.GetPositionByGrid(grid.x , grid.y , -99);
	
		transform.position = spritePosition;

		gridPosition = grid;

		return spritePosition;

	}

	
	// Update is called once per frame
	void Update () {

		if (Logic.control != null){
			if (Logic.control.CurrentStatus == Control.ControlStatus.ShowMenu){
				Vector3 spritePosition = logic.GetPositionByGrid(Logic.selectedPawn.gridPosition.x , Logic.selectedPawn.gridPosition.y , -99);
				
				transform.position = spritePosition;
				gridPosition = Logic.selectedPawn.gridPosition;
			}
			else {
				GetGrid ();
			}
		}
		else{
			GetGrid ();
		}


			
	}



}
