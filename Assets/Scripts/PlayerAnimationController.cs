using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerAnimationController : MonoBehaviour
{
    private const string AnimParamVelocityX = "VelocityX";
    private const string AnimParamIsInAir = "IsInAir";
    private const string AnimParamIsJumping = "IsJumping";
    private const string AnimParamIsMidAirJumping = "IsMidAirJumping";
    private const string AnimParamComboCount = "ComboCount";
    private const string AnimParamIsPunching = "IsPunching";
    private const string AnimParamIsDead = "IsDead";

	private Animator animator = null;
    private SpriteRenderer spriteRenderer = null;
	private InputController inputController = null;
    private MovementController movementController = null;
    private MeleeController meleeController = null;
    private DamageController damageController = null;
    private Rigidbody2D playerRigidBody = null;

	void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
		inputController = FindObjectOfType<InputController>();
        movementController = GetComponent<MovementController>();
        meleeController = GetComponent<MeleeController>();
        damageController = GetComponent<DamageController>();
        playerRigidBody = movementController.gameObject.GetComponent<Rigidbody2D>();
     
        inputController.StartMoveEvent.AddListener(FlipSprite);
        movementController.UpdateInAirEvent.AddListener(UpdateInAirParam);
        meleeController.UpdateComboCounterEvent.AddListener(UpdateComboCountParam);
        damageController.DieEvent.AddListener(UpdateIsDeadParam);
    }

    void Update()
    {
        animator.SetFloat(AnimParamVelocityX, Mathf.Abs(playerRigidBody.velocity.x));
		animator.SetBool(AnimParamIsJumping, movementController.IsJumping);
        animator.SetBool(AnimParamIsMidAirJumping, movementController.IsMidAirJumping);
        animator.SetBool(AnimParamIsPunching, inputController.HasPunchInput);
	}

	private void OnDisable()
	{
		inputController.StartMoveEvent.RemoveListener(FlipSprite);
		movementController.UpdateInAirEvent.RemoveListener(UpdateInAirParam);
		meleeController.UpdateComboCounterEvent.RemoveListener(UpdateComboCountParam);
	}

	private void FlipSprite()
    {
        if (inputController.MoveVector.x > 0) spriteRenderer.flipX = false;
        else if (inputController.MoveVector.x < 0) spriteRenderer.flipX = true;
    }

    private void UpdateInAirParam (bool isInAir)
    {
		animator.SetBool(AnimParamIsInAir, isInAir);
	}

    private void UpdateComboCountParam(int newComboCount)
    {
        animator.SetInteger(AnimParamComboCount, newComboCount);
    }

    private void UpdateIsDeadParam(bool isDead)
    {
        animator.SetBool(AnimParamIsDead, isDead);

        if (isDead)
        {
			inputController.StartMoveEvent.RemoveListener(FlipSprite);
		}
    }
}
