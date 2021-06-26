using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    ChargeKnife
}
public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected float shootTime = 1f;
    protected WaitForSeconds shootWait = null;

    [HideInInspector]
    public WeaponType weaponType;

    public bool canShoot = false;
    public bool is_SlowAttack = false;

    [Space(10f)]
    public bool weapon_Enable = false;

    [Space(10f)]
    [SerializeField]
    protected SpriteRenderer weaponImage = null;
    [SerializeField]
    protected int showUpgradeValue;

    protected virtual void Awake()
    {
        shootWait = new WaitForSeconds(shootTime);
        canShoot = false;
        weapon_Enable = false;
        weaponImage.enabled = false;
        is_SlowAttack = false;
    }

    public abstract void PlayShoot();
    public abstract void StopShoot();
    public abstract void SetEnable(bool isEnable, int upgradeValue);

    protected abstract IEnumerator Shoot();
}
