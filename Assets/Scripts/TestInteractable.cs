using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : InteractableObject
{
	public override float InteractRange => throw new System.NotImplementedException();

	public override void Interact(bool hasInteractInput)
	{
		if (hasInteractInput) Debug.Log("I am interacting");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		base.OnTriggerEnter2D(collision);

		GameObject go = collision.gameObject;

		if (go.TryGetComponent<DamagableObject>(out var damagable))
		{
			Debug.Log("YOU SHALL NOT PASS!!!!!!");
			damagable.ReceiveDamage(60);
		}
	}
}
