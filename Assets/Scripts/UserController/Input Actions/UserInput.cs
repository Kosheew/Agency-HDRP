using UnityEngine;
using UnityEngine.InputSystem;
using UserController;

namespace InputActions
{
	public class UserInput : MonoBehaviour, IUserInputs
	{
		[Header("Character Input Values")]
		private Vector2 _move;
		private Vector2 _look;
		private bool _jump;
		private bool _sprint;
		private bool _crouch;

		[Header("Movement Settings")]
		[SerializeField] private bool analogMovement;

		[Header("Mouse Cursor Settings")]
		[SerializeField] private bool cursorLocked = true;
		[SerializeField] private bool cursorInputForLook = true;

		public Vector2 Move => _move;
		public Vector2 Look => _look;
		public bool Jump => _jump;
		public bool Sprint => _sprint;
		public bool Crouch => _crouch;
		public bool AnalogMovement => analogMovement;

		public void OnMove(InputValue value) => _move = value.Get<Vector2>();
		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				_look = value.Get<Vector2>();
			}
		}

		public void OnJump(InputValue value) => _jump = value.isPressed;
		public void OnSprint(InputValue value) => _sprint = value.isPressed;
		public void OnCrouched(InputValue value) => _crouch = value.isPressed;

		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				SetCursorState(cursorLocked);
			}
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !newState;
		}
	}
	
}
