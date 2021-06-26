using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Pistol : Weapon
{
    public Transform shootPos = null;

    private readonly string bulletType = typeof(Bullet_PlayerPistol).ToString();
    private Vector3 startRotate = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponType.Pistol;
        startRotate = transform.eulerAngles;
    }

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    public override void PlayShoot()
    {
        canShoot = true;
    }

    protected override IEnumerator Shoot()
    {
        while (true)
        {
            if (canShoot && weapon_Enable)
            {
                if (is_SlowAttack)
                {
                    SlowAttack();
                }
                else
                {
                    transform.eulerAngles = startRotate;
                }

                GameObject bullet = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletType);
                bullet.transform.position = shootPos.position;
                bullet.GetComponent<Bullet>().ChangeDir(shootPos.up);

                yield return shootWait;
            }

            yield return null;
        }
    }

    public override void StopShoot()
    {
        canShoot = false;
    }

    public override void SetEnable(bool isEnable, int upgradeValue)
    {
        if (showUpgradeValue > upgradeValue) return;
        weapon_Enable = isEnable;
        weaponImage.enabled = isEnable;
    }

    private void SlowAttack()
    {
        Vector3 nearEnemyPos = Vector3.zero;
        float distance = 999f;
        float temp = 0f;

        if (PoolManager.Instance.enemies.Count.Equals(0))
        {
            transform.eulerAngles = startRotate;
            return;
        }

        for (int i = 0; i < PoolManager.Instance.enemies.Count; i++)
        {
            temp = (transform.position - PoolManager.Instance.enemies[i].transform.position).sqrMagnitude;

            if (distance > temp)
            {
                distance = temp;
                nearEnemyPos = PoolManager.Instance.enemies[i].transform.position - transform.position;
            }
        }

        float angle = Mathf.Atan2(nearEnemyPos.y, nearEnemyPos.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
