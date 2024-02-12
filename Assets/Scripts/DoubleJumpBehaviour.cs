using UnityEngine;

public class DoubleJumpBehaviour : StateMachineBehaviour
{
	private const string PlayerTag = "Player";

    private PlayerController playerController = null;

	private void OnEnable()
	{
		playerController = GameObject.Find(PlayerTag).GetComponent<PlayerController>();
	}

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// consume mid air jump
		playerController.IsMidAirJumping = false;
	}
}
