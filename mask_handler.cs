using UnityEngine;
using System.Collections;

public class mask_handler : StateMachineBehaviour {



	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
		if (stateInfo.IsTag ("idle")) {
			Texture2D  mask = animator.gameObject.GetComponent<UpdateMask>().mask48;
			animator.gameObject.GetComponent<Renderer> ().material.SetInt ("_FrameNumber", 11);
			animator.gameObject.GetComponent<Renderer> ().material.SetTexture ("_AlphaTex", mask);
		} else {
			Texture2D  mask = animator.gameObject.GetComponent<UpdateMask>().mask64;
			animator.gameObject.GetComponent<Renderer> ().material.SetInt ("_FrameNumber", 12);
			animator.gameObject.GetComponent<Renderer> ().material.SetTexture ("_AlphaTex", mask);
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
//	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
//	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

//	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
