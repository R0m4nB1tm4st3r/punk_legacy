using System.Collections;
using UnityEngine;

public class DoorCloser : MonoBehaviour
{
	private const string DoorTagPrefix = "Door-";
	private const string PlayerTag = "Player";

	[field: SerializeField]
	public string DoorId { get; set; } = "02";
	[field: SerializeField]
	public float ClosingHeight { get; set; } = 9f;
	[field: SerializeField]
	public float ClosingSpeed { get; set; } = 8f;

	private BoxCollider2D boxCollider = null;
	private GameObject doorToClose = null;
	private IEnumerator closeDoorCoroutine = null;

	void Start()
    {
        doorToClose = GameObject.FindWithTag($"{DoorTagPrefix}{DoorId}");
		boxCollider = GetComponent<BoxCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag(PlayerTag))
		{
			boxCollider.enabled = false;

			closeDoorCoroutine = CloseDoor();
			StartCoroutine(closeDoorCoroutine);
		}
	}

	private IEnumerator CloseDoor()
	{
		float targetHeight = doorToClose.transform.position.y - ClosingHeight;
		Vector2 targetPosition = new(doorToClose.transform.position.x, targetHeight);

		while (doorToClose.transform.position.y > targetHeight)
		{
			doorToClose.transform.position =
				Vector2.MoveTowards(doorToClose.transform.position, targetPosition, ClosingSpeed * Time.deltaTime);

			yield return null;
		}

		Destroy(gameObject);
	}
}
