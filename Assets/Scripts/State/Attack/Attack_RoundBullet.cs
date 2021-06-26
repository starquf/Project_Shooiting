using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_RoundBullet : MonoBehaviour, IState
{
    private int bulletCount = 20;    // 총알의 갯수
    private float bulletSpeed = 5f;  // 총알 스피드
    private float bulletTwist = 0f;  // 회전값
    private string bulletType = "";
    private Transform shootPos = null;

    private readonly string sound_Shoot = "Effect_Sound_Shoot";

    public void OnEnter()
    {
        float angle = 360f / bulletCount;                                   // 원 둘레 / 총알의 갯수
        Vector3 dir = Vector3.up;                                           // 총알 기본 방향 == Vector3.up

        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

        for (int i = 0; i < bulletCount; i++)
        {
            Bullet bullet = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletType).GetComponent<Bullet>();
            bullet.transform.position = shootPos.position;

            dir.x = Mathf.Cos((angle * i + bulletTwist) * Mathf.Deg2Rad);
            dir.y = Mathf.Sin((angle * i + bulletTwist) * Mathf.Deg2Rad);

            bullet.ChangeDir(dir.normalized);
            bullet.ChangeSpeed(bulletSpeed);
        }
    }

    public void OnEnd()
    {

    }

    public void SetValue(int bulletCount, float bulletSpeed, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.bulletTwist = 0f;
    }

    public void SetValue(int bulletCount, float bulletSpeed, float bulletTwist, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.bulletTwist = bulletTwist;
    }
}
