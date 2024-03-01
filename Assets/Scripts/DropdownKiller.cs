using UnityEngine;

public class DropdownKiller : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<DamagableObject>(out var damagable))
		{
			damagable.ReceiveDamage(damagable.MaxHp);
		}
	}
}
