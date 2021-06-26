using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player_Tech_Spell : MonoBehaviour, ISpell
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
    private readonly string bombBullet = typeof(Bullet_PlayerBomb).ToString();
    private readonly string sound_bomb = "Effect_Sound_Bomb";
    private readonly string sound_shoot = "Effect_Sound_Shoot";

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

        PoolManager.Instance.GetQueue(PoolType.Effect, sound_bomb);

        Camera.main.DOShakePosition(1f, 1f, 10, 40f, true);

        playerDamaged.StartInvincible(12);

        PoolManager.Instance.OnUseSpell.Invoke();

        // 스킬 이펙트
        for (int i = 0; i < 5; i++)
        {

            ShootBomb(new Vector2(-0.05f, 1f));
            ShootBomb(new Vector2(0.05f, 1f));

            yield return pOneSecWait;

            ShootBomb(Vector2.up);

            yield return pOneSecWait;

            ShootBomb(new Vector2(0.05f, 1f));
            ShootBomb(new Vector2(-0.05f, 1f));

            yield return pOneSecWait;
        }

        for (int i = 0; i < 2; i++)
        {
            ShootBomb(new Vector2(-0.5f, 1f));
            ShootBomb(new Vector2(0.5f, 1f));

            yield return pOneSecWait;

            ShootBomb(new Vector2(-0.25f, 1f));
            ShootBomb(new Vector2(0.25f, 1f));

            yield return pOneSecWait;

            ShootBomb(Vector2.up);

            yield return pOneSecWait;

            ShootBomb(new Vector2(0.25f, 1f));
            ShootBomb(new Vector2(-0.25f, 1f));

            yield return pOneSecWait;

            ShootBomb(new Vector2(0.5f, 1f));
            ShootBomb(new Vector2(-0.5f, 1f));

            yield return pOneSecWait;
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

    private void ShootBomb(Vector2 dir)
    {
        PoolManager.Instance.GetQueue(PoolType.Effect, sound_shoot);
        obj = PoolManager.Instance.GetQueue(PoolType.Bullet, bombBullet).GetComponent<Bullet>();
        obj.ChangeSpeed(30f);
        obj.ChangeDir(dir);
        obj.transform.position = transform.position;
    }
}
