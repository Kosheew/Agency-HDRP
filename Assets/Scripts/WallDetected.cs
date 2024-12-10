using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetected : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    [Range(-1f, 1f)]
    [SerializeField] private int wallDetected;
    
    [SerializeField] private Transform leftHandTarget; // Точка контакту для лівої руки
    [SerializeField] private Transform rightHandTarget; // Точка контакту для правої руки
    private bool isTouchingWall = false;
    
    [SerializeField] private Transform startPositionModel;
    private Quaternion _startRotation;
    
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

            // Розрахунок розвороту персонажа спиною до стіни
            Vector3 wallNormal = other.transform.forward; // Напрямок нормалі стіни
            Vector3 characterPosition = transform.position;
            
            
            // Позиціювання персонажа спиною до стіни
            Quaternion targetRotation = Quaternion.LookRotation(wallDetected * wallNormal, Vector3.up);
            transform.parent.rotation = targetRotation;

            // Отримання точок для IK
            //  leftHandTarget = other.transform.Find("LeftHandPoint");
            // rightHandTarget = other.transform.Find("RightHandPoint");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            isTouchingWall = false;
            animator.SetBool("StandWall", false);
            startPositionModel.eulerAngles = _startRotation.eulerAngles;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isTouchingWall)
        {
            // Налаштування позиції для лівої руки
            if (leftHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
            }

            // Налаштування позиції для правої руки
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
