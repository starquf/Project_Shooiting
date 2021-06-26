using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die_GivePoint : MonoBehaviour, IState
{
    private readonly string exp_Light = "Effect_Exp_Light";
    private readonly string sound_Exp = "Effect_Sound_Explode";

    private readonly string upgrade = typeof(Point_Upgrade).ToString();
    private readonly string score = typeof(Point_Score).ToString();

    public void OnEnter()
    {
        GameManager.Instance.uiHandler.AddScore(10);

        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Exp);

        GameObject ex = PoolManager.Instance.GetQueue(PoolType.Effect, exp_Light);
        ex.transform.position = transform.position;

        float rand = Random.value * 10;

        PoolManager.Instance.GetQueue(PoolType.Point, score).transform.position = GetRandomPoint();

        if (rand > 7f) // 30%
        {
            PoolManager.Instance.GetQueue(PoolType.Point, upgrade).transform.position = GetRandomPoint();
        }
    }

    private Vector3 GetRandomPoint()
    {
        Vector3 myPoint = transform.position;
        Vector3 randomPoint = new Vector3(Random.Range(myPoint.x - 0.5f, myPoint.x + 0.5f), Random.Range(myPoint.y - 0.5f, myPoint.y + 0.5f));

        return randomPoint;
    }

    public void OnEnd()
    {

    }
}
