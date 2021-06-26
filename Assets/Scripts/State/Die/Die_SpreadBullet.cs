using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die_SpreadBullet : MonoBehaviour, IState
{
    private readonly string exp_Light = "Effect_Exp_Light";
    private readonly string sound_Exp = "Effect_Sound_Explode";

    private readonly string upgrade = typeof(Point_Upgrade).ToString();
    private readonly string score = typeof(Point_Score).ToString();

    public int bulletCount;
    public float bulletSpeed;
    public string bulletType;

    private Vector2 shootDir;

    public void OnEnter()
    {
        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Exp);

        shootDir = GameManager.Instance.playerPos.position - transform.position;
        float targetAngle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        float angle = 360f / bulletCount;                                   // ¿ø µÑ·¹ / ÃÑ¾ËÀÇ °¹¼ö
        Vector3 dir = Vector3.up;                                           // ÃÑ¾Ë ±âº» ¹æÇâ == Vector3.up

        for (int i = 0; i < bulletCount; i++)
        {
            Bullet bullet = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletType).GetComponent<Bullet>();
            bullet.transform.position = transform.position;

            dir.x = Mathf.Cos((angle * i + targetAngle) * Mathf.Deg2Rad);
            dir.y = Mathf.Sin((angle * i + targetAngle) * Mathf.Deg2Rad);

            bullet.ChangeDir(dir.normalized);
            bullet.ChangeSpeed(bulletSpeed);
        }


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
