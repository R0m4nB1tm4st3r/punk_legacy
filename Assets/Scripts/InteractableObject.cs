using TMPro;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public abstract class InteractableObject : MonoBehaviour
{
    public abstract float InteractRange { get; }
    public virtual string InteractText { get; } = "Interact";

    protected InputController inputController = null;
    protected Canvas interactCanvas = null;
    protected TextMeshProUGUI textPrompt = null;

    public abstract void Interact(bool hasInteractInput);

    protected void Start()
	{
		InitializeInteractable();
	}

	protected virtual void InitializeInteractable()
	{
		inputController = FindObjectOfType<InputController>();

		interactCanvas = transform.GetChild(0).GetComponent<Canvas>();
		
		textPrompt = interactCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		textPrompt.text = InteractText;

		// hide interact canvas by default
		interactCanvas.enabled = false;
	}

	protected void OnTriggerEnter2D(Collider2D other)
    {
        // show interact canvas
        interactCanvas.enabled = true;

		// listen to interact event
		inputController.InteractEvent.AddListener(Interact);
	}

	protected void OnTriggerExit2D(Collider2D other)
	{
		// hide interact canvas
		interactCanvas.enabled = false;

		// unlisten to interact event
		inputController.InteractEvent.RemoveListener(Interact);
	}
}
