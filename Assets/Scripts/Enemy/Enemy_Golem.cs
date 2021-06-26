using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[System.Serializable]
public class PhaseInfo
{
    public int waitTime;
    public float hp;
}

public class Enemy_Golem : Enemy
{
    [SerializeField]
    private List<PhaseInfo> phaseInfos = new List<PhaseInfo>();
    private int currentPhase = 0;
    private bool isPhaseEnd = false;
    private bool isInvincible = false;

    private int currentWaitTime = 0;

    private Coroutine phaseRoutine = null;
    private Coroutine phaseMoveRoutine = null;
    private Coroutine phaseSubAttackRoutine = null;

    private Coroutine waitTimeRoutine = null;

    private Create_MoveCenter create = null;

    private Move_Back move_Back = null;
    private Move_SetPosition move_Position = null;

    private Attack_RoundBullet attack_Round = null;
    private Attack_RoundGoBack attack_RoundGoBack = null;
    private Attack_RoundCharge attack_RoundCharge = null;
    private Attack_RoundOrbit attack_RoundOrbit = null;
    private Attack_SpreadBulletToPlayer attack_SpreadToPlayer = null;
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
        attack_RoundOrbit = gameObject.AddComponent<Attack_RoundOrbit>();

        attack_SpreadAndFaceToPlayer = gameObject.AddComponent<Attack_SpreadAndFaceToPlayer>();

        dicState[State.Attack] = attack_SpreadToPlayer;

        // 죽음
        dicState[State.Die] = gameObject.AddComponent<Die_BossDie>();

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
        phaseMoveRoutine = StartCoroutine(PhaseTwo_Move());
        yield return new WaitUntil(() => isPhaseEnd);
        StopCoroutine(phaseRoutine);
        StopCoroutine(phaseMoveRoutine);
        StopCoroutine(phaseSubAttackRoutine);

        GameManager.Instance.uiHandler.LockFade(false);
        GameManager.Instance.uiHandler.FadeIn();
        #endregion

        PoolManager.Instance.OnUseSpell.Invoke();

        #region PhaseThree
        isPhaseEnd = false;

        isInvincible = true;
        phaseRoutine = StartCoroutine(PhaseThree());
        yield return new WaitUntil(() => isPhaseEnd);
        StopCoroutine(phaseRoutine);

        GameManager.Instance.uiHandler.LockFade(false);
        GameManager.Instance.uiHandler.FadeIn();
        #endregion

        PoolManager.Instance.OnUseSpell.Invoke();

        yield return oneSecWait;

        #region PhaseFour
        isPhaseEnd = false;

        isInvincible = true;
        phaseRoutine = StartCoroutine(PhaseFour());
        phaseSubAttackRoutine = StartCoroutine(PhaseFour_SubAttack());
        yield return new WaitUntil(() => isPhaseEnd);
        StopCoroutine(phaseRoutine);
        StopCoroutine(phaseSubAttackRoutine);

        GameManager.Instance.uiHandler.LockFade(false);
        GameManager.Instance.uiHandler.FadeIn();
        #endregion

        PoolManager.Instance.OnUseSpell.Invoke();

        yield return oneSecWait;

        #region PhaseFive
        isPhaseEnd = false;

        isInvincible = true;
        phaseRoutine = StartCoroutine(PhaseFive());
        phaseSubAttackRoutine = StartCoroutine(PhaseFive_SubAttack());
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

        GetPlayerAngle();

        RoundBullet(50, 3f, angle, bulletBlue, transform);
        yield return pOneSecWait;
        RoundBullet(50, 3f, angle, bulletBlue, transform);
        yield return pOneSecWait;
        RoundBullet(50, 3f, angle, bulletBlue, transform);

        yield return oneSecWait;

        GetPlayerAngle();

        RoundBullet(30, 5f, angle, bulletRed, transform);
        yield return pOneSecWait;
        RoundBullet(30, 5f, angle, bulletRed, transform);
        yield return pOneSecWait;
        RoundBullet(30, 5f, angle, bulletRed, transform);

        yield return halfSecWait;

        GetPlayerAngle();

        RoundBullet(30, 5f, angle, bulletRed, transform);
        yield return pOneSecWait;
        RoundBullet(30, 5f, angle, bulletRed, transform);
        yield return pOneSecWait;
        RoundBullet(30, 5f, angle, bulletRed, transform);


        MovePosition(new Vector2(2f, 0f), 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        GetPlayerAngle();

        RoundBullet(50, 3f, angle, bulletBlue, transform);
        yield return pOneSecWait;
        RoundBullet(50, 3f, angle, bulletBlue, transform);
        yield return pOneSecWait;
        RoundBullet(50, 3f, angle, bulletBlue, transform);

        yield return oneSecWait;

        GetPlayerAngle();

        RoundBullet(30, 5f, angle, bulletRed, transform);
        yield return pOneSecWait;
        RoundBullet(30, 5f, angle, bulletRed, transform);
        yield return pOneSecWait;
        RoundBullet(30, 5f, angle, bulletRed, transform);

        yield return halfSecWait;

        GetPlayerAngle();

        RoundBullet(30, 5f, angle, bulletRed, transform);
        yield return pOneSecWait;
        RoundBullet(30, 5f, angle, bulletRed, transform);
        yield return pOneSecWait;
        RoundBullet(30, 5f, angle, bulletRed, transform);


        MovePosition(new Vector2(-2f, 0f), 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                RoundBullet(20, 7f, i * 20f, sharpCyan, transform);
                yield return pOneSecWait;
            }

            GetPlayerAngle();

            RoundBullet(50, 3f, angle, bulletBlue, transform);
            yield return pOneSecWait;
            RoundBullet(50, 3f, angle, bulletBlue, transform);
            yield return pOneSecWait;
            RoundBullet(50, 3f, angle, bulletBlue, transform);

            yield return oneSecWait;

            GetPlayerAngle();

            RoundBullet(30, 5f, angle, bulletRed, transform);
            yield return pOneSecWait;
            RoundBullet(30, 5f, angle, bulletRed, transform);
            yield return pOneSecWait;
            RoundBullet(30, 5f, angle, bulletRed, transform);

            yield return halfSecWait;

            GetPlayerAngle();

            RoundBullet(30, 5f, angle, bulletRed, transform);
            yield return pOneSecWait;
            RoundBullet(30, 5f, angle, bulletRed, transform);
            yield return pOneSecWait;
            RoundBullet(30, 5f, angle, bulletRed, transform);

            MovePosition(new Vector2(2f, 0f), 2f);
            yield return oneSecWait;
            yield return oneSecWait;


            for (int i = 0; i < 5; i++)
            {
                RoundBullet(20, 7f, i * -20f, sharpCyan, transform);
                yield return pOneSecWait;
            }

            GetPlayerAngle();

            RoundBullet(50, 3f, angle, bulletBlue, transform);
            yield return pOneSecWait;
            RoundBullet(50, 3f, angle, bulletBlue, transform);
            yield return pOneSecWait;
            RoundBullet(50, 3f, angle, bulletBlue, transform);

            yield return oneSecWait;

            GetPlayerAngle();

            RoundBullet(30, 5f, angle, bulletRed, transform);
            yield return pOneSecWait;
            RoundBullet(30, 5f, angle, bulletRed, transform);
            yield return pOneSecWait;
            RoundBullet(30, 5f, angle, bulletRed, transform);

            yield return halfSecWait;

            GetPlayerAngle();

            RoundBullet(30, 5f, angle, bulletRed, transform);
            yield return pOneSecWait;
            RoundBullet(30, 5f, angle, bulletRed, transform);
            yield return pOneSecWait;
            RoundBullet(30, 5f, angle, bulletRed, transform);

            MovePosition(new Vector2(-2f, 0f), 2f);
            yield return oneSecWait;
            yield return oneSecWait;

            /*
            for (int i = 0; i < 4; i++)
            {
                RoundBullet(30, 6f, 1f * i, bulletRed, transform);
                yield return pOneSecWait;
            }

            RoundBullet(30, 6f, 6f, bulletRed, transform);

            for (int i = 0; i < 4; i++)
            {
                RoundBullet(30, 6f, -1f * i, bulletBlue, transform);
                yield return pOneSecWait;
            }
            */


            /* 재밌는 패턴
            for (int i = 0; i < 50; i++)
            {
                SpreadBulleAndFaceToPlayer(4, 10f, 120f, 36f * i + 90f, bulletBlue, transform, 0f);
                yield return pOneSecWait;
                yield return pOneSecWait;
            }
            */
            /*
            yield return pOneSecWait;

            yield return oneSecWait;
            yield return halfSecWait;
            yield return oneSecWait;
            yield return halfSecWait;

            for (int i = 0; i < 5; i++)
            {
                RoundBullet(10, 3f, i * 10f, bulletCyan, transform);
                yield return pOneSecWait;
            }
            */
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

        while (true)
        {
            GetPlayerAngle();

            RoundCharge(50, 3f, 2.2f, Vector2.down, bulletRed, transform);
            yield return pOneSecWait;

            yield return oneSecWait;
            yield return oneSecWait;
        }
    }

    private IEnumerator PhaseTwo_SubAttack()
    {
        yield return oneSecWait;
        yield return oneSecWait;

        while (true)
        {
            GetPlayerAngle();

            RoundBulletToPlayer(20, 5f, sharpCyan, transform);
            yield return pOneSecWait;
            RoundBulletToPlayer(30, 2f, bulletBW, transform);
            yield return pOneSecWait;

            yield return oneSecWait;
        }
    }

    private IEnumerator PhaseTwo_Move()
    {
        yield return oneSecWait;
        yield return oneSecWait;

        while (true)
        {
            for (int i = -4; i <= 4; i++)
            {
                MovePosition(new Vector2(i, 0f), 1f);
                yield return oneSecWait;
            }

            for (int i = 4; i >= -4; i--)
            {
                MovePosition(new Vector2(i, 0f), 1f);
                yield return oneSecWait;
            }
        }
    }


    private IEnumerator PhaseThree()
    {
        MovePosition(new Vector2(0f, 0f), 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        isInvincible = false;

        for (int i = 0; i < 2; i++)
        {
            GetPlayerAngle();
            RoundGoBack(30, 3f, angle, 1f, 40f, sharpRed, transform);
            yield return pOneSecWait;

            RoundGoBack(30, 3f, angle, 1f, -20f, sharpBlue, transform);
            yield return pOneSecWait;

            yield return oneSecWait;

            GetPlayerAngle();
            RoundGoBack(30, 3f, angle, 1f, -40f, sharpRed, transform);
            yield return pOneSecWait;

            RoundGoBack(30, 3f, angle, 1f, 20f, sharpBlue, transform);
            yield return pOneSecWait;

            yield return oneSecWait;
        }

        while (true)
        {
            for (int i = 0; i < 2; i++)
            {
                GetPlayerAngle();
                RoundGoBack(30, 3f, angle, 1f, 40f, sharpRed, transform);
                yield return pOneSecWait;

                RoundGoBack(30, 3f, angle, 1f, -20f, sharpBlue, transform);
                yield return pOneSecWait;

                yield return oneSecWait;

                RoundChargeToPlayer(15, 5f, 3f, bulletBW, transform);

                GetPlayerAngle();
                RoundGoBack(30, 3f, angle, 1f, -40f, sharpRed, transform);
                yield return pOneSecWait;

                RoundGoBack(30, 3f, angle, 1f, 20f, sharpBlue, transform);
                yield return pOneSecWait;

                yield return oneSecWait;
            }

        }
    }


    private IEnumerator PhaseFour()
    {
        GameManager.Instance.uiHandler.FadeOut();
        GameManager.Instance.uiHandler.LockFade(true);

        MovePosition(new Vector2(0f, -4f), 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        isInvincible = false;

        while (true)
        {
            RoundOrbit(6, 0.5f, 1f, 30f, Vector2.down, bulletRed, transform);
            RoundOrbit(5, 0.5f, 1f, 30f, Vector2.up, bulletBlue, transform);
            yield return halfSecWait;
        }
    }
    private IEnumerator PhaseFour_SubAttack()
    {
        yield return threeSecWait;
        yield return threeSecWait;

        while (true)
        {
            RoundChargeToPlayer(15, 2f, 1.5f, bulletBW, transform);

            yield return threeSecWait;
            yield return oneSecWait;
        }
    }

    private IEnumerator PhaseFive()
    {
        GameManager.Instance.uiHandler.FadeOut();
        GameManager.Instance.uiHandler.LockFade(true);

        MovePosition(new Vector2(0f, -4f), 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        isInvincible = false;

        while (true)
        {
            RoundOrbit(6, 0.5f, 1f, 30f, Vector2.down, bulletRed, transform);
            yield return halfSecWait;
            RoundOrbit(6, 0.5f, 1f, 30f, Vector2.up, bulletBlue, transform);
            yield return halfSecWait;

            RoundOrbit(7, 0.5f, 1f, -30f, Vector2.down, bulletBlue, transform);
            yield return halfSecWait;
            RoundOrbit(7, 0.5f, 1f, -30f, Vector2.up, bulletRed, transform);
            yield return halfSecWait;
        }
    }
    private IEnumerator PhaseFive_SubAttack()
    {
        yield return threeSecWait;
        yield return threeSecWait;

        while (true)
        {
            GetPlayerAngle();
            RoundBulletToPlayer(10, 2f, bulletBW, transform);

            yield return oneSecWait;
            yield return oneSecWait;
        }
    }



    private IEnumerator WaitForPhaseEnd()
    {
        yield return null;

        GameManager.Instance.uiHandler.ShowOrHideTimer(true);

        for (int i = 0; i < phaseInfos[currentPhase].waitTime; i++)
        {
            currentWaitTime = phaseInfos[currentPhase].waitTime - i;
            GameManager.Instance.uiHandler.SetTimerText(currentWaitTime);
            yield return oneSecWait;
        }

        GameManager.Instance.uiHandler.ShowOrHideTimer(false);
        PhaseEnd();
    }

    private void PhaseEnd()
    {
        StopCoroutine(waitTimeRoutine);

        if (currentPhase >= phaseInfos.Count - 1)
        {
            SetState(State.Die);
            StopCoroutine(lifeTime);
            is_Die = true;
            SetDisable();

            return;
        }

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
                GameManager.Instance.uiHandler.ShowMessage($"Phase Bonus! + {currentWaitTime * 1000}", fontSize: 40, waitTime: 1.5f);
                GameManager.Instance.uiHandler.AddScore(currentWaitTime * 1000);

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

    private void RoundOrbit(int bulletCount, float bulletSpeed, float radiusSpeed, float rotateSpeed, Vector2 shootDir, string bulletType, Transform shootPos)
    {
        attack_RoundOrbit.SetValue(bulletCount, bulletSpeed, radiusSpeed, rotateSpeed, shootDir, bulletType, shootPos);
        dicState[State.Attack] = attack_RoundOrbit;
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
        Camera.main.DOShakePosition(2f, 1.5f, 15, 20, true)
            .SetEase(Ease.InQuart);

        GameManager.Instance.uiHandler.LockFade(false);
        GameManager.Instance.uiHandler.FadeIn();

        is_Die = true;
        GameManager.Instance.uiHandler.StopHpBar();
        GameManager.Instance.uiHandler.ShowOrHideTimer(false);

        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.enemies.Remove(this);
        }

        GameManager.Instance.uiHandler.ShowMessage($"Defeat Bonus! + 100000", fontSize: 40, waitTime: 1.5f);
        GameManager.Instance.uiHandler.AddScore(100000);

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        currentPhase = 0;
    }
}
