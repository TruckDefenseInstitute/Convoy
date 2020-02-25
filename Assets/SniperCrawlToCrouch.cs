using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperCrawlToCrouch : StateMachineBehaviour {
    public bool DisableMoveOnEntry;
    public bool EnableMoveOnExit;

    Unit _u;
    Weapon _w;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (_w == null) {
            _w = animator.GetComponentInParent<Weapon>();
        }
        _w.LoseAim();
        if (DisableMoveOnEntry) {
            if (_u == null) {
                _u = animator.GetComponentInParent<Unit>();
            }
            _u.DisableMovement(true);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (EnableMoveOnExit) {
            if (_u == null) {
                _u = animator.GetComponentInParent<Unit>();
            }
            _u.DisableMovement(false);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
