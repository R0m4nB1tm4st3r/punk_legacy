using System.Collections;
using UnityEngine;

public class OpenDoorInteractable : InteractableObject
{
	private const string DoorTagPrefix = "Door-";

    [field: SerializeField]
    public override float InteractRange { get; set; } = 1.5f;
	[field: SerializeField]
	public float OpeningHeight { get; set; } = 4f;
	[field: SerializeField]
	public float OpeningSpeed { get; set; } = 6f;
	[field: SerializeField]
	public string DoorId { get; set; } = "01";

	private GameObject doorToOpen = null;

	private bool hasBeenUsed = false;
	private IEnumerator openDoorCoroutine = null;

	private new void Start()
    {
        base.Start();

		doorToOpen = GameObject.FindWithTag($"{DoorTagPrefix}{DoorId}");
    }

	public override void Interact(bool hasInteractInput)
	{
		if (hasInteractInput && !hasBeenUsed)
		{
			openDoorCoroutine = OpenDoor();
			StartCoroutine( openDoorCoroutine );
		}
	}

	protected override void SwitchButtonPrompt(bool shouldBeEnabled)
	{
		base.SwitchButtonPrompt(shouldBeEnabled && !hasBeenUsed);
	}

	IEnumerator OpenDoor()
	{
		float targetHeight = doorToOpen.transform.position.y + OpeningHeight;
		Vector2 targetPosition = new(doorToOpen.transform.position.x, targetHeight);
		
		while (doorToOpen.transform.position.y < targetHeight)
		{
			doorToOpen.transform.position = 
				Vector2.MoveTowards(doorToOpen.transform.position, targetPosition, OpeningSpeed * Time.deltaTime);

			yield return null;
		}

		hasBeenUsed = true;
	}
}
