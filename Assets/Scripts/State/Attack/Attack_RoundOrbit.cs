using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_RoundOrbit : MonoBehaviour, IState
{
    private int bulletCount = 20;               // �Ѿ��� ����
    private float bulletSpeed = 5f;             // �Ѿ� ���ǵ�

    private float radiusSpeed = 0f;             // �þ�� ���ǵ�
    private float rotateSpeed = 30f;            // ���� �ӵ�
    private Vector2 shootDir = Vector2.zero;

    private string bulletType = "";

    private Transform shootPos = null;

    private readonly string sound_Shoot = "Effect_Sound_Shoot";

    public void OnEnter()
    {
        float angle = 360f / bulletCount;                                   // �� �ѷ� / �Ѿ��� ����
        Vector3 dir = Vector3.up;                                           // �Ѿ� �⺻ ���� == Vector3.up

        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

        for (int i = 1; i <= bulletCount; i++)
        {
            Bullet bullet = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletType).GetComponent<Bullet>();

            bullet.transform.position = shootPos.position;

            dir.x = Mathf.Cos((angle * i) * Mathf.Deg2Rad);
            dir.y = Mathf.Sin((angle * i) * Mathf.Deg2Rad);

            bullet.ChangeDir(dir.normalized);
            bullet.ChangeSpeed(radiusSpeed);
            bullet.ChangeDir(shootDir, 1f);
            bullet.ChangeSpeed(3f, 1f);

            bullet.RotateAround(shootPos, rotateSpeed, 1.5f);
            bullet.ChangeSpeed(bulletSpeed, 1.5f);
        }
    }

    public void OnEnd()
    {

    }

    public void SetValue(int bulletCount, float bulletSpeed, float radiusSpeed, float rotateSpeed, Vector2 shootDir, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.radiusSpeed = radiusSpeed;
        this.shootDir = shootDir;
        this.rotateSpeed = rotateSpeed;
    }
}
