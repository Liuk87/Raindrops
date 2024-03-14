using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoSingleton<Spawner>
{
    public Vector3 spawnAreaSize = new Vector3(1000f, 5f, 0f);
    public Canvas canvas;
    public GameObject NormalOperationToSpawn;
    public GameObject BinaryOperationToSpawn;
    private GameObject GodenOperationToSpawn;
    [Range(1f, 2f)] public float ObjectScale = 1;

    public bool isSpawning = false;
    public DifficultyLevel currentDifficulty = DifficultyLevel.Easy;
    public List<GameObject> spawnList = new List<GameObject>();
    public List<string> results = new List<string>();
    private bool normalOperationSpawned = false;
    public bool NormalOperationSpawned {get { return normalOperationSpawned; } set { normalOperationSpawned = value; } }
    private bool binaryOperationSpawned = false;
    public bool BinarylOperationSpawned { get { return binaryOperationSpawned; } set { binaryOperationSpawned = value; } }
    private bool goldenOperationSpawned = false;
    public bool GoldenlOperationSpawned { get { return goldenOperationSpawned; } set { goldenOperationSpawned = value; } }
    

    public TMP_Text DifficultyText;
    private string startingDifficultyText;

    [Range(1f, 120f)]
    public float waitTimeSlider = 60f;
    

    void Start()
    {
                
        startingDifficultyText = DifficultyText.text;
        
        UpdateDifficultyText(currentDifficulty);

        StartCoroutine(ManageDifficulty());
    }

    public void SpawnObjectInRectangle()
    {
        
        Vector3 randomPosition = transform.position + new Vector3(Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f), 0, 0f);


        
            GameObject objectToSpawn = PoolManager.Instance.activatePoolItem();

            if (objectToSpawn != null)
            {
            
                objectToSpawn.transform.position = randomPosition;
               // spawnList.Add(objectToSpawn);
            objectToSpawn.GetComponent<ObjectMovement>().StopFalling();
            objectToSpawn.GetComponent<ObjectMovement>().ResumeFalling();
            Debug.Log("Falling Obj: " + objectToSpawn.name);
            }
        
        else
        {

            GameObject spawnedObject = Instantiate(GetRandomOperationType(), randomPosition, Quaternion.identity);
            spawnList.Add(spawnedObject);

            if (canvas != null)
            {
                spawnedObject.transform.SetParent(canvas.transform, true);
            }

            //Correzione transform oggetto
            spawnedObject.transform.localScale = Vector3.one * 100* ObjectScale;
            spawnedObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        
        isSpawning = false;
    }

    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard
    }

      IEnumerator ManageDifficulty()
    {
        while (true)
        {
            // Attendi un certo periodo di tempo prima di passare alla prossima verifica della difficoltà
            yield return new WaitForSeconds(waitTimeSlider);

            
            switch (currentDifficulty)
            {
                case DifficultyLevel.Easy:
                    currentDifficulty = DifficultyLevel.Normal;
                    UpdateDifficultyText(currentDifficulty);
                    Debug.Log("Livello di difficoltà: Normale");
                    break;
                case DifficultyLevel.Normal:
                    currentDifficulty = DifficultyLevel.Hard;
                    UpdateDifficultyText(currentDifficulty);
                    Debug.Log("Livello di difficoltà: Difficile");
                    break;
                case DifficultyLevel.Hard:
                    UpdateDifficultyText(currentDifficulty);
                    Debug.Log("Livello di difficoltà massima raggiunto.");
                    break;
                default:
                    Debug.LogError("Livello di difficoltà non gestito: " + currentDifficulty);
                    break;
            }
        }
    }

    public GameObject[] GetContainerWithMatchingResult(string _result)
    {
        List<GameObject> matchingContainers = new List<GameObject>();

        foreach (GameObject spawnedObject in spawnList)
        {
            
            if (spawnedObject.GetComponent<RandomOperations>() != null)
            {
                
                if (spawnedObject.GetComponent<RandomOperations>().Result == _result)
                {
                    //DA TESTARE BENE
                    if (spawnedObject.GetComponent<RandomOperations>().isGoldenOperation)
                    {
                        ScoreSystem.Instance.GoldenObject = spawnedObject;
                        matchingContainers.Add(spawnedObject);
                        return matchingContainers.ToArray();

                    }
                    else
                    {
                        matchingContainers.Add(spawnedObject);
                    }
                    
                }
            }
            else if (spawnedObject.GetComponent<BinaryOperations>() != null)
            {
                if (spawnedObject.GetComponent<BinaryOperations>().Result == _result)
                {
                    matchingContainers.Add(spawnedObject);
                }
            }
        }

        if (matchingContainers.Count == 0)
        {
            Debug.Log("No results matching in list!");
            return null;
        }

        return matchingContainers.ToArray();
    }

    //public void RemoveMatchingResults(string _result)
    //{
    //    string resultToRemove;
    //    List<string> resultsToRemove = new List<string>(); // Modifica il tipo da List<int> a List<string>

    //    // Rimuovi la conversione a int qui, poiché _result è già una stringa
    //    resultToRemove = _result;

    //    foreach (string result in results)
    //    {
    //        if (result == resultToRemove)
    //        {
    //            resultsToRemove.Add(result);
    //        }
    //    }

    //    foreach (string result in resultsToRemove)
    //    {
    //        results.Remove(result);
    //    }
    //}

    public void RemoveOneMatchingResult(string _result)
    {
        results.Remove(_result);
    }

    public void UpdateDifficultyText(DifficultyLevel _value)
    {
        DifficultyText.text = startingDifficultyText + " " + _value.ToString();
    }

    private GameObject GetRandomOperationType()
    {
        
        int random = Random.Range(1, 11);

        switch (random)
        {
            case int n when (n > 8):

                
                return BinaryOperationToSpawn;


            case int n when (n == 8):

                GodenOperationToSpawn = NormalOperationToSpawn;
                RandomOperations GoldenOperation = GodenOperationToSpawn.GetComponent<RandomOperations>();
                GoldenOperation.isGoldenOperation = true;

                
                GodenOperationToSpawn.GetComponent<SpriteRenderer>().color = GoldenOperation.GoldenColor;

                return GodenOperationToSpawn;


            default:
                
                RandomOperations normalOps = NormalOperationToSpawn.GetComponent<RandomOperations>();
                normalOps.isGoldenOperation = false;
                NormalOperationToSpawn.GetComponent<SpriteRenderer>().color = normalOps.DefaultColor;
                return NormalOperationToSpawn;
                
        }
    }

    

}
