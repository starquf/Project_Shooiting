using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_RoundGoBack : MonoBehaviour, IState
{
    private int bulletCount = 20;    // �Ѿ��� ����
    private float bulletSpeed = 5f;  // �Ѿ� ���ǵ�
    private float bulletTwist = 0f;  // ȸ����

    private float goBackTime = 0f;   //  ���ư��� �ð�
    private float rotateAngle = 60f; 

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
            bullet.ChangeSpeed(bulletSpeed);
            bullet.ChangeDir(-dir.normalized, goBackTime);
            bullet.RotateAngleInTime(rotateAngle, goBackTime * 2f, 3f);
        }
    }

    public void OnEnd()
    {

    }

    public void SetValue(int bulletCount, float bulletSpeed, float goBackTime, float rotateAngle, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.bulletTwist = 0f;
        this.goBackTime = goBackTime;
        this.rotateAngle = rotateAngle;
    }

    public void SetValue(int bulletCount, float bulletSpeed, float bulletTwist, float goBackTime, float rotateAngle, string bulletType, Transform shootPos)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.bulletTwist = bulletTwist;
        this.goBackTime = goBackTime;
        this.rotateAngle = rotateAngle;
    }
}
