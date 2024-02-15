using UnityEngine;

public class DoubleJumpBehaviour : StateMachineBehaviour
{
	private const string PlayerTag = "Player";

    private MovementController playerMovementController = null;

	private void OnEnable()
	{
		playerMovementController = GameObject.Find(PlayerTag).GetComponent<MovementController>();
	}

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// consume mid air jump
		playerMovementController.IsMidAirJumping = false;
	}
}
