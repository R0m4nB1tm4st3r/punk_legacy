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
	public bool HasPunchInput { get; private set; } = false;
	[field: SerializeField]
	public bool HasJumpInput { get; set; } = false;
	[field: SerializeField]
	public bool HasInteractInput { get; private set; } = false;

	public UnityEvent StartMoveEvent { get; private set; } = null;
	public UnityEvent StopMoveEvent { get; private set; } = null;
	public UnityEvent<bool> PunchEvent { get; private set; } = null;
    public UnityEvent<bool> JumpEvent { get; private set; } = null;
    public UnityEvent<bool> InteractEvent { get; private set; } = null;

	private void Awake()
	{
		StartMoveEvent = new UnityEvent();
		StopMoveEvent = new UnityEvent();
		PunchEvent = new UnityEvent<bool>();
		JumpEvent = new UnityEvent<bool>();
		InteractEvent = new UnityEvent<bool>();

		StartMoveEvent.AddListener(LogStartMove);
		StopMoveEvent.AddListener(LogStopMove);
		PunchEvent.AddListener(LogPunch);
		JumpEvent.AddListener(LogJump);
		InteractEvent.AddListener(LogInteract);
	}

	private void OnDisable()
	{
		StartMoveEvent.RemoveAllListeners();
		StopMoveEvent.RemoveAllListeners();
		PunchEvent.RemoveAllListeners();
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

	private void LogPunch(bool isPunching)
    {
        Debug.Log($"is punching: {isPunching}");
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

	public void OnPunch(InputAction.CallbackContext context)
	{
		HasPunchInput = context.ReadValueAsButton();
        PunchEvent.Invoke(HasPunchInput);
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
