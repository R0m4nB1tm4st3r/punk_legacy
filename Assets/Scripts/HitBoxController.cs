using UnityEngine;

public class HitBoxController : DamageDealerObject
{
    [field: SerializeField]
    public float Power { get; set; } = 60;
    [field: SerializeField]
    public float DmgModificator { get; set; } = 0.75f;

	public override float RawDmg { get => (Power + stats.Atk) * DmgModificator; }

	private StatsContainer stats = null;

    void Start()
    {
        stats = transform.parent.GetComponent<StatsContainer>();
        Debug.Log($"atk in parent: {stats.Atk}");
    }
}
