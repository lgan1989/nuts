using UnityEngine;
using System.Enum;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {

	public MenuRender menu;
	public Canvas canvas;

	private ControlStatus currentStatus;
	public ControlStatus CurrentStatus{
		get{
			return currentStatus;
		}
		set{
			currentStatus = value;
		}
	}

	public enum ActionType{
		Attack,
		Move
	}

	public enum ControlStatus{
		Idle ,
		PlayerPawnSelected ,
		NonPlayerPawnSelected,
		MapTileSelected ,
		ShowMenu ,
		ChooseAttackTarget ,
		Moving ,
		Attacking,
		FinishAttack 
	}

	// Use this for initialization

	public List<Status> statusList;

	void InitDFA(){
		statusList = new List<Status>();
		int len = System.Enum.GetNames(typeof(ControlStatus)).Length;
		for (int i = 0 ; i < len ; i ++)
			statusList.Add( new Status() );

		//from idle to next

		LinkStatus(ControlStatus.Idle , new GameEvent(EventTargetType.PlayerPawn  , EventType.LeftClick) , ControlStatus.PlayerPawnSelected);
		LinkStatus(ControlStatus.Idle , new GameEvent(EventTargetType.FriendPawn , EventType.LeftClick) , ControlStatus.NonPlayerPawnSelected);
		LinkStatus(ControlStatus.Idle , new GameEvent(EventTargetType.EnemyPawn , EventType.LeftClick) , ControlStatus.NonPlayerPawnSelected);

		LinkStatus(ControlStatus.PlayerPawnSelected, new GameEvent( EventTargetType.MoveTile , EventType.LeftClick) , ControlStatus.Moving);
		LinkStatus(ControlStatus.Moving , new GameEvent( EventTargetType.None , EventType.MoveFinished) , ControlStatus.ShowMenu);

		LinkStatus(ControlStatus.ShowMenu , new GameEvent( EventTargetType.MenuItemAttack, EventType.LeftClick) , ControlStatus.ChooseAttackTarget);
		LinkStatus(ControlStatus.ChooseAttackTarget , new GameEvent(EventTargetType.EnemyPawn , EventType.LeftClick) , ControlStatus.Attacking);
		
	}

	void LinkStatus(ControlStatus from , GameEvent evt, ControlStatus to){
		statusList[ (int) from ].next[ evt ] = to; 
	}


	void Awake(){
//		DFA.Add();
	}

	void Start () {
		canvas = menu.GetComponent<Canvas> ();
		canvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ProcessEvent(){

	}

	public void ShowMenu(Vector3 position){
//		position.Set(position.x , position.y , -100);

//		Transform panel = canvas.transform.FindChild ("Panel");
	//	panel.position = position;
	//	panel.position = new Vector3 (panel.position.x + 60, panel.position.y - 60, panel.position.z);
	//	menu.transform.FindChild("Panel").transform.position = position;
		canvas.enabled = true;
	}

	public void HideMenu(){
		canvas.enabled = false;
	}
}
