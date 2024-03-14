using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseManager : MonoSingleton<PauseManager>
{
    private bool isPaused = false;
    public bool IsPaused { get { return isPaused; } }
    public GameObject ResumeGameButton;

    
    void Update()
    {
        if (UIManager.Instance.GameState == UIManager.eGameState.InGame)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ResumeGame();
                    

                }
                else
                    UIManager.Instance.EnterMenu(0);
                ResumeGameButton.SetActive(true);
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        UIManager.Instance.StartingTrack.Pause();

        foreach (GameObject obj in Spawner.Instance.spawnList)
        {
            if (obj != null)
            {
                ObjectMovement objectMovement;
                objectMovement = obj.GetComponent<ObjectMovement>();

                objectMovement.StopFalling();
            }
        }
    }

    
        public void ResumeGame()
        {
            Time.timeScale = 1f;
            isPaused = false;
            UIManager.Instance.StartingTrack.UnPause();

        foreach (GameObject obj in Spawner.Instance.spawnList)
        {
            if (obj != null)
            {
                ObjectMovement objectMovement;
                objectMovement = obj.GetComponent<ObjectMovement>();
                Debug.Log(obj);
                objectMovement.ResumeFalling();
            }
        }
        ResumeGameButton.SetActive(false);
    }

    }
