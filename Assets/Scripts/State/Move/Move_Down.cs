using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Down : MonoBehaviour, IState
{
    public float speed = 5f;

    private Vector3 startPos = Vector3.zero;
    private Coroutine behave = null;

    public void OnEnter()
    {
        startPos = transform.position;

        behave = StartCoroutine(MoveDown());
    }

    private IEnumerator MoveDown()
    {
        while (true)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;

            yield return null;
        }
    }

    public void OnEnd()
    {
        StopCoroutine(behave);
    }
}
