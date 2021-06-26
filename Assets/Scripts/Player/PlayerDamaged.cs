using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDamaged : MonoBehaviour
{
    private IAttack playerAttack = null;
    private PlayerMove playerMove = null;

    [SerializeField]
    private PlayerPoint playerPoint = null;

    private ISpell playerSpell = null;
    private PlayerPointMagnet playerMagent = null;

    public bool can_damaged = true;

    private Collider2D c2;
    private SpriteRenderer playerSpr = null;
    private SpriteRenderer heartSpr = null;

    public Transform respawnPos = null;

    public float respawnTime = 1f;
    private WaitForSeconds respawnWait;

    private WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);

    private Coroutine blinkingRoutine = null;

    private readonly string effect = "Effect_Exp_Light";
    private readonly string sound_Exp = "Effect_Sound_BigExplode";

    [Space(10f)]
    public int maxHp;
    [SerializeField]
    private int currHp;

    private void Awake()
    {
        Transform myParent = transform.parent;

        playerAttack = myParent.GetComponent<IAttack>();
        playerMove = myParent.GetComponent<PlayerMove>();
        playerSpell = myParent.GetComponent<ISpell>();
        playerMagent = myParent.GetComponent<PlayerPointMagnet>();

        c2 = GetComponent<Collider2D>();
        playerSpr = myParent.GetComponent<SpriteRenderer>();
        heartSpr = GetComponent<SpriteRenderer>();

        if (playerAttack == null || playerMove == null)
        {
            Debug.LogError("Missing Script : PlayerAttack or PlayerMove");
        }
    }

    private void Start()
    {
        currHp = maxHp;
        respawnWait = new WaitForSeconds(respawnTime);

        GameManager.Instance.OnStageEnd.AddListener(() => {
            transform.parent.position = respawnPos.position;
        });

        GameManager.Instance.OnGameRestart.AddListener(() =>
        {
            c2.enabled = false;
        });
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!can_damaged) return;

        IDamage damageObj = coll.GetComponent<IDamage>();

        if (damageObj != null)
        {
            can_damaged = false;

            PoolManager.Instance.GetQueue(PoolType.Effect, effect).transform.position = transform.position;
            PoolManager.Instance.GetQueue(PoolType.Effect, sound_Exp);

            StartCoroutine(OnDie());
        }
    }

    private IEnumerator OnDie()
    {
        Camera.main.DOShakePosition(0.5f, 0.3f, 10, 90, true);

        c2.enabled = false;

        ChangeEnabled(false);

        playerAttack.SetWeaponEnable(false);

        /*
        for (int i = 0; i < playerAttack.currentWeapon.Count; i++)
        {
            playerAttack.currentWeapon[i].SetEnable(false, playerAttack.upgradeValue);
        }
        */

        yield return respawnWait;


        if (currHp <= 0)
        {
            // 게임오버
            playerPoint.can_Earn = false;
            GameManager.Instance.OnGameOver.Invoke();
        }
        else
        {
            currHp--;
            GameManager.Instance.uiHandler.SetPlayerHp(currHp);
            playerSpell.SetBomb(3);

            ReSpawn();
        }
    }

    public void AddHealth()
    {
        if (currHp >= 8) return;

        currHp++;
        GameManager.Instance.uiHandler.SetPlayerHp(currHp);
    }

    public void StartInvincible(int repeat)
    {
        c2.enabled = false;

        if (blinkingRoutine != null)
        {
            StopCoroutine(blinkingRoutine);
        }

        blinkingRoutine = StartCoroutine(Blinking(repeat));
    }

    private IEnumerator Blinking(int repeat)
    {
        for (int i = 0; i < repeat; i++)
        {
            playerSpr.color = new Color(1f, 1f, 1f, 0.3f);
            yield return pOneSecWait;
            playerSpr.color = Color.white;
            yield return pOneSecWait;
        }

        can_damaged = true;
        c2.enabled = true;
    }

    private void ReSpawn()
    {
        transform.parent.position = respawnPos.position;

        ChangeEnabled(true);

        playerAttack.SetWeaponEnable(true);

        /*
        for (int i = 0; i < playerAttack.currentWeapon.Count; i++)
        {
            playerAttack.currentWeapon[i].SetEnable(true, playerAttack.upgradeValue);
        }
        */

        if (blinkingRoutine != null)
        {
            StopCoroutine(blinkingRoutine);
        }

        blinkingRoutine = StartCoroutine(Blinking(12));
    }

    private void ChangeEnabled(bool state)
    {
        playerSpr.enabled = state;
        heartSpr.enabled = state;

        playerMove.can_Move = state;
        playerAttack.can_shoot = state;
        playerSpell.can_Spell = state;
        playerMagent.can_Magnet = state;
    }
}
