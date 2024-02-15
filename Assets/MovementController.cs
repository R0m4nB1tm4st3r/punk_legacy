using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{
	private const int DefaultJumpCount = 1;

	[field: SerializeField]
	float MoveSpeed { get; set; } = 10f;
	[field: SerializeField]
	float JumpStrength { get; set; } = 10f;
	[field: SerializeField]
	float AccelerationFactor { get; set; } = 0.2f;
	[field: SerializeField]
	bool HasMultiJump { get; set; } = false;
	[field: SerializeField]
	int ExtraJumps { get; set; } = 1;

	public bool IsOnGround { get; private set; } = false;
	public bool IsOnPlatform { get; private set; } = false;
	public bool IsJumping { get; set; } = false;
	public bool IsMidAirJumping { get; set; } = false;
	public int RemainingJumps { get => remainingJumps; }
	public int MaxAvailableJumps { get => DefaultJumpCount + ExtraJumps; }
	public UnityEvent<bool> UpdateInAirEvent { get; private set; } = null;
	public UnityEvent<bool> UpdateMidAirJumpEvent { get; private set; } = null;

	private Rigidbody2D rigidBody = null;
	private InputController inputController = null;

	private int remainingJumps = 0;
	private float targetVelocityX = 0f;
	Vector2 targetVelocity = Vector2.zero;
	IEnumerator moveCoroutine = null;

	private void Awake()
	{
		UpdateInAirEvent = new UnityEvent<bool>();
	}

	void Start()
    {
		rigidBody = GetComponent<Rigidbody2D>();
		inputController = FindObjectOfType<InputController>();

		inputController.StartMoveEvent.AddListener(StartMoving);
		inputController.StopMoveEvent.AddListener(StopMoving);
		inputController.JumpEvent.AddListener(Jump);
	}

	void StartMoving()
	{
		targetVelocityX = inputController.MoveVector.x * MoveSpeed;

		moveCoroutine = Move();
		StartCoroutine(moveCoroutine);
	}

	void StopMoving()
	{
		StopCoroutine(moveCoroutine);

		Vector2 velocity = rigidBody.velocity;
		velocity.x = targetVelocityX * Time.fixedDeltaTime;
		rigidBody.velocity = velocity;
	}

	void Jump(bool hasJumpInput)
	{
		if (hasJumpInput && (remainingJumps > 0))
		{
			IsJumping = true;

			if (remainingJumps < DefaultJumpCount + ExtraJumps && remainingJumps > 0)
				IsMidAirJumping = true;

			rigidBody.velocity = new(rigidBody.velocity.x, 0);
			rigidBody.AddForce(new Vector2(0, JumpStrength), ForceMode2D.Impulse);
			remainingJumps--;
		}
	}

	void CheckWalkableCollision(Collision2D collision, bool entering)
	{
		var go = collision.gameObject;

		if (go.TryGetComponent<WalkableObject>(out var walkable))
		{
			if (go.transform.position.y < transform.position.y)
			{
				if (walkable.IsGround) IsOnGround = entering;
				else if (walkable.IsPlatform) IsOnPlatform = entering;

				if (entering)
				{
					remainingJumps = DefaultJumpCount + (HasMultiJump ? ExtraJumps : 0);
					IsJumping = false;
				}
				else if (!IsJumping) remainingJumps--;

				UpdateInAirEvent.Invoke(!(IsOnGround || IsOnPlatform));
			}
		}
	}

	IEnumerator Move()
	{
		while (true)
		{
			targetVelocity = new(targetVelocityX, rigidBody.velocity.y);

			rigidBody.velocity = Vector2.MoveTowards(
				rigidBody.velocity,
				targetVelocity,
				AccelerationFactor);

			yield return new WaitForFixedUpdate();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		CheckWalkableCollision(collision, true);
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		CheckWalkableCollision(collision, false);
	}
}
