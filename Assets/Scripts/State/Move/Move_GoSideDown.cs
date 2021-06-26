using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_GoSideDown : MonoBehaviour, IState
{
    public float speed = 5f;

    private Vector3 startPos = Vector3.zero;
    private Coroutine behave = null;

    public void OnEnter()
    {
        startPos = transform.position;

        behave = StartCoroutine(MoveSideDown());
    }

    private IEnumerator MoveSideDown()
    {
        float moveY = Random.Range(-0.6f, -0.4f);

        while (true)
        {
            if (startPos.x <= GameManager.Instance.mapCenter.x)
            {
                transform.position += new Vector3(1f, moveY).normalized * speed * Time.deltaTime;
            }
            else
            {
                transform.position += new Vector3(-1f, moveY).normalized * speed * Time.deltaTime;
            }

            yield return null;
        }
    }

    public void OnEnd()
    {
        StopCoroutine(behave);
    }
}
