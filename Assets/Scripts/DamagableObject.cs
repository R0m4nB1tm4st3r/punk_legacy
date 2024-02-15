using UnityEngine;
using UnityEngine.Events;

public abstract class DamagableObject : MonoBehaviour, IDamagable
{
	[field: SerializeField]
	public Stats InitialStats { get; set; }
	[field: SerializeField]
	public float CurrentHp { get; set; }
	[field: SerializeField]
	public float MaxHp { get; set; }
	[field: SerializeField]
	public float Atk { get; set; }
	[field: SerializeField]
	public float Def { get; set; }

	public UnityEvent<bool> DieEvent { get; protected set; }

	protected bool isDead = false;

	protected void Start()
	{
		CurrentHp = InitialStats.CurrentHp;
		MaxHp = InitialStats.MaxHp;
		Atk = InitialStats.Atk;
		Def = InitialStats.Def;

		DieEvent = new UnityEvent<bool>();

		Debug.Log($"Max HP: {InitialStats.MaxHp}");
		Debug.Log($"Current HP: {InitialStats.CurrentHp}");
		Debug.Log($"Atk: {InitialStats.Atk}");
		Debug.Log($"Def: {InitialStats.Def}");
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

	public void ReceiveDamage(float dmg)
	{
		CurrentHp = Mathf.Clamp(CurrentHp - dmg, 0, MaxHp);

		if (CurrentHp <= 0) Die();
	}
}
