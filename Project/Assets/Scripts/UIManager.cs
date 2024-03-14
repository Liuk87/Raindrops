using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Spawner;

public class UIManager : MonoSingleton<UIManager>
{
    public GameObject MenuPanel;
    public GameObject GamePanel;
    public GameObject GameOverPanel;
    public GameObject LeaderBoardPanel;
    public AudioSource StartingTrack;

    public enum eGameState
    {
        InMenu,
        InGame,
        GameOver,
        LeaderboardMenu
    }

    public eGameState GameState = eGameState.InMenu;

    private void Start()
    {
        GoToState(eGameState.InMenu);
        Time.timeScale = 1f;
    }

    public void GoToState(eGameState _state)
    {
        GameState = _state;
        HideAllPanels();

        switch (GameState)
        {

            case eGameState.InMenu:
                PauseManager.Instance.PauseGame();
                MenuPanel.SetActive(true);

                break;

            case eGameState.InGame:

                GamePanel.SetActive(true);
                PauseManager.Instance.ResumeGame();  
                
                ScoreSystem.Instance.HPText.text ="HP: "+ Player.Instance.Health.ToString();
                ScoreSystem.Instance.scorePointsText.text = Player.Instance.score.ToString();
                break;

            case eGameState.GameOver:
                PauseManager.Instance.PauseGame();
                ScoreSystem.Instance.ClearBoard();
                GameOverPanel.SetActive(true);
                GameOverPanel.GetComponentInChildren<TMP_Text>().text = "Your SCORE is: " + Player.Instance.score.ToString(); 
                
                break;

            case eGameState.LeaderboardMenu:
                LeaderBoardPanel.SetActive(true);
                break;

        }
    }

    
    public void StartNewGame()
    {
        if (NickNameHandler.Instance.IsNameSet == true)
        {
            Player.Instance.Health = Player.Instance.startingHealth;
            Player.Instance.score = 0;
            Player.Instance.IsGameOver = false;
            ScoreSystem.Instance.ClearBoard();
            Spawner.Instance.currentDifficulty = DifficultyLevel.Easy;
            Spawner.Instance.UpdateDifficultyText(Spawner.Instance.currentDifficulty);
            GoToState(eGameState.InGame);
            StartingTrack.Play();
        }
    }

    public void EnterMenu(int _stateValue)
    {
        eGameState state = (eGameState)_stateValue;
        GoToState(state);
    }

    public void HideAllPanels()
    {
        MenuPanel.SetActive(false);
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        LeaderBoardPanel.SetActive(false);
    }

    public void ExitGame() //To implement also the save function
    {
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // This exits Play mode in the Unity Editor
        #else
        Application.Quit(); // This exits the application when not in the Unity Editor
        #endif
        }
    
}
