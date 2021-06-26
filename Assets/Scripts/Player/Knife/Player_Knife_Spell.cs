using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player_Knife_Spell : MonoBehaviour, ISpell
{
    private PlayerInput playerInput = null;

    [SerializeField]
    private PlayerDamaged playerDamaged = null;

    public int spellCount = 3;

    //public float spellWaitTime = 3f;
    //private WaitForSeconds spellWait;

    public bool can_Spell { get; set; } = true;   // 스킬을 사용할 수 있는가

    private bool is_Spell = false;        // 스킬 사용중인가

    private Bullet obj;

    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly string bombBullet = typeof(Bullet_PlayerChargeKnife).ToString();
    private readonly string knifeBullet = typeof(Bullet_PlayerKnife).ToString();

    private readonly string sound_Shoot = "Effect_Sound_Knife";

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        //spellWait = new WaitForSeconds(spellWaitTime);
    }

    private void Update()
    {
        if (playerInput.Skill && can_Spell && !is_Spell)
        {
            UseSpell();
        }
    }

    private void UseSpell()
    {
        if (spellCount <= 0) return;

        is_Spell = true;
        StartCoroutine(Spell());
    }

    private IEnumerator Spell()
    {
        spellCount--;
        GameManager.Instance.uiHandler.SetPlayerBomb(spellCount);
        GameManager.Instance.uiHandler.FadeOut();

        Camera.main.DOShakePosition(1f, 1f, 10, 40f, true);

        playerDamaged.StartInvincible(9);

        PoolManager.Instance.OnUseSpell.Invoke();

        // 스킬 이펙트
        for (int i = 0; i < 5; i++)
        {
            ShootBomb();
            ShootBomb();
            ShootBomb();

            ShootKnife();

            PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

            yield return pOneSecWait;
            PoolManager.Instance.OnUseSpell.Invoke();

            ShootBomb();
            ShootBomb();

            ShootKnife();

            PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

            yield return pOneSecWait;
            PoolManager.Instance.OnUseSpell.Invoke();

            ShootBomb();
            ShootBomb();

            ShootKnife();

            PoolManager.Instance.GetQueue(PoolType.Effect, sound_Shoot);

            yield return pOneSecWait;
            PoolManager.Instance.OnUseSpell.Invoke();
        }

        GameManager.Instance.uiHandler.FadeIn();
        is_Spell = false;
    }

    public void AddBomb()
    {
        if (spellCount >= 8) return;

        spellCount++;
        GameManager.Instance.uiHandler.SetPlayerBomb(spellCount);
    }

    public void SetBomb(int value)
    {
        spellCount = value;
        GameManager.Instance.uiHandler.SetPlayerBomb(spellCount);
    }

    private void ShootBomb()
    {
        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bombBullet).GetComponent<Bullet>();
        obj.ChangeDir(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
        obj.ChangeSpeed(0f);
        obj.ChangeSpeed(Random.Range(20f, 30f), 1f);
        obj.transform.position = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f));
    }

    private void ShootKnife()
    {
        float angle = 360f / 8;                          
        Vector3 dir = Vector3.zero;

        for (int i = 0; i < 8; i++)
        {
            Bullet bullet = PoolManager.Instance.GetQueue(PoolType.Bullet, knifeBullet).GetComponent<Bullet>();
            bullet.transform.position = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f));

            dir.x = Mathf.Cos((angle * i + Random.Range(0f, 180f)) * Mathf.Deg2Rad);
            dir.y = Mathf.Sin((angle * i + Random.Range(0f, 180f)) * Mathf.Deg2Rad);

            bullet.ChangeDir(dir.normalized);

            bullet.ChangeSpeed(0f);
            bullet.ChangeSpeed(Random.Range(20f, 30f), 1f);
        }
    }
}
