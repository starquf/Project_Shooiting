using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_GoLeftOrRight : MonoBehaviour, IState
{
    public float moveSpeed = 3f;

    private Vector2 createPos = Vector2.zero;
    private Coroutine behave = null;


    public void OnEnter()
    {
        createPos = transform.position;
        behave = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            if (createPos.x <= GameManager.Instance.mapCenter.x)
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }

    public void OnEnd()
    {
        StopCoroutine(behave);
    }
}
