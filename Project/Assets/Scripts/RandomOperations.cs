using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomOperations : MonoBehaviour
{

    
    private string difficulty;
    public string Difficulty { set { difficulty = value; } get { return difficulty; } }
    private int topNumber;
    private int bottomNumber;
    private int min;
    private int max;
    private int result;
    public string Result { get { return result.ToString(); } }
    private ObjectMovement objectMovement;
    private char operation;
    public bool isGoldenOperation = false;
    public Color DefaultColor;
    private SpriteRenderer spriteRenderer;
    private TMP_Text TMPtext;
    public Color GoldenColor;


    void Start()
    {
        

        spriteRenderer = this.GetComponent<SpriteRenderer>();
        TMPtext = this.GetComponentInChildren<TextMeshPro>();

        if (isGoldenOperation)
        {
            spriteRenderer.color = GoldenColor;
            TMPtext.color = Color.white;
            GetComponent<ObjectMovement>().speed = 2;
            Spawner.Instance.GoldenlOperationSpawned = true;

            //TMPtext.font.material = spriteRenderer.material;
        }
        else
        {
            spriteRenderer.color = DefaultColor;
            Spawner.Instance.NormalOperationSpawned = true;
        }


        PerformOperation();

        
       // Debug.Log(topNumber + " " + operation + " " + bottomNumber + " = " + result);

        //SetText  (topNumber,operation,bottomNumber);

        Spawner.Instance.results.Add (result.ToString());
    }

  

    private bool IsInteger(int dividend, int divisor, int quotient)
    {
        return dividend % divisor == 0;
    }
    private char GetRandomOperation()
    {
        // Array contenente le possibili operazioni
        char[] operations = { '+', '-', '*', '/' };

        // Restituisce casualmente un'operazione dall'array
        return operations[Random.Range(0, operations.Length)];
    }

    private void SetText(int _topNumber, char _operation, int _bottomNumber)
    {

        string text = string.Concat(_topNumber.ToString() , "\n" , _operation.ToString() , "\n" , _bottomNumber.ToString());
        TMPtext.text = text;
    }

    public void PerformOperation()
    {
        difficulty = Spawner.Instance.currentDifficulty.ToString();
        objectMovement = GetComponent<ObjectMovement>();
        //Debug.Log(objectMovement.speed);

        switch (difficulty)
        {
            case "Easy":
                min = 1; max = 10;
                break;
            case "Normal":
                min = 1; max = 15;
                objectMovement.speed = .75f;

                break;
            case "Hard":
                min = 1; max = 20;
                objectMovement.speed = 1f;
                break;
        }

        topNumber = Random.Range(min, max);
        bottomNumber = Random.Range(min, max);


        operation = GetRandomOperation();


        switch (operation)
        {
            case '+':
                result = topNumber + bottomNumber;

                break;
            case '-':
                result = topNumber - bottomNumber;
                break;
            case '*':
                result = topNumber * bottomNumber;
                break;
            case '/':

                if (bottomNumber != 0)
                {

                    result = topNumber / bottomNumber;


                    if (IsInteger(topNumber, bottomNumber, result))
                    {
                        break;
                    }
                }
                else
                {
                    Debug.LogWarning("Second number cannot be zero for division operation.");
                }

                // Se il risultato non è intero, ripeti l'assegnazione dei numeri casuali
                do
                {
                    topNumber = Random.Range(min, max);
                    bottomNumber = Random.Range(min, max);
                } while (bottomNumber == 0 || !IsInteger(topNumber, bottomNumber, topNumber / bottomNumber));


                result = topNumber / bottomNumber;
                break;

            default:
                Debug.LogWarning("Invalid operation.");
                break;

        }
        SetText(topNumber, operation, bottomNumber);
    }

}
