
public class TestDamagable : DamagableObject
{
	public override void ReceiveDamage(float dmg)
	{
		base.ReceiveDamage(dmg);
		
		if (CurrentHp <= 0)
		{
			Destroy(gameObject);
		}
	}
}
