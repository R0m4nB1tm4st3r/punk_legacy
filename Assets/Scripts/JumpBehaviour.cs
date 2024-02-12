using UnityEngine;

public class JumpBehaviour : StateMachineBehaviour
{
    private InputController inputController = null;

	private void OnEnable() 
	{
		inputController = FindObjectOfType<InputController>();
	}

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// consume jump
		inputController.HasJumpInput = false;
	}
}
