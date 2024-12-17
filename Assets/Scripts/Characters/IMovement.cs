using UnityEngine;
public interface IMovement
{
    public void RotateCharacter(float lookX, float lookY, Transform bodyTransform, Transform camera);
    public Vector3 MoveCharacter(float horizontal, float vertical, Transform bodyTransform);
    public Vector3 AddGravity();
}
