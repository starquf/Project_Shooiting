using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_BezierCurve : MonoBehaviour, IState
{
    public float moveDur = 3f;

    private Coroutine behave = null;

    //private bool 

    public void OnEnter()
    {
        behave = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector3 startPos = transform.position;
        Vector3 target = startPos.x <= GameManager.Instance.mapCenter.x ? new Vector3(GameManager.Instance.maxSize.x + 8f, 0f) : new Vector3(GameManager.Instance.minSize.x - 8f, 0f);
        Vector3 middlePoint = new Vector3((startPos.x + target.x) / 2f, startPos.y - 3f);

        float increase = 0f;

        while (true)
        {
            transform.position = BezierCurve(increase, startPos, middlePoint, target);
            increase += Time.deltaTime / moveDur;

            yield return null;
        }
    }

    Vector3 BezierCurve(float t, Vector2 P0, Vector2 P1, Vector2 P2)
    {
        float x = (Mathf.Pow((1 - t), 2) * P0.x) + (2 * (1 - t) * t * P1.x) + (Mathf.Pow(t, 2) * P2.x);
        float y = (Mathf.Pow((1 - t), 2) * P0.y) + (2 * (1 - t) * t * P1.y) + (Mathf.Pow(t, 2) * P2.y);
        return new Vector2(x, y);
    }

    public void OnEnd()
    {
        StopCoroutine(behave);
    }
}
