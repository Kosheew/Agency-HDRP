﻿using UnityEngine;

namespace CharacterSettings
{
    public abstract class CharacterSetting: ScriptableObject
    {
        [Header("Character Speed")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float turnSpeed;
        [SerializeField] private float sprintSpeed = 5.335f;
        [SerializeField] private float speedChangeRate = 10f;
        
        [Header("Character Health")]
        [SerializeField] private int health = 100;
        
        [SerializeField] private CharacterAudioSettings characterAudioSettings;
        
        public float MoveSpeed => moveSpeed;
        public float TurnSpeed => turnSpeed;
        public float SprintSpeed => sprintSpeed;
        public float SpeedChangeRate => speedChangeRate;
        public int Health => health;
        
        public CharacterAudioSettings CharacterAudioSettings => characterAudioSettings;
    }
}