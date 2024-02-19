using UnityEngine;

public class TestInteractable : InteractableObject
{
	public override float InteractRange => throw new System.NotImplementedException();

	public override void Interact(bool hasInteractInput)
	{
		if (hasInteractInput) Debug.Log("I am interacting");
	}
}
