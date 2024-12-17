using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the behavior of an obstacle zone in the game.
/// Controls the Rigidbody's kinematic state when a collider enters the trigger zone.
/// </summary>

namespace Obstacle
{
    public class ObstacleZone : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_rb; // The Rigidbody component associated with the obstacle zone.
        [SerializeField] private AudioClip _activateSound; // Sound for obstacle activation
        private AudioManager _audioManager; // Reference to AudioManager

        /// <summary>
        /// Initializes the obstacle zone by setting the Rigidbody to kinematic state.
        /// </summary>
        private void Start()
        {
            m_rb.isKinematic = true; // Set the Rigidbody to kinematic at the start.
         
        }

        /// <summary>
        /// Called when another collider enters the trigger zone.
        /// Changes the Rigidbody state to non-kinematic when triggered.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            m_rb.isKinematic = false; // Update the Rigidbody's kinematic state.
            _audioManager.PlaySound(_activateSound);
        }
    }
}
