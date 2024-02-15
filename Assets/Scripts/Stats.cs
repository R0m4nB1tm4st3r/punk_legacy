using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats for Damagable Objects", order = 1)]
public class Stats : ScriptableObject
{
	[field: SerializeField]
	public float CurrentHp { get; set; }
	[field: SerializeField]
	public float MaxHp { get; set; }
	[field: SerializeField]
	public float Atk { get; set; }
	[field: SerializeField]
	public float Def { get; set; }
}
