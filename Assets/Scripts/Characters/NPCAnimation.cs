using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private bool isTaking;
    [SerializeField] private bool isSitting;
    [SerializeField] private bool isRest;
    [SerializeField] private bool isTyping;

    private int _animIDTaking;
    private int _animIDSitting;
    private int _animIDRest;
    private int _animIDTyping;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        _animIDTaking = Animator.StringToHash("IsTaking");
        _animIDSitting = Animator.StringToHash("IsSitting");
        _animIDRest = Animator.StringToHash("IsRest");
        _animIDTyping = Animator.StringToHash("IsTyping");
    }

    private void Start()
    {
        animator.SetBool(_animIDTaking, isTaking);
        animator.SetBool(_animIDSitting, isSitting);
        animator.SetBool(_animIDRest, isRest);
        animator.SetBool(_animIDTyping, isTyping);
    }
   
}
