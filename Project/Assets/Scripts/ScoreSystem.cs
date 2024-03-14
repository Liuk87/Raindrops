using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class ScoreSystem : MonoSingleton<ScoreSystem>
{
    public TMP_InputField inputField;
    public TMP_Text scorePointsText;
    public TMP_Text HPText;
    //public List<string> operations;
    public int pointsPerCorrectAnswer = 100;
    public GameObject GoldenObject;
    //public Collider2D MainPanelCollider;
    private Spawner spawner;

    private void Start()
    {
        spawner = Spawner.Instance;
        

        if (UIManager.Instance.GameState == UIManager.eGameState.InGame)
        {
            inputField.Select();

            
            inputField.onEndEdit.AddListener(delegate { CheckOperationResult(); });
        }

        
        
        HPText.text += Player.Instance.startingHealth.ToString();
       
    }
    private void Update()
    {
        if (UIManager.Instance.GameState == UIManager.eGameState.InGame)
        {
            //  Debug.Log(inputField.isFocused);
            if (inputField.isFocused == false) { inputField.Select(); }

            
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                
                CheckOperationResult();
            }
        }
    }

    

    private void CheckOperationResult()
    {
        
        
        string userInput = inputField.text;

        if (spawner.results.Contains(userInput))
        {
            int numberOfSameResults = 0;

            List<string> operationsToRemove = new List<string>();
            
            

            foreach (string result in spawner.results)
            {
                if (result.Equals(userInput))
                {
                    numberOfSameResults++;
                    Debug.Log("num same op:" + numberOfSameResults);
                    operationsToRemove.Add(result);
                }
            }
            foreach (string operationToRemove in operationsToRemove)
            {
                spawner.results.Remove(operationToRemove);
            }

            //Spawner.Instance.RemoveMatchingResults(userInput);

            
            AddPoints(pointsPerCorrectAnswer*numberOfSameResults);

            GameObject[] ContainerToMove = Spawner.Instance.GetContainerWithMatchingResult(userInput);
            if (ContainerToMove != null)
            {
                foreach (GameObject container in ContainerToMove)
                {
                    if (container == GoldenObject)
                    {
                        Debug.Log("Golden Explosion");
                        GoldenExplosion();
                    }
                    else
                    {
                        PoolManager.Instance.MoveToReserve(container);
                        
                    }
                }
            }
            
            Debug.Log("Risposta corretta! Hai ottenuto " + pointsPerCorrectAnswer + " punti.");
        }
        else
        {
            
            Debug.Log("Risposta errata. Riprova!");
        }

        
        inputField.text = "";

        inputField.ActivateInputField();
    }

    private void AddPoints(int _points)
    {
        Player.Instance.score += _points;
        string totalscore = Player.Instance.score.ToString();
        scorePointsText.text = totalscore;
 
    }

    

    public void RemoveHealth (int _points)
    {
        
        Player.Instance.Health -= _points;
        //Debug.Log(Player.Instance.Health);
        HPText.text = "HP: " + Player.Instance.Health.ToString();

    }


    public void ClearBoard()
    {
        foreach (GameObject obj in spawner.spawnList)
        {
            Destroy(obj);
        }
        foreach(GameObject obj in PoolManager.Instance.reserveObject)
        {
            Destroy(obj);
        }
        spawner.spawnList.Clear();
        spawner.results.Clear();
        PoolManager.Instance.reserveObject.Clear();
       
    }


    public void GoldenExplosion()
    {
        
        int i = 0;
        foreach (GameObject obj in spawner.spawnList)
        {
            i++;
            Destroy(obj);
           // PoolManager.Instance.MoveToReserve(obj);
        }

        
        AddPoints(i*100);
        spawner.spawnList.Clear();
        spawner.results.Clear();
        
    }


}
