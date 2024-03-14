using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ColliderDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != null)
        {
            Debug.Log("GameObject entered: " + other.gameObject.name);

            GameObject otherGameObject = other.gameObject;

            // Controllo se l'altro oggetto ha uno dei componenti necessari
            RandomOperations randomOperations = otherGameObject.GetComponent<RandomOperations>();
            BinaryOperations binaryOperations = otherGameObject.GetComponent<BinaryOperations>();

            if (randomOperations != null || binaryOperations != null)
            {
                // Se uno dei due componenti è presente, ottieni il risultato da rimuovere
                string resultToRemove = randomOperations != null ? randomOperations.Result : binaryOperations.Result;
                Spawner.Instance.RemoveOneMatchingResult(resultToRemove);
                ScoreSystem.Instance.RemoveHealth(1);
            }
            else
            {
                Debug.LogWarning("L'altro oggetto non ha né il componente RandomOperations né il componente BinaryOperations.");
            }

            // Muovi l'oggetto al manager dei pool solo se ha uno dei componenti necessari
            if (randomOperations != null || binaryOperations != null)
            {
                PoolManager.Instance.MoveToReserve(otherGameObject);
            }
        }
    }

}
