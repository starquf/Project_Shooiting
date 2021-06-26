using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy_Tank : Enemy
{
    [SerializeField]
    private List<PhaseInfo> phaseInfos = new List<PhaseInfo>();
    private int currentPhase = 0;
    private bool isPhaseEnd = false;
    private bool isInvincible = false;

    private Coroutine phaseRoutine = null;
    private Coroutine phaseSubAttackRoutine = null;

    private Coroutine waitTimeRoutine = null;

    private Create_MoveCenter create = null;

    private Move_Back move_Back = null;
    private Move_SetPosition move_Position = null;

    private Attack_RoundBullet attack_Round = null;
    private Attack_RoundGoBack attack_RoundGoBack = null;
    private Attack_RoundCharge attack_RoundCharge = null;

    private Attack_SpreadBulletToPlayer attack_SpreadToPlayer = null;
    private Attack_SpreadBullet attack_SpreadBullet = null;
    private Attack_SpreadAndFaceToPlayer attack_SpreadAndFaceToPlayer = null;

    private Vector2 shootDir = Vector2.zero;
    private float angle = 0;

    #region ReadOnlys
    private readonly string bulletBW = typeof(Bullet_RoundBlackWhite).ToString();
    private readonly string bulletRed = typeof(Bullet_RoundRed).ToString();
    private readonly string bulletBlue = typeof(Bullet_RoundBlue).ToString();
    private readonly string bulletCyan = typeof(Bullet_RoundCyan).ToString();

    private readonly string sharpBlue = typeof(Bullet_SharpBlue).ToString();
    private readonly string sharpRed = typeof(Bullet_SharpRed).ToString();
    private readonly string sharpCyan = typeof(Bullet_SharpCyan).ToString();

    private readonly WaitForSeconds ppFiveSecWait = new WaitForSeconds(0.05f);
    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds halfSecWait = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);
    private readonly WaitForSeconds threeSecWait = new WaitForSeconds(3f);
    #endregion

    private void Awake()
    {
        dicState[State.Default] = gameObject.AddComponent<State_Empty>();

        // 생성
        create = gameObject.AddComponent<Create_MoveCenter>();
        create.moveDur = 1f;

        dicState[State.Create] = create;


        // 이동
        move_Back = gameObject.AddComponent<Move_Back>();
        move_Position = gameObject.AddComponent<Move_SetPosition>();
        move_Back.speed = 0.5f;


        dicState[State.Move] = move_Back;

        // 공격
        attack_SpreadToPlayer = gameObject.AddComponent<Attack_SpreadBulletToPlayer>();
        attack_Round = gameObject.AddComponent<Attack_RoundBullet>();
        attack_RoundCharge = gameObject.AddComponent<Attack_RoundCharge>();
        attack_RoundGoBack = gameObject.AddComponent<Attack_RoundGoBack>();

        attack_SpreadAndFaceToPlayer = gameObject.AddComponent<Attack_SpreadAndFaceToPlayer>();
        attack_SpreadBullet = gameObject.AddComponent<Attack_SpreadBullet>();

        dicState[State.Attack] = attack_SpreadToPlayer;

        // 죽음
        dicState[State.Die] = gameObject.AddComponent<Die_RemoveAndGiveHealth>();

        currentPhase = 0;
    }

    protected override IEnumerator LifeTime()
    {
        yield return null;
        currHp = phaseInfos[currentPhase].hp;

        GameManager.Instance.uiHandler.ShowHpBar();
        GameManager.Instance.uiHandler.SetHpBar(1f);

        waitTimeRoutine = StartCoroutine(WaitForPhaseEnd());

        #region PhaseOne
        isPhaseEnd = false;

        isInvincible = true;
        phaseRoutine = StartCoroutine(PhaseOne());
        yield return new WaitUntil(() => isPhaseEnd);
        StopCoroutine(phaseRoutine);
        #endregion

        PoolManager.Instance.OnUseSpell.Invoke();

        #region PhaseTwo
        isPhaseEnd = false;

        isInvincible = true;
        phaseRoutine = StartCoroutine(PhaseTwo());
        phaseSubAttackRoutine = StartCoroutine(PhaseTwo_SubAttack());
        yield return new WaitUntil(() => isPhaseEnd);
        StopCoroutine(phaseRoutine);
        StopCoroutine(phaseSubAttackRoutine);

        GameManager.Instance.uiHandler.LockFade(false);
        GameManager.Instance.uiHandler.FadeIn();
        #endregion
    }

    private IEnumerator PhaseOne()
    {
        yield return oneSecWait;
        yield return oneSecWait;

        isInvincible = false;

        float speed;

        while (true)
        {
            MovePosition(new Vector2(-4f, 0f), 0.5f);
            yield return halfSecWait;

            speed = 5f;

            for (int i = 0; i < 10; i++)
            {
                //SpreadBullet(1, speed, 0f, 0f, 180f + (20f * i), bulletRed, transform);
                SpreadBulletToPlayer(8, speed, 180f, sharpRed, transform);
                speed += 0.25f;
            }

            RoundBullet(60, 5f, 0f, bulletBlue, transform);
            yield return halfSecWait;

            //==========================================================

            MovePosition(new Vector2(4f, 0f), 0.5f);
            yield return halfSecWait;

            speed = 5f;

            for (int i = 0; i < 10; i++)
            {
                //SpreadBullet(1, speed, 0f, 0f, 180f + (20f * i), bulletRed, transform);
                SpreadBulletToPlayer(9, speed, 180f, sharpBlue, transform);
                speed += 0.25f;
            }

            RoundBullet(60, 5f, 0f, bulletRed, transform);
            yield return halfSecWait;

            //==========================================================

            MovePosition(new Vector2(-4f, 0f), 0.5f);
            yield return halfSecWait;

            speed = 5f;

            for (int i = 0; i < 10; i++)
            {
                //SpreadBullet(1, speed, 0f, 0f, 180f + (20f * i), bulletRed, transform);
                SpreadBulletToPlayer(8, speed, 180f, sharpRed, transform);
                speed += 0.25f;
            }

            RoundBullet(60, 5f, 0f, bulletBlue, transform);
            yield return oneSecWait;

            //==========================================================

            MovePosition(new Vector2(0f, 0f), 1.5f);
            yield return oneSecWait;
            yield return halfSecWait;

            for (int j = 0; j < 4; j++)
            {
                speed = 5f;

                RoundBulletToPlayer(40, 5f, bulletBW, transform);
                RoundBulletToPlayer(20, 7f, bulletBlue, transform);

                for (int i = 0; i < 20; i++)
                {
                    //SpreadBullet(1, speed, 0f, 0f, 180f + (20f * i), bulletRed, transform);
                    SpreadBulletToPlayer(1, speed, 0f, sharpCyan, transform);
                    speed += 0.25f;
                }

                yield return oneSecWait;
            }


            yield return halfSecWait;

            yield return halfSecWait;
        }
        
    }


    private IEnumerator PhaseTwo()
    {
        //MovePosition(new Vector2(0f, 0f), 1.5f);
        GameManager.Instance.uiHandler.FadeOut();
        GameManager.Instance.uiHandler.LockFade(true);

        MovePosition(new Vector2(0f, 0f), 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        isInvincible = false;

        float speed = 0f;

        while (true)
        {
            GetPlayerAngle();
            float a = angle;

            speed = 5f;

            for (int i = 0; i < 8; i++)
            {
                //SpreadBullet(1, speed, 0f, 0f, 180f + (20f * i), bulletRed, transform);
                SpreadBullet(1, speed, 0f, 0f, a, sharpRed, transform);
                speed += 0.25f;
            }

            yield return oneSecWait;

            speed = 5f;

            for (int i = 0; i < 8; i++)
            {
                //SpreadBullet(1, speed, 0f, 0f, 180f + (20f * i), bulletRed, transform);
                SpreadBullet(5, speed, 60f, 0f, a, sharpCyan, transform);
                speed += 0.25f;
            }

            yield return oneSecWait;
        }
    }

    private IEnumerator PhaseTwo_SubAttack()
    {
        yield return oneSecWait;
        yield return oneSecWait;

        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                RoundBulletToPlayer(30, 5f, bulletBlue, transform);
                yield return pOneSecWait;
                RoundBulletToPlayer(15, 4f, bulletRed, transform);
                yield return halfSecWait;
            }

            RoundBulletToPlayer(30, 2f, bulletBW, transform);
        }
    }


    private IEnumerator WaitForPhaseEnd()
    {
        yield return null;

        GameManager.Instance.uiHandler.ShowOrHideTimer(true);

        for (int i = 0; i < phaseInfos[currentPhase].waitTime; i++)
        {
            GameManager.Instance.uiHandler.SetTimerText(phaseInfos[currentPhase].waitTime - i);
            yield return oneSecWait;
        }

        GameManager.Instance.uiHandler.ShowOrHideTimer(false);
        PhaseEnd();
    }

    private void PhaseEnd()
    {
        StopCoroutine(waitTimeRoutine);

        currentPhase++;

        currHp = phaseInfos[currentPhase].hp;
        isPhaseEnd = true;

        waitTimeRoutine = StartCoroutine(WaitForPhaseEnd());
    }

    public override void GetDamage(float damage)
    {
        if (currentState.Equals(State.Die)) return;

        if (!isInvincible)
        {
            currHp -= damage;
        }

        GameManager.Instance.uiHandler.SetHpBar(currHp / phaseInfos[currentPhase].hp);

        StartCoroutine(Blinking());

        CheckHp();
    }

    protected override void CheckHp()
    {
        if (currHp <= 0f)
        {
            if (currentPhase >= phaseInfos.Count - 1)
            {
                SetState(State.Die);
                StopCoroutine(lifeTime);
                is_Die = true;
                SetDisable();
            }
            else
            {
                PhaseEnd();
            }
        }
    }

    private void MovePosition(Vector2 movePos, float moveDur)
    {
        move_Position.movePos = new Vector2(GameManager.Instance.mapCenter.x + movePos.x, 4f + movePos.y);
        move_Position.moveDur = moveDur;
        dicState[State.Move] = move_Position;
        SetState(State.Move);
    }

    private void RoundBulletToPlayer(int bulletCount, float bulletSpeed, string bulletType, Transform shootPos)
    {
        GetPlayerAngle();

        attack_Round.SetValue(bulletCount, bulletSpeed, angle, bulletType, shootPos);
        dicState[State.Attack] = attack_Round;
        PlayState(State.Attack);
    }

    private void GetPlayerAngle()
    {
        shootDir = GameManager.Instance.playerPos.position - transform.position;
        angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
    }

    private void RoundBullet(int bulletCount, float bulletSpeed, float rotate, string bulletType, Transform shootPos)
    {
        attack_Round.SetValue(bulletCount, bulletSpeed, rotate, bulletType, shootPos);
        dicState[State.Attack] = attack_Round;
        PlayState(State.Attack);
    }

    private void RoundGoBack(int bulletCount, float bulletSpeed, float rotate, float goBackTime, float rotateAngle, string bulletType, Transform shootPos)
    {
        attack_RoundGoBack.SetValue(bulletCount, bulletSpeed, rotate, goBackTime, rotateAngle, bulletType, shootPos);
        dicState[State.Attack] = attack_RoundGoBack;
        PlayState(State.Attack);
    }

    private void RoundCharge(int bulletCount, float bulletSpeed, float radiusSpeed, Vector2 shootDir, string bulletType, Transform shootPos)
    {
        attack_RoundCharge.SetValue(bulletCount, bulletSpeed, radiusSpeed, shootDir, bulletType, shootPos);
        dicState[State.Attack] = attack_RoundCharge;
        PlayState(State.Attack);
    }

    private void RoundChargeToPlayer(int bulletCount, float bulletSpeed, float radiusSpeed, string bulletType, Transform shootPos)
    {
        GetPlayerAngle();

        attack_RoundCharge.SetValue(bulletCount, bulletSpeed, radiusSpeed, shootDir, bulletType, shootPos);
        dicState[State.Attack] = attack_RoundCharge;
        PlayState(State.Attack);
    }

    private void SpreadBullet(int bulletCount, float bulletSpeed, float arc, float waitTime, float rotate, string bulletType, Transform shootPos, bool reverse = false)
    {
        attack_SpreadBullet.SetValue(bulletCount, bulletSpeed, arc, waitTime, rotate, bulletType, shootPos, reverse);
        dicState[State.Attack] = attack_SpreadBullet;
        PlayState(State.Attack);
    }

    private void SpreadBulletToPlayer(int bulletCount, float bulletSpeed, float arc, string bulletType, Transform shootPos)
    {
        attack_SpreadToPlayer.SetValue(bulletCount, bulletSpeed, arc, bulletType, shootPos);
        dicState[State.Attack] = attack_SpreadToPlayer;
        PlayState(State.Attack);
    }

    private void SpreadBulleAndFaceToPlayer(int bulletCount, float bulletSpeed, float arc, float rotate, string bulletType, Transform shootPos, float waitTime = 0f)
    {
        attack_SpreadAndFaceToPlayer.SetValue(bulletCount, bulletSpeed, arc, rotate, bulletType, shootPos, waitTime);
        dicState[State.Attack] = attack_SpreadAndFaceToPlayer;
        PlayState(State.Attack);
    }

    public override void SetDisable()
    {
        GameManager.Instance.uiHandler.LockFade(false);
        GameManager.Instance.uiHandler.FadeIn();

        is_Die = true;
        GameManager.Instance.uiHandler.StopHpBar();
        GameManager.Instance.uiHandler.ShowOrHideTimer(false);

        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.enemies.Remove(this);
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        currentPhase = 0;
    }
}
