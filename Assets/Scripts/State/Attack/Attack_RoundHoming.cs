using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_RoundHoming : MonoBehaviour, IState
{
    private int bulletCount = 20;    // �Ѿ��� ����
    private float bulletSpeed = 5f;  // �Ѿ� ���ǵ�
    private float bulletTwist = 0f;  // ȸ����
    private float radius = 1f;
    private string bulletType = "";
    private Transform shootPos = null;

    private readonly string sound_Shoot = "Effect_Sound_Shoot";

    public void OnEnter()
    {
        float angle = 360f / bulletCount;                                   // �� �ѷ� / �Ѿ��� ����
        Vector3 dir = Vector3.up;                                           // �Ѿ� �⺻ ���� == Vector3.up

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
