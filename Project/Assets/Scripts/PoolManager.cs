using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    //public GameObject collisionObject;

    [SerializeField]
    private Vector3 ReservePosition = new Vector3(1200, 225, 0);
    public List<GameObject> reserveObject;
    private Spawner spawner;

    private void Start()
    {
        spawner = Spawner.Instance;
    }
    public void MoveToReserve(GameObject _collisionObject)
    {
        _collisionObject.SetActive(false);
        _collisionObject.transform.position = ReservePosition;
        reserveObject.Add(_collisionObject);
        spawner.spawnList.Remove(_collisionObject);
        

    }

    public GameObject activatePoolItem()
    {
        if (reserveObject.Count > 5 && spawner.NormalOperationSpawned && spawner.GoldenlOperationSpawned && spawner.BinarylOperationSpawned)
        {
            GameObject objectToActivate = reserveObject[0];
            objectToActivate.SetActive(true);
            //objectToActivate.transform.position = 

            spawner.spawnList.Add(objectToActivate);
            reserveObject.Remove(objectToActivate);


            if (objectToActivate.GetComponent<RandomOperations>() != null)
            {
                RandomOperations randomOperations = objectToActivate.GetComponent<RandomOperations>();
                randomOperations.PerformOperation();
                spawner.results.Add(randomOperations.Result);

                return objectToActivate;
            }
            else if (objectToActivate.GetComponent<BinaryOperations>() != null)
            {
                BinaryOperations operation = objectToActivate.GetComponent<BinaryOperations>();
                operation.GenerateRandomBinaryOperation();
                spawner.results.Add(operation.Result);
                return objectToActivate;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}
