using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsContainer))]
public abstract class DamagableObject : MonoBehaviour, IDamagable
{
	public float CurrentHp { get => statsContainer.CurrentHp; set => statsContainer.CurrentHp = value; }
	public float MaxHp { get => statsContainer.MaxHp; set => statsContainer.MaxHp = value; }
	public float Atk { get => statsContainer.Atk; set => statsContainer.Atk = value; }
	public float Def { get => statsContainer.Def; set => statsContainer.Def = value; }
	public UnityEvent<bool> DieEvent { get; protected set; }

	protected StatsContainer statsContainer = null;

	protected bool isDead = false;

	protected void Start()
	{
		statsContainer = GetComponent<StatsContainer>();

		DieEvent = new UnityEvent<bool>();

		Debug.Log($"Max HP: {MaxHp}");
		Debug.Log($"Current HP: {CurrentHp}");
		Debug.Log($"Atk: {Atk}");
		Debug.Log($"Def: {Def}");
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

		if (CurrentHp <= 0) Die();
	}
}
