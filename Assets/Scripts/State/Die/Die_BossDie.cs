using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die_BossDie : MonoBehaviour, IState
{
    private readonly string exp_Light = "Effect_Exp_Light";
    private readonly string sound_Exp = "Effect_Sound_BigExplode";

    private readonly string upgrade = typeof(Point_Upgrade).ToString();
    private readonly string score = typeof(Point_Score).ToString();
    private readonly string health = typeof(Point_Health).ToString();

    private Point point;

    public void OnEnter()
    {
        GameManager.Instance.uiHandler.AddScore(20000);

        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Exp);

        GameObject ex = PoolManager.Instance.GetQueue(PoolType.Effect, exp_Light);
        ex.transform.position = transform.position;

        PoolManager.Instance.OnUseSpell.Invoke();

        point = PoolManager.Instance.GetQueue(PoolType.Point, health).GetComponent<Point>();
        point.transform.position = transform.position;
        point.ChangeDirToPlayer();
        point.SetSpeed(10f);

        for (int i = 0; i < 8; i++)
        {
            point = PoolManager.Instance.GetQueue(PoolType.Point, score).GetComponent<Point>();
            point.transform.position = GetRandomPoint();
            point.ChangeDirToPlayer();
            point.SetSpeed(10f);
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
