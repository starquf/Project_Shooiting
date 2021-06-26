using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_RoundHoming : MonoBehaviour, IState
{
    private int bulletCount = 20;    // ÃÑ¾ËÀÇ °¹¼ö
    private float bulletSpeed = 5f;  // ÃÑ¾Ë ½ºÇÇµå
    private float bulletTwist = 0f;  // È¸Àü°ª
    private float radius = 1f;
    private string bulletType = "";
    private Transform shootPos = null;

    private readonly string sound_Shoot = "Effect_Sound_Shoot";

    public void OnEnter()
    {
        float angle = 360f / bulletCount;                                   // ¿ø µÑ·¹ / ÃÑ¾ËÀÇ °¹¼ö
        Vector3 dir = Vector3.up;                                           // ÃÑ¾Ë ±âº» ¹æÇâ == Vector3.up

        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

        for (int i = 0; i < bulletCount; i++)
        {
            Bullet bullet = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletType).GetComponent<Bullet>();
            bullet.transform.position = shootPos.position;

            dir.x = Mathf.Cos((angle * i + bulletTwist) * Mathf.Deg2Rad);
            dir.y = Mathf.Sin((angle * i + bulletTwist) * Mathf.Deg2Rad);

            bullet.ChangeDir(dir.normalized);
            bullet.ChangeSpeed(3f);

            bullet.ChangeDirToPlayer(radius);
            bullet.ChangeSpeed(bulletSpeed, radius);
        }
    }

    public void OnEnd()
    {

    }

    public void SetValue(int bulletCount, float bulletSpeed, float radius, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.bulletTwist = 0f;
        this.radius = radius;
    }

    public void SetValue(int bulletCount, float bulletSpeed, float bulletTwist, float radius, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.bulletTwist = bulletTwist;
        this.radius = radius;
    }
}
