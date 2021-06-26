using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_SpreadBullet : MonoBehaviour, IState
{
    private int bulletCount = 10;
    private float bulletSpeed = 5f;  // 총알 스피드

    private float arc = 60f;
    private float waitTime = 0.05f;
    private float rotate = 180f;
     
    private string bulletType = "";
    
    private bool reverse = false;

    private Transform shootPos = null;

    private readonly string sound_Shoot = "Effect_Sound_Shoot";

    public void OnEnter()
    {
        StartCoroutine(ShootBullet());
    }

    private IEnumerator ShootBullet()
    {
        Vector3 pos = shootPos.position;

        float a = arc;
        float r = rotate;
        float angle = arc / bulletCount;
        int count = bulletCount;
        float wt = waitTime;
        float bs = bulletSpeed;

        Vector3 dir = Vector3.zero;

        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

        if (!reverse)
        {
            for (int i = 0; i < count; i++)
            {
                Bullet bullet = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletType).GetComponent<Bullet>();
                bullet.transform.position = pos;

                dir.x = Mathf.Cos((angle * i + r + angle / 2 - a / 2) * Mathf.Deg2Rad);
                dir.y = Mathf.Sin((angle * i + r + angle / 2 - a / 2) * Mathf.Deg2Rad);

                bullet.ChangeSpeed(bs);
                bullet.ChangeDir(dir);

                yield return new WaitForSeconds(wt);
            }
        }

        else
        {
            for (int i = count; i > 0; i--)
            {
                Bullet bullet = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletType).GetComponent<Bullet>();
                bullet.transform.position = transform.position;

                dir.x = Mathf.Cos((angle * i + r + angle / 2 - a / 2) * Mathf.Deg2Rad);
                dir.y = Mathf.Sin((angle * i + r + angle / 2 - a / 2) * Mathf.Deg2Rad);

                bullet.ChangeSpeed(bs);
                bullet.ChangeDir(dir);

                yield return new WaitForSeconds(wt);
            }
        }

        yield return null;
    }

    public void OnEnd()
    {

    }

    public void SetValue(int bulletCount, float bulletSpeed, float arc, float waitTime, float rotate, string bulletType, Transform shootPos, bool reverse = false)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.arc = arc;
        this.waitTime = waitTime;
        this.rotate = rotate;
        this.reverse = reverse;
    }
}
