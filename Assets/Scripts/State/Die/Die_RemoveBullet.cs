using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die_RemoveBullet : MonoBehaviour, IState
{
    private readonly string exp_Light = "Effect_Exp_Light";
    private readonly string sound_Exp = "Effect_Sound_Explode";

    private readonly string upgrade = typeof(Point_Upgrade).ToString();
    private readonly string score = typeof(Point_Score).ToString();

    public void OnEnter()
    {
        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Exp);

        GameObject ex = PoolManager.Instance.GetQueue(PoolType.Effect, exp_Light);
        ex.transform.position = transform.position;

        PoolManager.Instance.OnUseSpell.Invoke();

        float rand = Random.value * 10;

        if (rand > 7f) // 30%
        {
            PoolManager.Instance.GetQueue(PoolType.Point, upgrade).transform.position = transform.position;
        }
        else
        {
            PoolManager.Instance.GetQueue(PoolType.Point, score).transform.position = transform.position;
        }
    }

    public void OnEnd()
    {

    }
}
