using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die_RemoveAndGiveBomb : MonoBehaviour, IState
{
    private readonly string exp_Light = "Effect_Exp_Light";
    private readonly string sound_Exp = "Effect_Sound_BigExplode";

    private readonly string upgrade = typeof(Point_Upgrade).ToString();
    private readonly string score = typeof(Point_Score).ToString();
    private readonly string bomb = typeof(Point_Bomb).ToString();

    public void OnEnter()
    {
        GameManager.Instance.uiHandler.AddScore(10000);

        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Exp);

        GameObject ex = PoolManager.Instance.GetQueue(PoolType.Effect, exp_Light);
        ex.transform.position = transform.position;

        PoolManager.Instance.OnUseSpell.Invoke();

        float rand = Random.value * 10;

        PoolManager.Instance.GetQueue(PoolType.Point, bomb).transform.position = transform.position;

        for (int i = 0; i < 5; i++)
        {
            PoolManager.Instance.GetQueue(PoolType.Point, score).transform.position = GetRandomPoint();
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
