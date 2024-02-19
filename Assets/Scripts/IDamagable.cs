public interface IDamagable
{
	public float CurrentHp { get; set; }
	public float MaxHp { get; set; }
	public float Atk { get; set; }
	public float Def { get; set; }

	public void ReceiveDamage(float dmg);
    public void Heal(float hp);
    public void Die();
}
