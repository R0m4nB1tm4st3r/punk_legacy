using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
	private const string AnimParamVelocityX = "VelocityX";
	private const string AnimParamIsDead = "IsDead";

	private Animator animator = null;
	private SpriteRenderer spriteRenderer = null;
	private EnemyController enemyController = null;
	private DamageController damageController = null;

	void Start()
    {
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		enemyController = GetComponent<EnemyController>();
		damageController = GetComponent<DamageController>();

		damageController.DieEvent.AddListener(UpdateIsDeadParam);
	}

	private void Update()
	{
		animator.SetFloat(AnimParamVelocityX, Mathf.Abs(enemyController.CurrentVelocity.x));
		FlipSprite();
	}

	private void OnDisable()
	{
		damageController.DieEvent.RemoveListener(UpdateIsDeadParam);
	}

	private void FlipSprite()
	{
		if (enemyController.CurrentVelocity.x > 0) spriteRenderer.flipX = false;
		else if (enemyController.CurrentVelocity.x < 0) spriteRenderer.flipX = true;
	}

	private void UpdateIsDeadParam(bool isDead)
	{
		animator.SetBool(AnimParamIsDead, isDead);

		if (isDead)
		{
			enemyController.enabled = false;
		}
	}

	public void DestroyObject()
	{
		Destroy(gameObject);
	}
}
