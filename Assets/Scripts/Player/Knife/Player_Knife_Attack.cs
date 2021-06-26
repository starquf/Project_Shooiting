using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Knife_Attack : MonoBehaviour, IAttack
{
    private PlayerInput playerInput = null;

    public bool can_shoot { get; set; } = true;

    [Space(10f)]
    public int upgradeValue = 0;
    private int upgradeExp = 0;

    [Space(10f)]
    public Transform shootPos_0 = null;
    public Transform shootPos_1 = null;
    public Transform shootPos_2 = null;

    private readonly string bulletKnife = typeof(Bullet_PlayerKnife).ToString();

    public float shootDelay = 0.1f;
    private WaitForSeconds shootWait = null;

    [Space(10f)]
    public List<Weapon> weapons = new List<Weapon>();
    private WeaponType currentWeaponType = WeaponType.ChargeKnife;

    [HideInInspector]
    public List<Weapon> currentWeapon = new List<Weapon>();

    private readonly string sound_Shoot = "Effect_Sound_PlayerShoot";

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }

        shootWait = new WaitForSeconds(shootDelay);
    }

    private void Start()
    {
        SetCurrentWeapon();
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            if (can_shoot && playerInput.Attack)
            {
                ShootBullet();

                yield return shootWait;
            }

            if (can_shoot && playerInput.MoveSlow)
            {
                ShootWeapon();
            }
            else
            {
                StopShootWeapon();
            }

            yield return null;
        }
    }

    private void ShootBullet()
    {
        PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

        switch (upgradeValue)
        {
            case 0:
                PowerOne();
                break;

            case 1:
                PowerTwo();
                break;

            case 2:
                PowerThree();
                break;

            case 3:
                PowerFour();
                break;

            case 4:
                PowerFive();
                break;

            case 5:
                PowerSix();
                break;
        }
    }

    private void ShootWeapon()
    {
        for (int i = 0; i < currentWeapon.Count; i++)
        {
            currentWeapon[i].PlayShoot();
        }
    }

    private void StopShootWeapon()
    {
        for (int i = 0; i < currentWeapon.Count; i++)
        {
            currentWeapon[i].StopShoot();
        }
    }

    private void SetCurrentWeapon()
    {
        currentWeapon.Clear();

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].weaponType.Equals(currentWeaponType))
            {
                weapons[i].SetEnable(true, upgradeValue);
                currentWeapon.Add(weapons[i]);
            }
            else
            {
                weapons[i].SetEnable(false, upgradeValue);
            }
        }
    }

    public void SetWeaponEnable(bool enable)
    {
        for (int i = 0; i < currentWeapon.Count; i++)
        {
            currentWeapon[i].SetEnable(enable, upgradeValue);
        }
    }

    public void Upgrade(int value)
    {
        if (upgradeValue >= 5) return;

        int upgradeLine = 0;

        upgradeExp += value;

        switch (upgradeValue)
        {
            case 0:
                upgradeLine = 5;
                break;

            case 1:
                upgradeLine = 10;
                break;

            case 2:
                upgradeLine = 20;
                break;

            case 3:
                upgradeLine = 30;
                break;

            case 4:
                upgradeLine = 30;
                break;
        }

        if (upgradeExp >= upgradeLine)
        {
            if (upgradeValue.Equals(4))
            {
                GameManager.Instance.uiHandler.ShowMessage("Full Power!");
            }
            else
            {
                GameManager.Instance.uiHandler.ShowMessage("Power Up!");
            }

            upgradeExp = 0;
            upgradeValue++;
            SetCurrentWeapon();
        }
    }

    private void PowerOne()         // 0
    {
        GameObject obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);
    }

    private void PowerTwo()         // 1
    {
        GameObject obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(-0.1f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(0.1f, 1f));
    }

    private void PowerThree()       // 2
    {
        GameObject obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(-0.1f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(0.1f, 1f));
    }

    private void PowerFour()        // 3
    {
        GameObject obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(-0.3f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(-0.1f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(0.1f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(0.3f, 1f));
    }

    private void PowerFive()        // 4
    {
        GameObject obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(-0.5f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(-0.3f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(-0.1f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(0.1f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(0.3f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(0.5f, 1f));
    }

    private void PowerSix()        // 4
    {
        GameObject obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(-0.7f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(-0.5f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(-0.3f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(-0.1f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_1.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(Vector2.up);

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(0.1f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(0.3f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(0.5f, 1f));

        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bulletKnife);
        obj.transform.position = shootPos_2.position;
        obj.GetComponent<Bullet>().ChangeDir(new Vector2(0.7f, 1f));
    }
}
