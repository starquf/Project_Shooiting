using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_SpreadBulletToPlayer : MonoBehaviour, IState
{
    private int bulletCount = 10;
    private float bulletSpeed = 5f;  // 총알 스피드
    private float arc = 60f;
    private string bulletType = "";

    public Transform shootPos = null;

    private readonly string sound_Shoot = "Effect_Sound_Shoot";

    public void OnEnter()
    {
        Vector3 dir = Vector3.zero;
        dir = GameManager.Instance.playerPos.position - shootPos.position;

        float rotate = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float angle = arc / bulletCount;
        int count = bulletCount;

        dir = Vector3.zero;

        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

        for (int i = 0; i < count; i++)
        {
            Bullet bullet = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletType).GetComponent<Bullet>();
            bullet.transform.position = shootPos.position;

            dir.x = Mathf.Cos((angle * i + rotate + angle / 2 - arc / 2) * Mathf.Deg2Rad);
            dir.y = Mathf.Sin((angle * i + rotate + angle / 2 - arc / 2) * Mathf.Deg2Rad);

            bullet.ChangeSpeed(bulletSpeed);
            bullet.ChangeDir(dir);
        }
    }

    public void OnEnd()
    {

    }

    public void SetValue(int bulletCount, float bulletSpeed, float arc, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.arc = arc;
    }
}
