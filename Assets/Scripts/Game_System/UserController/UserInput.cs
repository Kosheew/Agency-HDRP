using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using System;
namespace InputActions
{
	public class UserInput : MonoBehaviour
	{
		[Header("Character Input Values")]
		private Vector2 _move;
		private Vector2 _look;
		private Vector2 _mousePosition;
		private float _scroll;
		
		[SerializeField] private bool jump;
		[SerializeField] private bool sprint;
		[SerializeField] private bool crouch;
		[SerializeField] private bool fire;
		[SerializeField] private bool reload;
		[SerializeField] private bool pause;
		[SerializeField] private bool use;
		
		[Header("Movement Settings")]
		[SerializeField] private bool analogMovement;

		[Header("Mouse Cursor Settings")]
		[SerializeField] private bool cursorLocked = true;
		[SerializeField] private bool cursorInputForLook = true;

		private bool _pauseTriggered = false;
		
		public event Action<float> OnWeaponScroll;
		public event Action<bool> OnPaused;
		
		public Vector2 Move => _move;
		public Vector2 Look => _look;
		public Vector2 MousePosition => _mousePosition;
		public float Scroll => _scroll; 
		public bool Jump => jump;
		public bool Sprint => sprint;
		public bool Crouch => crouch;
		public bool AnalogMovement => analogMovement;
		public bool Fire => fire;
		public bool Reload => reload;
		public bool Pause => pause;
		public bool Use => use;
		
		public void OnMove(InputValue value) => _move = value.Get<Vector2>();
		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				_look = value.Get<Vector2>();
			}
		}

		public void OnJump(InputValue value) => jump = value.isPressed;
		public void OnSprint(InputValue value) => sprint = value.isPressed;
		public void OnCrouched(InputValue value) => crouch = value.isPressed;
		public void OnFire(InputValue value) => fire = value.isPressed;
		public void OnReload(InputValue value) => reload = value.isPressed;
		public void OnMousePosition(InputValue value) => _mousePosition = value.Get<Vector2>();
		public void OnUse(InputValue value) => use = value.isPressed;
		
		public void OnScroll(InputValue value)
		{
			_scroll = value.Get<Vector2>().y;
			OnWeaponScroll?.Invoke(_scroll);
		}

		public void OnPause(InputValue value)
		{
			if (!_pauseTriggered && value.isPressed)  
			{
				pause = !pause;  
				OnPaused?.Invoke(pause);  
				_pauseTriggered = true;  
			}
			else if (!value.isPressed)  
			{
				_pauseTriggered = false;
			}
		}
		
		public void ResetInput()
		{
			crouch = false;
			jump = false;
		}
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
