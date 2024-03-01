using UnityEngine;
using UnityEngine.Events;

public abstract class DamageDealerObject : MonoBehaviour, IDamageDealer
{
	[field: SerializeField]
	public float AtkRange { get; set; } = 0;
	[field: SerializeField]
	public LayerMask AtkMask { get; set; }

	public abstract float RawDmg { get; }
	public UnityEvent DealtDamageEvent { get; protected set; } = null;

	protected void Awake()
	{
		DealtDamageEvent = new UnityEvent();
	}

	public void DealDamage(DamagableObject target, float rawDmg)
	{
		float cleanDmg = Mathf.Clamp(rawDmg - target.Def, 1, rawDmg);

		target.ReceiveDamage(cleanDmg);

		DealtDamageEvent.Invoke();
	}

	public void Attack()
	{
		Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, AtkRange, AtkMask);

		foreach (Collider2D obj in hitObjects)
		{
			if (!obj.isTrigger && obj.TryGetComponent(out DamagableObject damagable))
			{
				damagable.ReceiveDamage(Mathf.Clamp(RawDmg - damagable.Def, 1, RawDmg));
			}
		}
	}
}
