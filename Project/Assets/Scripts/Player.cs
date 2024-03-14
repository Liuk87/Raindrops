using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    
    private int health;
    public int Health { get { return health; }set { health = value; } }
    public string nickname;
    public int score;
    public List<int> operationsToPerform;

    private UIManager uiManager;
    private bool isGameOver = false;
    public bool IsGameOver { get { return isGameOver; } set { isGameOver = value; } }

    public int startingHealth;

    private void Start()
    {
        
        Health = startingHealth;
        score = 0;
        uiManager = UIManager.Instance;
    }
    private void Update()
    {
        
        if (!isGameOver && health <= 0)
        {
            health = 0;
            isGameOver = true;
            uiManager.GoToState(UIManager.eGameState.GameOver);
            Debug.Log("GAME OVER!");
        }
    }
}
