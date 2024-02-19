using UnityEngine;

public interface IDamageDealer
{
	public float AtkRange { get; set; }
	public LayerMask AtkMask { get; set; }
	public void Attack();
}
