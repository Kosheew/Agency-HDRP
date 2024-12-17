using UnityEngine.InputSystem;

namespace UserController
{
  public interface IUserInputs
  {
    public void OnMove(InputValue value);

    public void OnLook(InputValue value);

    public void OnJump(InputValue value);

    public void OnSprint(InputValue value);

    public void OnCrouched(InputValue value);
  }
}
