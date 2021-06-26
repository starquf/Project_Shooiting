using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_RoundGoAndStop : MonoBehaviour, IState
{
    private int bulletCount = 20;    // ÃÑ¾ËÀÇ °¹¼ö
    private float bulletSpeed = 5f;  // ÃÑ¾Ë ½ºÇÇµå
    private float bulletTwist = 0f;  // È¸Àü°ª
    private string bulletType = "";
    private float stopTime = 1f;
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
            bullet.ChangeSpeed(bulletSpeed);
            bullet.ChangeSpeed(1.5f, stopTime);
        }
    }

    public void OnEnd()
    {

    }

    public void SetValue(int bulletCount, float bulletSpeed, float stopTime, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.bulletTwist = 0f;
        this.stopTime = stopTime;
    }

    public void SetValue(int bulletCount, float bulletSpeed, float bulletTwist, float stopTime, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.bulletTwist = bulletTwist;
        this.stopTime = stopTime;
    }
}
