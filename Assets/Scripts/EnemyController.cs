using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(CircleCollider2D))]
public class EnemyController : MonoBehaviour
{
	private const string BossTag = "Boss";
	private const string UnlockableTagPrefix = "Unlockable-";
    private const float ReturnToMainMenuDelay = 2f;

    [field: SerializeField]
    public float PlayerDetectRange { get; set; } = 4f;
    [field: SerializeField]
    public float MoveSpeed { get; set; } = 9f;
    [field: SerializeField]
    public float DirectionChangeIntervalSeconds { get; set; } = 2.5f;
    [field: SerializeField]
    public string UnlockableId = "01";

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
    private GameObject unlockable = null;
    private DamageController damageController = null;
    private GameManager gameManager = null;
    
    private IEnumerator patrolCoroutine = null;
    private IEnumerator chaseCoroutine = null;
    private IEnumerator resumeActionCoroutine = null;

    void Start()
    {
        playerDetector = GetComponent<CircleCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        damageController = GetComponent<DamageController>();
        unlockable = GameObject.FindWithTag($"{UnlockableTagPrefix}{UnlockableId}");
        gameManager = FindObjectOfType<GameManager>();

        if (unlockable != null) unlockable.SetActive( false );

        playerDetector.radius = PlayerDetectRange;

        damageController.ReceiveDmgEvent.AddListener(SuspendActions);

        if (gameObject.CompareTag(BossTag))
            damageController.DieEvent.AddListener((bool isDead) => { if (isDead) gameManager.GoBackToMainScreen(); });

		StartPatroling();
    }

	private void OnDisable()
	{
		damageController.ReceiveDmgEvent.RemoveListener(SuspendActions);
	}

	private void OnDestroy()
	{
        if (unlockable != null) unlockable.SetActive(true);
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
