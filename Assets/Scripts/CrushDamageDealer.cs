using System.Data;
using UnityEngine;

public class CrushDamageDealer : DamageDealerObject
{
	private const string PlayerTag = "Player";

	[field: SerializeField]
	public float Power { get; set; } = 30;
	[field: SerializeField]
	public float DmgModificator { get; set; } = 1f;

	public override float RawDmg { get => (Power + stats.Atk) * DmgModificator; }

	private StatsContainer stats = null;

	void Start()
    {
        stats = GetComponent<StatsContainer>();
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag(PlayerTag) && 
			collision.gameObject.TryGetComponent<DamageController>(out var damageController))
		{
			damageController.ReceiveDamage(Mathf.Clamp(RawDmg - damageController.Def, 1, RawDmg));
		}
	}
}
