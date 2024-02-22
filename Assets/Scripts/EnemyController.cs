using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(CircleCollider2D))]
public class EnemyController : MonoBehaviour
{
    [field: SerializeField]
    public float PlayerDetectRange { get; set; } = 4f;
    [field: SerializeField]
    public float MoveSpeed { get; set; } = 9f;
    [field: SerializeField]
    public float DirectionChangeIntervalSeconds { get; set; } = 2.5f;

	public bool IsAttackingPlayer { get; private set; } = false;
    public bool IsPlayerLeft { get => transform.position.x > player.transform.position.x; }
	public Vector2 CurrentVelocity { get => rigidBody.velocity; }
    public Vector2 VelocityTowardsPlayer
    {
        get => IsPlayerLeft
			? new(MoveSpeed * -1, CurrentVelocity.y)
            : new(MoveSpeed, CurrentVelocity.y);
    }

	private CircleCollider2D playerDetector = null;
    private Rigidbody2D rigidBody = null;
    private GameObject player = null;
    private DamageController damageController = null;
    
    private IEnumerator patrolCoroutine = null;
    private IEnumerator chaseCoroutine = null;
    private IEnumerator resumeActionCoroutine = null;

    void Start()
    {
        playerDetector = GetComponent<CircleCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        damageController = GetComponent<DamageController>();

        playerDetector.radius = PlayerDetectRange;

        damageController.ReceiveDmgEvent.AddListener(SuspendActions);

        StartPatroling();
    }

	private void OnDisable()
	{
		damageController.ReceiveDmgEvent.RemoveListener(SuspendActions);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent<MovementController>(out var playerMovementController))
        {
            if (player == null) player = playerMovementController.gameObject;

            StartChasing();
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.TryGetComponent<MovementController>(out var playerMovementController))
		{
			if (player == null) player = playerMovementController.gameObject;

			StartPatroling();
		}
	}

	private void StartPatroling()
    {
        if (chaseCoroutine != null) StopCoroutine(chaseCoroutine);

        IsAttackingPlayer = false;

        patrolCoroutine = Patrol();
        StartCoroutine(patrolCoroutine);
    }

    private void StartChasing()
    {
        if (patrolCoroutine != null) StopCoroutine(patrolCoroutine);

        IsAttackingPlayer = true;

        chaseCoroutine = Chase();
        StartCoroutine(chaseCoroutine);
    }

    private void SuspendActions()
    {
        Debug.Log("SUSPENDING ACTIONS");
        rigidBody.velocity = Vector2.zero;

        if (IsAttackingPlayer)
        {
			if (chaseCoroutine != null) StopCoroutine(chaseCoroutine);
		} 
        else
        {
			if (patrolCoroutine != null) StopCoroutine(patrolCoroutine);
		}

        if (resumeActionCoroutine != null) StopCoroutine(resumeActionCoroutine);

        resumeActionCoroutine = ResumeAction();
        StartCoroutine (resumeActionCoroutine);
    }

    private IEnumerator Patrol()
    {
		rigidBody.velocity = new(MoveSpeed, CurrentVelocity.y);

		while (true)
        {
            yield return new WaitForSeconds(DirectionChangeIntervalSeconds);
            rigidBody.velocity = new(CurrentVelocity.x * -1, CurrentVelocity.y);
        }
    }

    private IEnumerator Chase()
    {
        while (true)
        {
            rigidBody.velocity = VelocityTowardsPlayer;
            yield return new WaitForFixedUpdate();
		}
    }

    private IEnumerator ResumeAction()
    {
        yield return new WaitForSeconds(damageController.RecoveryTimeSeconds);
        
        if (IsAttackingPlayer) StartChasing();
        else StartPatroling();
    }
}
