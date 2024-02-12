using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerAnimationController : MonoBehaviour
{
    private const string AnimParamVelocityX = "VelocityX";
    private const string AnimParamIsInAir = "IsInAir";
    private const string AnimParamIsJumping = "IsJumping";
    private const string AnimParamIsMidAirJumping = "IsMidAirJumping";

	private Animator animator = null;
    private SpriteRenderer spriteRenderer = null;
	private InputController inputController = null;
    private PlayerController playerController = null;
    private Rigidbody2D playerRigidBody = null;

	void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
		inputController = FindObjectOfType<InputController>();
        playerController = GetComponent<PlayerController>();
        playerRigidBody = playerController.gameObject.GetComponent<Rigidbody2D>();
     

        inputController.StartMoveEvent.AddListener(FlipSprite);
        playerController.UpdateInAirEvent.AddListener(UpdateInAirParam);
    }

    void Update()
    {
        animator.SetFloat(AnimParamVelocityX, Mathf.Abs(playerRigidBody.velocity.x));
		animator.SetBool(AnimParamIsJumping, inputController.HasJumpInput);
        animator.SetBool(AnimParamIsMidAirJumping, playerController.IsMidAirJumping);
	}

	private void OnDisable()
	{
		inputController.StartMoveEvent.RemoveListener(FlipSprite);
		playerController.UpdateInAirEvent.RemoveListener(UpdateInAirParam);
	}

	private void FlipSprite()
    {
        if (playerRigidBody.velocity.x > 0) spriteRenderer.flipX = false;
        else if (playerRigidBody.velocity.x < 0) spriteRenderer.flipX = true;
    }

    private void UpdateInAirParam (bool isInAir)
    {
		animator.SetBool(AnimParamIsInAir, isInAir);
	}
}
