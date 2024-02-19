using UnityEngine;

public class StatsContainer : MonoBehaviour
{
	private const string MissingStatsMessage = "Stats are not set!!";

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

	// Start is called before the first frame update
	void Start()
    {
		if (InitialStats != null)
		{
			CurrentHp = InitialStats.CurrentHp;
			MaxHp = InitialStats.MaxHp;
			Atk = InitialStats.Atk;
			Def = InitialStats.Def;
		}
		else
		{
			throw new MissingReferenceException(MissingStatsMessage);
		}
		
	}
}
