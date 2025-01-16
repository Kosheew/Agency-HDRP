using System;
using UnityEngine;

public enum StandardVector
{
    Up,
    Down,
    Left,
    Right,
    Forward,
    Back,
    Zero
}

public class WallDetected : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Vector3 _dir;
    [SerializeField] private StandardVector vectorChoice;

    [SerializeField] private Transform leftHandTarget; // Точка контакту для лівої руки
    [SerializeField] private Transform rightHandTarget; // Точка контакту для правої руки
    private bool isTouchingWall = false;

    [SerializeField] private Transform startPositionModel;
    private Quaternion _startRotation;

    private void OnValidate()
    {
        _dir = GetVector(vectorChoice);
    }

    private Vector3 GetVector(StandardVector choice)
    {
        return choice switch
        {
            StandardVector.Up => Vector3.up,
            StandardVector.Down => Vector3.down,
            StandardVector.Left => Vector3.left,
            StandardVector.Right => Vector3.right,
            StandardVector.Forward => Vector3.forward,
            StandardVector.Back => Vector3.back,
            StandardVector.Zero => Vector3.zero,
            _ => Vector3.zero,
        };
    }

    private void Start()
    {
        _startRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            isTouchingWall = true;

            // Увімкнення анімації
            animator.SetBool("StandWall", true);

            // Визначення повороту персонажа
            RotateCharacterToWall(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            isTouchingWall = false;

            // Вимкнення анімації
            animator.SetBool("StandWall", false);

            // Повернення початкового обертання
            startPositionModel.eulerAngles = _startRotation.eulerAngles;
        }
    }

    private void RotateCharacterToWall(Transform wallTransform)
    {
        // Отримання нормалі поверхні стіни
        Vector3 wallNormal = -wallTransform.forward; // Персонаж має стояти спиною до стіни

        // Розрахунок цільового обертання
        Quaternion targetRotation = Quaternion.LookRotation(wallNormal, _dir);

        // Застосування обертання до персонажа
        startPositionModel.rotation = targetRotation;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isTouchingWall)
        {
            // Налаштування IK для лівої руки
            if (leftHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
            }

            // Налаштування IK для правої руки
            if (rightHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
            }
        }
        else
        {
            // Скидання IK
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }
    }
}
