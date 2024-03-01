using UnityEngine;

public class HitBoxController : DamageDealerObject
{
    [field: SerializeField]
    public float Power { get; set; } = 60;
    [field: SerializeField]
    public float DmgModificator { get; set; } = 0.75f;

	public override float RawDmg { get => (Power + stats.Atk) * DmgModificator; }

	private StatsContainer stats = null;
    private InputController inputController = null;
    private float localPositionOffset;

    void Start()
    {
        localPositionOffset = transform.localPosition.x;
        stats = transform.parent.gameObject.GetComponent<StatsContainer>();
        Debug.Log($"atk in parent: {stats.Atk}");

        inputController = FindObjectOfType<InputController>();
        inputController.StartMoveEvent.AddListener(PlaceHitbox);
    }

    private void PlaceHitbox()
    {
        if (inputController.MoveVector.x < 0) 
		    transform.position = new Vector2(
				transform.parent.position.x - localPositionOffset,
				transform.position.y);
        else
			transform.position = new Vector2(
				transform.parent.position.x + localPositionOffset,
				transform.position.y);
	}
}
