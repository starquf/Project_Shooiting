using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapon_ChargeKnife : Weapon
{
    [SerializeField]
    private float chargeTime = 3f;

    [SerializeField]
    private Color unChargedColor;
    [SerializeField]
    private Color chargedColor;

    private string bullet = typeof(Bullet_PlayerChargeKnife).ToString();

    private Tweener colorTween = null;

    private readonly string sound_Shoot = "Effect_Sound_Knife";

    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponType.ChargeKnife;
    }

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    public override void PlayShoot()
    {
        canShoot = true;
    }

    public override void SetEnable(bool isEnable, int upgradeValue)
    {
        if (showUpgradeValue > upgradeValue) return;
        weapon_Enable = isEnable;
        weaponImage.enabled = isEnable;
    }

    public override void StopShoot()
    {
        canShoot = false;
    }

    protected override IEnumerator Shoot()
    {
        float currentTime = 0f;
        bool charged = false;
        bool isChargeStart = false;

        while (true)
        {
            if (canShoot && weapon_Enable && !charged)
            {
                if (!isChargeStart)
                {
                    isChargeStart = true;
                    colorTween = weaponImage.DOColor(chargedColor, chargeTime).SetEase(Ease.Linear);
                }

                if (currentTime >= chargeTime)
                {
                    currentTime = 0f;
                    charged = true;
                }
                else
                {
                    currentTime += Time.deltaTime;
                }
            }
            else
            {
                isChargeStart = false;
                currentTime = 0f;

                if (!charged)
                {
                    colorTween.Kill();
                    weaponImage.color = unChargedColor;
                }
            }

            if (!canShoot && charged)
            {
                PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

                GameObject obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bullet);
                obj.transform.position = transform.position;
                obj.GetComponent<Bullet>().ChangeDir(transform.up);

                charged = false;

                colorTween.Kill();
                weaponImage.color = unChargedColor;
            }
            

            yield return null;
        }
    }
}
