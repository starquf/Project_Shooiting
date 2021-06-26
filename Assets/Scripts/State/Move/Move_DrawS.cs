using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_DrawS : MonoBehaviour, IState
{
    private Coroutine behave = null;

    public float speed = 3f;
    public float height = 6f;
    public float move_y = 1f;

    public Vector3 createPos = Vector3.zero;

    public void OnEnter()
    {
        createPos = transform.position;
        behave = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float gravity = height;
        Vector3 dir = Vector3.zero;

        while (true)
        {
            dir.x = gravity >= 0f ? -gravity : gravity;
            dir.y = createPos.y > 2f ? -move_y : move_y;

            gravity -= speed * Time.deltaTime;

            transform.position += dir.normalized * speed * Time.deltaTime;

            yield return null;
        }
    }

    public void OnEnd()
    {
        StopCoroutine(behave);
    }
}
