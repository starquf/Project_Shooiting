using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Move_Back : MonoBehaviour, IState
{
    private Coroutine behave = null;
    public float speed = 5f;

    private float currentSpeed = 0f;
    private float ac_y = 0f;

    public Vector3 createPos = Vector3.zero;

    public void OnEnter()
    {
        currentSpeed = speed;
        ac_y = 0f;
        createPos = transform.position;
        behave = StartCoroutine(Behave());
    }

    public void OnEnd()
    {
        StopCoroutine(behave);
    }

    private IEnumerator Behave()
    {
        while (true)
        {
            if (createPos.x <= GameManager.Instance.mapCenter.x)
            {
                transform.position += new Vector3(-2f, (-3f + ac_y) * currentSpeed) * Time.deltaTime;
                currentSpeed += 0.01f;
                ac_y += 0.03f;
            }
            else
            {
                transform.position += new Vector3(2f, (-3f + ac_y) * currentSpeed) * Time.deltaTime;
                currentSpeed += 0.01f;
                ac_y += 0.03f;
            }

            yield return null;
        }
    }
}
