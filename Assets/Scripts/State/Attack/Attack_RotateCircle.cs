using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_RotateCircle : MonoBehaviour, IState
{
    private int bulletCount = 20;    // 총알의 갯수
    private float bulletSpeed = 5f;  // 총알 스피드
    private float rotateSpeed = 30f;
    private string bulletType = "";

    private Transform shootPos = null;

    private readonly string sound_Shoot = "Effect_Sound_Shoot";

    public void OnEnter()
    {
        float angle = 360f / bulletCount;                                   // 원 둘레 / 총알의 갯수
        Vector3 dir = Vector3.up;                                           // 총알 기본 방향 == Vector3.up
        Vector3 tar = Vector3.zero;

        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

        for (int i = 1; i <= bulletCount; i++)
        {
            Bullet bullet = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletType).GetComponent<Bullet>();

            bullet.transform.position = shootPos.position;

            dir.x = Mathf.Cos((angle * i) * Mathf.Deg2Rad);
            dir.y = Mathf.Sin((angle * i) * Mathf.Deg2Rad);

            tar.x = Mathf.Cos((angle * (i + 1)) * Mathf.Deg2Rad);
            tar.y = Mathf.Sin((angle * (i + 1)) * Mathf.Deg2Rad);

            tar -= dir;

            bullet.ChangeDir(dir);
            bullet.ChangeSpeed(0.5f);
            bullet.ChangeSpeed(bulletSpeed, 0.5f);
            bullet.RotateAround(shootPos, rotateSpeed, 0.1f);
            bullet.ChangeDir(tar, 0.11f);
        }
    }

    public void OnEnd()
    {

    }

    public void SetValue(int bulletCount, float bulletSpeed, float rotateSpeed, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.rotateSpeed = rotateSpeed;
    }
}
