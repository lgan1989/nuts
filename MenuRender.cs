using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuRender : MonoBehaviour {

	public Button btnAttack;
	private Canvas canvas;
	private Logic logic;

	// Use this for initialization
	void Start () {
		canvas = gameObject.GetComponent<Canvas> ();
		canvas.enabled = false;
		btnAttack = (GameObject.Find("btnAttack")).GetComponent<Button>();
		btnAttack.onClick.AddListener(BtnAttackClicked);
		logic = GameObject.Find("LogicController").GetComponent<Logic>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver(){

		
		
	}

	void BtnAttackClicked(){
		canvas.enabled = false;
		Logic.control.CurrentStatus = Control.ControlStatus.ChooseAttackTarget;
	
	}




}
