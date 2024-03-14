using System;
using System.Collections.Generic;
using System.Text;
using com.cyborgAssets.inspectorButtonPro;
using TMPro;
using UnityEngine;

public class BinaryOperations : MonoBehaviour
{
    private string resultBinary = "";
    public string Result
    {
        get { return resultBinary; }
        set { resultBinary = value; }
    }

    void Start()
    {
        GenerateRandomBinaryOperation();
        Spawner.Instance.results.Add(resultBinary);
        Spawner.Instance.BinarylOperationSpawned = true;
    }

    public void GenerateRandomBinaryOperation()
    {
        string operation = GetRandomBinaryOperator();
        string a = GenerateRandomBinaryNumber();//GenerateRandomBinaryNumber(); // Genera un numero binario a 2 bit
        string b = GenerateRandomBinaryNumber(); // Genera un numero binario a 2 bit

        

        switch (operation)
        {
            case "&":
                resultBinary = PerformAndOperator(a, b);
                this.GetComponentInChildren<TextMeshPro>().text = $"{a}\n{operation}\n{b}";
                break;
            case "|":
                resultBinary = PerformOrOperator(a, b);
                this.GetComponentInChildren<TextMeshPro>().text = $"{a}\n{operation}\n{b}";
                break;
            case "^":
                resultBinary = PerformXorOperator(a, b);
                this.GetComponentInChildren<TextMeshPro>().text = $"{a}\n{operation}\n{b}";
                break;
            case "~":
                resultBinary = PerformNotOperator(a);
                this.GetComponentInChildren<TextMeshPro>().text = $"{operation}{a}\n";
                break;
            case "<<":
                int leftShiftAmount = 1;
                
                resultBinary = PerformLeftShiftOperator(a, leftShiftAmount);
                this.GetComponentInChildren<TextMeshPro>().text = $"{a}\n{operation}\n{1}";
                break;
            case ">>":
                int rightShiftAmount = 1;
                
                resultBinary = PerformRightShiftOperator(a, rightShiftAmount);
                this.GetComponentInChildren<TextMeshPro>().text = $"{a}\n{operation}\n{1}";
                break;
            default:
                Debug.LogWarning("Invalid operation.");
                break;
        }
        
       
    }

    private string GetRandomBinaryOperator()
    {
        // Array contenente gli operatori binari disponibili
        string[] operators = { "&", "|", "^", "~", "<<", ">>" };

        // Restituisce casualmente un'operatore binario dall'array
        return operators[UnityEngine.Random.Range(0, operators.Length)];
    }

    private string GenerateRandomBinaryNumber()
    {
        // Genera un numero binario casuale a 2 bit
        string binaryNumber = UnityEngine.Random.Range(0, 2).ToString();
        binaryNumber += UnityEngine.Random.Range(0, 2).ToString();
        return binaryNumber;
    }

    
    private string FromDecimalTo(int value, int destinationBase)
    {
        var result = Convert.ToString(value, destinationBase);

        result = result.PadLeft(2, '0');
        return result;
    }


    
    private int FromBaseToDecimal(string value, int sourceBase)
    {
        var result = Convert.ToInt32(value, sourceBase);

        
        return result;
    }


    
    private string PerformAndOperator(string a, string b)
    {
        var result = FromBaseToDecimal(a, 2) & FromBaseToDecimal(b, 2);
        var resultAsBinary = FromDecimalTo(result, 2);
        
        return resultAsBinary;
    }


    
    private string PerformOrOperator(string a, string b)
    {
        var result = FromBaseToDecimal(a, 2) | FromBaseToDecimal(b, 2);
        var resultAsBinary = FromDecimalTo(result, 2);
        
        return resultAsBinary;
    }


    
    private string PerformXorOperator(string a, string b)
    {
        var result = FromBaseToDecimal(a, 2) ^ FromBaseToDecimal(b, 2);
        var resultAsBinary = FromDecimalTo(result, 2);
        
        return resultAsBinary;
    }



    private string PerformNotOperator(string a)
    {
        var intValue = FromBaseToDecimal(a, 2);
        var result = ~intValue;

       
        var mask = 3;
        result &= mask;

        
        var resultAsBinary = FromDecimalTo(result, 2);

        return resultAsBinary;
    }



    private string PerformLeftShiftOperator(string a, int shift)
    {
        var result = FromBaseToDecimal(a, 2) << shift;

        var mask = 3;
        result &= mask;

        var resultAsBinary = FromDecimalTo(result, 2);

        

        return resultAsBinary;
    }


    
    private string PerformRightShiftOperator(string a, int shift)
    {
        var result = FromBaseToDecimal(a, 2) >> shift;

        var mask = 3;
        result &= mask;

        var resultAsBinary = FromDecimalTo(result, 2);
            

        return resultAsBinary;
    }

    
}
