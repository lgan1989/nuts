using UnityEngine;
using System.Collections;

public class StateTransition : StateMachineBehaviour {



	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Logic logic = GameObject.Find("LogicController").GetComponent<Logic>();
		Pawn pawn = animator.gameObject.GetComponent<Pawn>();
		if (pawn == null)
			return;

		TileInfo.TileType tileType = logic.GetTileType( pawn.gridPosition );
		if (stateInfo.IsTag ("attack")) {
			Texture2D  mask = GameObject.Find("MaskSet").GetComponent<MaskSet>().GetMask(tileType , true);
			animator.gameObject.GetComponent<Renderer> ().material.SetInt ("_FrameNumber", 12);
			animator.gameObject.GetComponent<Renderer> ().material.SetTexture ("_AlphaTex", mask);
		}
		else {
			Texture2D  mask = GameObject.Find("MaskSet").GetComponent<MaskSet>().GetMask( tileType , false);
			animator.gameObject.GetComponent<Renderer> ().material.SetInt ("_FrameNumber", 11);
			animator.gameObject.GetComponent<Renderer> ().material.SetTexture ("_AlphaTex", mask);
		} 
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
//	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.IsTag ("attack")) {
			Logic.control.CurrentStatus = Control.ControlStatus.FinishAttack;
		}
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
