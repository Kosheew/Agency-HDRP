using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseView : MonoBehaviour
{
      [SerializeField] private GameObject pausePanel;

      public void Pause(bool pause)
      {
            pausePanel.SetActive(pause);
            Time.timeScale = pause ? 0 : 1;
      }
}
