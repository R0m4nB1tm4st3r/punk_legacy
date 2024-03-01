using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsContainer))]
public abstract class DamagableObject : MonoBehaviour, IDamagable
{
	[field: SerializeField]
	public float RecoveryTimeSeconds { get; set; } = 1f;

	public float CurrentHp { get => statsContainer.CurrentHp; set => statsContainer.CurrentHp = value; }
	public float MaxHp { get => statsContainer.MaxHp; set => statsContainer.MaxHp = value; }
	public float Atk { get => statsContainer.Atk; set => statsContainer.Atk = value; }
	public float Def { get => statsContainer.Def; set => statsContainer.Def = value; }

	public UnityEvent ReceiveDmgEvent { get; protected set; } = null;
	public UnityEvent<bool> DieEvent { get; protected set; } = null;

	protected StatsContainer statsContainer = null;

	protected bool isDead = false;

	protected void Awake()
	{
		ReceiveDmgEvent = new();
		DieEvent = new UnityEvent<bool>();
	}

	protected void Start()
	{
		statsContainer = GetComponent<StatsContainer>();
	}

	public void Die()
	{
		isDead = true;
		DieEvent.Invoke(isDead);
	}

	public void Heal(float hp)
	{
		CurrentHp = Mathf.Clamp(CurrentHp + hp, CurrentHp, MaxHp);
	}

	public virtual void ReceiveDamage(float dmg)
	{
		CurrentHp = Mathf.Clamp(CurrentHp - dmg, 0, MaxHp);
		ReceiveDmgEvent.Invoke();

		if (CurrentHp <= 0) Die();
	}
}
