using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_DelayedRound : MonoBehaviour, IState
{
    private int bulletCount = 20;    // ÃÑ¾ËÀÇ °¹¼ö
    private float bulletSpeed = 5f;  // ÃÑ¾Ë ½ºÇÇµå
    private float bulletTwist = 0f;  // È¸Àü°ª
    private float delay = 0.1f;
    private string bulletType = "";
    private Transform shootPos = null;

    private Coroutine behave = null;

    private readonly string sound_Shoot = "Effect_Sound_Shoot";

    public void OnEnter()
    {
        behave = StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        float angle = 360f / bulletCount;                                   // ¿ø µÑ·¹ / ÃÑ¾ËÀÇ °¹¼ö
        Vector3 dir = Vector3.up;                                           // ÃÑ¾Ë ±âº» ¹æÇâ == Vector3.up


        int bc = bulletCount;
        string type = bulletType;
        float bs = bulletSpeed;
        float bt = bulletTwist;

        WaitForSeconds shootWait = new WaitForSeconds(delay);

        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

        for (int i = 0; i < bc; i++)
        {
            Bullet bullet = PoolManager.Instance.GetQueue(PoolType.Bullet, type).GetComponent<Bullet>();
            bullet.transform.position = shootPos.position;

            dir.x = Mathf.Cos((angle * i + bt) * Mathf.Deg2Rad);
            dir.y = Mathf.Sin((angle * i + bt) * Mathf.Deg2Rad);

            bullet.ChangeDir(dir.normalized);
            bullet.ChangeSpeed(bs);

            yield return shootWait;
        }

        yield return null;
    }

    public void OnEnd()
    {
        StopCoroutine(behave);
    }

    public void SetValue(int bulletCount, float bulletSpeed, string bulletType, Transform shootPos, float delay)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.bulletTwist = 0f;
        this.delay = delay;
    }

    public void SetValue(int bulletCount, float bulletSpeed, float bulletTwist, string bulletType, Transform shootPos, float delay)
    {
        this.bulletCount = bulletCount;
        this.bulletSpeed = bulletSpeed;
        this.shootPos = shootPos;
        this.bulletType = bulletType;
        this.bulletTwist = bulletTwist;
        this.delay = delay;
    }
}
