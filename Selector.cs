using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

	public GameObject menu;
	private Canvas menuCanvas;
	private Logic logic;
	public static TileGrid gridPosition;


	// Use this for initialization
	void Start () {
		menuCanvas = menu.GetComponent<Canvas> ();
		logic = GetComponent<Logic>();
	}

	Vector2 GetGrid(){
		Vector3 mousePosition = Input.mousePosition;
		Vector3 realPosition = Camera.main.ScreenToWorldPoint(new Vector3 (mousePosition.x,mousePosition.y,mousePosition.z));

		TileGrid grid = logic.GetGridByPosition(realPosition.x , realPosition.y);

		Vector3 spritePosition = logic.GetPositionByGrid(grid.x , grid.y , 0);
	
		transform.position = spritePosition;

		gridPosition = grid;

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
