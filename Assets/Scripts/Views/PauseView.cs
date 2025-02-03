using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseView : MonoBehaviour
{
      [SerializeField] private GameObject pausePanel;

      public void Pause(bool pause)
      {
            pausePanel.SetActive(pause);
            Time.timeScale = pause ? 0 : 1;
      }

      public void LoadMenu()
      {
            SceneManager.LoadScene(0);
            Pause(false);
      }
}
