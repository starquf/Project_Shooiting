using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_RoundCharge : MonoBehaviour, IState
{
    private int bulletCount = 20;    // �Ѿ��� ����
    private float bulletSpeed = 5f;  // �Ѿ� ���ǵ�
    private float radiusSpeed = 0f;  // �þ�� ���ǵ�
    private string bulletType = "";
    private Vector2 shootDir = Vector2.zero;
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

            dir.x = Mathf.Cos((angle * i) * Mathf.Deg2Rad);
            dir.y = Mathf.Sin((angle * i) * Mathf.Deg2Rad);

            bullet.ChangeDir(dir.normalized);
            bullet.ChangeSpeed(radiusSpeed);
            bullet.ChangeDir(shootDir, 1f);
            bullet.ChangeSpeed(bulletSpeed, 1f);
        }
    }

    public void OnEnd()
    {

    }

    public void SetValue(int bulletCount, float bulletSpeed, Vector2 shootDir, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.radiusSpeed = 3f;
        this.shootDir = shootDir;
    }

    public void SetValue(int bulletCount, float bulletSpeed, float radiusSpeed, Vector2 shootDir, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.radiusSpeed = radiusSpeed;
        this.shootDir = shootDir;
    }
}
