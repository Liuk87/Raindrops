using UnityEngine;
using System.Collections;

public class ObjectMovement : MonoBehaviour
{
    public float speed = .5f;
    private Coroutine fallingCoroutine;
    

    void Start()
    {
        fallingCoroutine = StartCoroutine(Falling());
    }

    private void Update()
    {
        //Debug.Log(fallingCoroutine);
    }
    public IEnumerator Falling()
    {
        while (true)
        {
            Vector3 fallingMovement = -Vector3.right * Time.deltaTime * speed;

            transform.Translate(fallingMovement);

            yield return null;
        }
    }

    public void StopFalling()
    {
        StopAllCoroutines();
    }

    public void ResumeFalling()
    {
        
        StartCoroutine(Falling());
    }
}
