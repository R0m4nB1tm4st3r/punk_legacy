using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [field: SerializeField]
    public Vector2 MoveVector { get; private set; } = Vector2.zero;
	[field: SerializeField]
	public Vector2 AimVector { get; private set; } = Vector2.zero;
	[field: SerializeField]
	public bool HasFireInput { get; private set; } = false;
	[field: SerializeField]
	public bool HasJumpInput { get; set; } = false;
	[field: SerializeField]
	public bool HasInteractInput { get; private set; } = false;

	public UnityEvent StartMoveEvent { get; private set; } = null;
	public UnityEvent StopMoveEvent { get; private set; } = null;
	public UnityEvent<bool> FireEvent { get; private set; } = null;
    public UnityEvent<bool> JumpEvent { get; private set; } = null;
    public UnityEvent<bool> InteractEvent { get; private set; } = null;

	private void Awake()
	{
		StartMoveEvent = new UnityEvent();
		StopMoveEvent = new UnityEvent();
		FireEvent = new UnityEvent<bool>();
		JumpEvent = new UnityEvent<bool>();
		InteractEvent = new UnityEvent<bool>();

		StartMoveEvent.AddListener(LogStartMove);
		StopMoveEvent.AddListener(LogStopMove);
		FireEvent.AddListener(LogFire);
		JumpEvent.AddListener(LogJump);
		InteractEvent.AddListener(LogInteract);
	}

	private void OnDisable()
	{
		StartMoveEvent.RemoveAllListeners();
		StopMoveEvent.RemoveAllListeners();
		FireEvent.RemoveAllListeners();
		JumpEvent.RemoveAllListeners();
		InteractEvent.RemoveAllListeners();
	}

	private void LogStartMove()
	{
		Debug.Log($"start move: {MoveVector.x}");
	}

	private void LogStopMove()
	{
		Debug.Log($"stop move: {MoveVector.x}");
	}

	private void LogFire(bool isFiring)
    {
        Debug.Log($"is firing: {isFiring}");
    }

	private void LogJump(bool isJumping)
	{
		Debug.Log($"is jumping: {isJumping}");
	}

	private void LogInteract(bool isInteracting)
	{
		Debug.Log($"is interacting: {isInteracting}");
	}

	public void OnMove(InputAction.CallbackContext context)
    {
        MoveVector = context.ReadValue<Vector2>();

		if (context.performed)
		{
			StartMoveEvent.Invoke();
		}
		else if (context.canceled)
		{
			StopMoveEvent.Invoke();
		}
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimVector  = context.ReadValue<Vector2>();
    }

	public void OnFire(InputAction.CallbackContext context)
	{
		HasFireInput = context.ReadValueAsButton();
        FireEvent.Invoke(HasFireInput);
	}

    public void OnJump(InputAction.CallbackContext context)
    {
        HasJumpInput = context.ReadValueAsButton();
        JumpEvent.Invoke(HasJumpInput);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        HasInteractInput = context.ReadValueAsButton();
        InteractEvent.Invoke(HasInteractInput);
    }
}
