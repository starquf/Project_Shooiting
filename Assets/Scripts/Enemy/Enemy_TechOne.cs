using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy_TechOne : Enemy
{
    [SerializeField]
    private List<PhaseInfo> phaseInfos = new List<PhaseInfo>();
    private int currentPhase = 0;
    private bool isPhaseEnd = false;
    private bool isInvincible = false;

    private int currentWaitTime = 0;

    [SerializeField]
    private List<Transform> shootPos = new List<Transform>();

    private Coroutine phaseRoutine = null;
    private Coroutine phaseSubAttackRoutine = null;

    private Coroutine waitTimeRoutine = null;

    private Create_MoveCenter create = null;

    private Move_Back move_Back = null;
    private Move_SetPosition move_Position = null;

    private Attack_RoundBullet attack_Round = null;
    private Attack_RoundGoBack attack_RoundGoBack = null;
    private Attack_RoundCharge attack_RoundCharge = null;
    private Attack_RoundGoAndStop attack_RoundGoAndStop = null;
    private Attack_RoundStopAndGo attack_RoundStopAndGo = null;
    private Attack_RoundHoming attack_RoundHoming = null;
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
    private readonly string sharpYellow = typeof(Bullet_SharpYellow).ToString();

    private readonly WaitForSeconds ppOneSecWait = new WaitForSeconds(0.01f);
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
        attack_RoundGoAndStop = gameObject.AddComponent<Attack_RoundGoAndStop>();
        attack_RoundStopAndGo = gameObject.AddComponent<Attack_RoundStopAndGo>();
        attack_RoundHoming = gameObject.AddComponent<Attack_RoundHoming>();


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
        yield return new WaitUntil(() => isPhaseEnd);
        StopCoroutine(phaseRoutine);

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
        #endregion

        PoolManager.Instance.OnUseSpell.Invoke();

        #region PhaseFour
        isPhaseEnd = false;

        isInvincible = true;
        phaseRoutine = StartCoroutine(PhaseFour());
        yield return new WaitUntil(() => isPhaseEnd);
        StopCoroutine(phaseRoutine);

        GameManager.Instance.uiHandler.LockFade(false);
        GameManager.Instance.uiHandler.FadeIn();
        #endregion

        PoolManager.Instance.OnUseSpell.Invoke();
        yield return oneSecWait;

        #region PhaseFive
        isPhaseEnd = false;

        isInvincible = true;
        phaseRoutine = StartCoroutine(PhaseFive());
        yield return new WaitUntil(() => isPhaseEnd);
        StopCoroutine(phaseRoutine);
        #endregion

        PoolManager.Instance.OnUseSpell.Invoke();
        yield return oneSecWait;

        #region PhaseSix
        isPhaseEnd = false;

        isInvincible = true;
        phaseRoutine = StartCoroutine(PhaseSix());
        phaseSubAttackRoutine = StartCoroutine(PhaseSix_SubAttack());
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


        for (int i = 0; i < 5; i++)
        {
            RoundGoAndStop(40, 12f, i, 0.65f, sharpBlue, transform);
            RoundBullet(30, 4f, i * 10f, bulletRed, shootPos[0]);

            yield return halfSecWait;
        }

        for (int i = 0; i < 5; i++)
        {
            RoundGoAndStop(40, 12f, -i, 0.65f, sharpBlue, transform);
            RoundBullet(30, 4f, i * 10f, bulletRed, shootPos[1]);

            yield return halfSecWait;
        }

        while (true)
        {

            for (int i = 0; i < 5; i++)
            {
                RoundGoAndStop(40, 12f, i, 0.65f, sharpBlue, transform);
                RoundBullet(30, 4f, i * 10f, bulletRed, shootPos[0]);

                yield return halfSecWait;
            }

            MovePosition(new Vector2(1f, 0f), 2f);

            for (int i = 0; i < 5; i++)
            {
                RoundGoAndStop(40, 12f, -i, 0.65f, sharpBlue, transform);
                RoundBullet(30, 4f, i * 10f, bulletRed, shootPos[1]);

                yield return halfSecWait;
            }

            MovePosition(new Vector2(-1f, 0f), 2f);

            //RoundHoming(30, 4f, 0f, 0.5f, bulletRed, shootPos[1]);
        }
    }

    private IEnumerator PhaseTwo()
    {
        GameManager.Instance.uiHandler.FadeOut();
        GameManager.Instance.uiHandler.LockFade(true);

        MovePosition(new Vector2(0f, 0f), 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        isInvincible = false;

        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                RoundBullet(16, Random.Range(2f, 4f), i * 5f, sharpRed, shootPos[0]);
                RoundBullet(16, Random.Range(2f, 4f), -i * 5f, sharpRed, shootPos[1]);

                yield return pOneSecWait;
                yield return pOneSecWait;
                yield return pOneSecWait;
            }

            MovePosition(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 2f);

            RoundBulletToPlayer(40, 3f, sharpBlue, transform);
        }
    }


    private IEnumerator PhaseThree()
    {
        MovePosition(new Vector2(0f, 0f), 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        isInvincible = false;

        while (true)
        {
            //RoundGoAndStop(30, 12, 0f, 0.8f, sharpRed, transform);

            for (int i = 0; i < 5; i++)
            {
                RoundGoAndStop(30, 12, i * 10f, 0.65f, sharpRed, shootPos[0]);
                RoundGoAndStop(30, 12, -i * 10f, 0.65f, sharpRed, shootPos[1]);

                yield return halfSecWait;
                yield return pOneSecWait;
            }

            RoundBullet(80, 3f, 0f, sharpBlue, transform);

            //MovePosition(new Vector2(3f, 0f), );

            for (int i = 0; i < 5; i++)
            {
                RoundGoAndStop(30, 12, -i * 10f, 0.65f, sharpRed, shootPos[0]);
                RoundGoAndStop(30, 12, i * 10f, 0.65f, sharpRed, shootPos[1]);

                yield return halfSecWait;
                yield return pOneSecWait;
            }
        }
    }

    private IEnumerator PhaseFour()
    {
        yield return oneSecWait;

        GameManager.Instance.uiHandler.FadeOut();
        GameManager.Instance.uiHandler.LockFade(true);

        MovePosition(new Vector2(0f, -0.5f), 2f);
        yield return oneSecWait;
        yield return oneSecWait;

        for (int i = 0; i < 20; i++)
        {
            RoundStopAndGo(3, Random.Range(3f, 5f), i * 12f, 3f, sharpRed, shootPos[0], 4f);
            RoundStopAndGo(3, Random.Range(3f, 5f), -i * 12f, 3f, sharpRed, shootPos[1], 4f);

            yield return pOneSecWait;
        }

        yield return oneSecWait;
        RoundBullet(30, 7f, 0f, bulletBlue, transform);
        yield return oneSecWait;
        yield return oneSecWait;

        for (int i = 0; i < 20; i++)
        {
            RoundStopAndGo(3, Random.Range(3f, 5f), i * 12f, 3f, sharpRed, shootPos[0], i * 0.3f);
            RoundStopAndGo(3, Random.Range(3f, 5f), -i * 12f, 3f, sharpRed, shootPos[1], i * 0.3f);
            RoundStopAndGo(2, Random.Range(3f, 5f), i * 18f, 3f, sharpRed, shootPos[0], 4f);
            RoundStopAndGo(2, Random.Range(3f, 5f), -i * 18f, 3f, sharpRed, shootPos[1], 4f);

            yield return pOneSecWait;
        }

        yield return oneSecWait;
        RoundBullet(30, 7f, 0f, bulletBlue, transform);
        yield return oneSecWait;
        yield return oneSecWait;

        for (int i = 0; i < 20; i++)
        {
            RoundStopAndGo(4, 5f, -i * 12f, 3f, sharpBlue, shootPos[0], i * 0.5f);
            RoundStopAndGo(4, 5f, i * 12f, 3f, sharpBlue, shootPos[1], i * 0.5f);

            yield return pOneSecWait;
        }

        yield return oneSecWait;
        RoundBullet(30, 7f, 0f, bulletBlue, transform);
        yield return oneSecWait;
        yield return oneSecWait;

        for (int i = 0; i < 20; i++)
        {
            RoundStopAndGo(3, Random.Range(3f, 5f), i * 6f, 3f, sharpRed, shootPos[0], 4f);
            RoundStopAndGo(3, Random.Range(3f, 5f), i * 6f, 3f, sharpRed, shootPos[0], 6f);
            RoundStopAndGo(3, Random.Range(3f, 5f), -i * 6f, 3f, sharpRed, shootPos[1], 4f);
            RoundStopAndGo(3, Random.Range(3f, 5f), -i * 6f, 3f, sharpRed, shootPos[1], 6f);

            yield return pOneSecWait;
        }


        yield return oneSecWait;
        RoundBullet(30, 7f, 0f, bulletBlue, transform);
        yield return oneSecWait;
        yield return oneSecWait;

        for (int i = 0; i < 20; i++)
        {
            RoundStopAndGo(3, Random.Range(3f, 5f), -i * 6f, 3f, sharpRed, shootPos[0], 5f);
            RoundStopAndGo(3, Random.Range(3f, 5f), i * 6f, 3f, sharpBlue, shootPos[0], i * 0.25f);
            RoundStopAndGo(3, Random.Range(3f, 5f), -i * 6f, 3f, sharpRed, shootPos[1], 5f);
            RoundStopAndGo(3, Random.Range(3f, 5f), i * 6f, 3f, sharpBlue, shootPos[1], i * 0.25f);

            yield return pOneSecWait;
        }

        yield return oneSecWait;
        RoundBullet(30, 7f, 0f, bulletBlue, transform);
        yield return oneSecWait;
        yield return oneSecWait;

        for (int i = 0; i < 20; i++)
        {
            RoundStopAndGo(5, Random.Range(3f, 5f), i * 6f, 3f, sharpRed, shootPos[0], Random.Range(3f, 6f));
            RoundStopAndGo(5, Random.Range(3f, 5f), -i * 6f, 3f, sharpBlue, shootPos[0], Random.Range(3f, 6f));
            RoundStopAndGo(5, Random.Range(3f, 5f), i * 6f, 3f, sharpRed, shootPos[1], Random.Range(3f, 6f));
            RoundStopAndGo(5, Random.Range(3f, 5f), -i * 6f, 3f, sharpBlue, shootPos[1], Random.Range(3f, 6f));

            yield return pOneSecWait;
        }

        yield return oneSecWait;
        RoundBullet(30, 7f, 0f, bulletBlue, transform);
        yield return oneSecWait;
        yield return oneSecWait;

        for (int i = 0; i < 20; i++)
        {
            RoundStopAndGo(5, 4f, i * 6f, 3f, sharpRed, shootPos[0], 4f);
            RoundStopAndGo(5, 4f, -i * 6f, 3f, sharpRed, shootPos[1], 4f);
            RoundStopAndGo(5, 5f, i * 6f, 3f, sharpBlue, transform, 4f);
            RoundStopAndGo(5, 5f, -i * 6f, 3f, sharpBlue, transform, 4f);

            yield return pOneSecWait;
        }

        yield return oneSecWait;
        RoundBullet(30, 7f, 0f, bulletBlue, transform);
        yield return oneSecWait;
        yield return oneSecWait;

        for (int i = 0; i < 20; i++)
        {
            RoundStopAndGo(10, Random.Range(-6f, -4f), i * 6f, 3f, sharpBlue, transform, 8f);
            RoundStopAndGo(5, Random.Range(3f, 5f), i * 6f, 3f, sharpRed, shootPos[0], 5f);
            RoundStopAndGo(5, Random.Range(3f, 5f), i * 6f, 3f, sharpRed, shootPos[1], 5f);

            yield return pOneSecWait;
        }

        isInvincible = false;

        yield return oneSecWait;
        RoundBullet(30, 7f, 0f, bulletBlue, transform);
        yield return oneSecWait;
        yield return oneSecWait;
        yield return oneSecWait;
        yield return oneSecWait;
        yield return oneSecWait;
        yield return oneSecWait;


        while (true)
        {
            for (int i = 0; i < 20; i++)
            {
                RoundStopAndGo(5, Random.Range(3f, 5f), i * 6f, 3f, sharpRed, shootPos[0], Random.Range(3f, 6f));
                RoundStopAndGo(5, Random.Range(3f, 5f), -i * 6f, 3f, sharpBlue, shootPos[0], Random.Range(3f, 6f));
                RoundStopAndGo(5, Random.Range(3f, 5f), i * 6f, 3f, sharpRed, shootPos[1], Random.Range(3f, 6f));
                RoundStopAndGo(5, Random.Range(3f, 5f), -i * 6f, 3f, sharpBlue, shootPos[1], Random.Range(3f, 6f));

                yield return pOneSecWait;
            }

            yield return oneSecWait;
            RoundBullet(30, 7f, 0f, bulletBlue, transform);
            yield return oneSecWait;
            yield return oneSecWait;

            for (int i = 0; i < 20; i++)
            {
                RoundStopAndGo(5, 4f, i * 6f, 3f, sharpRed, shootPos[0], 4f);
                RoundStopAndGo(5, 4f, -i * 6f, 3f, sharpRed, shootPos[1], 4f);
                RoundStopAndGo(5, 5f, i * 6f, 3f, sharpBlue, transform, 4f);
                RoundStopAndGo(5, 5f, -i * 6f, 3f, sharpBlue, transform, 4f);

                yield return pOneSecWait;
            }

            yield return oneSecWait;
            RoundBullet(30, 7f, 0f, bulletBlue, transform);
            yield return oneSecWait;
            yield return oneSecWait;

            for (int i = 0; i < 20; i++)
            {
                RoundStopAndGo(10, Random.Range(-6f, -4f), i * 6f, 3f, sharpBlue, transform, 8f);
                RoundStopAndGo(5, Random.Range(3f, 5f), i * 6f, 3f, sharpRed, shootPos[0], 5f);
                RoundStopAndGo(5, Random.Range(3f, 5f), i * 6f, 3f, sharpRed, shootPos[1], 5f);

                yield return pOneSecWait;
            }

            yield return oneSecWait;
            RoundBullet(30, 7f, 0f, bulletBlue, transform);
            yield return oneSecWait;
            yield return oneSecWait;
            yield return oneSecWait;
            yield return oneSecWait;
            yield return oneSecWait;
            yield return oneSecWait;
        }
    }

    private IEnumerator PhaseFive()
    {
        yield return oneSecWait;
        yield return oneSecWait;

        isInvincible = false;

        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 30; i++)
            {
                RoundGoAndStop(4, 12, i * 10f, 0.65f, sharpRed, shootPos[0]);
                RoundGoAndStop(4, 12, i * 10f, 0.65f, sharpRed, shootPos[1]);

                yield return ppFiveSecWait;
                yield return ppOneSecWait;
                yield return ppOneSecWait;
            }

            for (int i = 0; i < 20; i++)
            {
                //RoundGoAndStop(4, 12, -i * 10f, 0.8f, sharpRed, transform);

                RoundGoAndStop(4, 12, -i * 7f, 0.65f, sharpRed, shootPos[0]);
                RoundGoAndStop(4, 12, -i * 7f, 0.65f, sharpRed, shootPos[1]);

                yield return ppFiveSecWait;
                yield return ppOneSecWait;
                yield return ppOneSecWait;
            }
        }

        while (true)
        {
            for (int i = 0; i < 30; i++)
            {
                RoundGoAndStop(4, 12, i * 10f, 0.65f, sharpRed, shootPos[0]);
                RoundGoAndStop(4, 12, i * 10f, 0.65f, sharpRed, shootPos[1]);

                yield return ppFiveSecWait;
                yield return ppOneSecWait;
            }

            RoundHoming(20, 5f, 0f, 1f, sharpBlue, shootPos[0]);
            RoundHoming(20, 5f, 0f, 1f, sharpBlue, shootPos[1]);
            yield return ppFiveSecWait;

            for (int i = 0; i < 20; i++)
            {
                //RoundGoAndStop(4, 12, -i * 10f, 0.8f, sharpRed, transform);
                
                RoundGoAndStop(4, 12, -i * 7f, 0.65f, sharpRed, shootPos[0]);
                RoundGoAndStop(4, 12, -i * 7f, 0.65f, sharpRed, shootPos[1]);

                yield return ppFiveSecWait;
                yield return ppOneSecWait;
            }


            /*
                RoundGoAndStop(50, 12, -i, 0.8f, sharpRed, shootPos[0]);
                RoundGoAndStop(50, 12, -i, 0.8f, sharpRed, shootPos[1]);

            */
        }
    }

    private IEnumerator PhaseSix()
    {
        yield return oneSecWait;

        GameManager.Instance.uiHandler.FadeOut();
        GameManager.Instance.uiHandler.LockFade(true);

        MovePosition(new Vector2(0f, -4f), 2f);
        yield return oneSecWait;
        yield return oneSecWait;


        while (true)
        {
            for (int i = 0; i < 90; i++)
            {
                RoundStopAndGo(4, -3f, i, 3f, sharpBlue, transform, 6f, 1.5f);
                yield return ppFiveSecWait;
                yield return ppOneSecWait;
            }
        }
    }

    private IEnumerator PhaseSix_SubAttack()
    {
        yield return oneSecWait;
        yield return oneSecWait;
        yield return oneSecWait;
        yield return threeSecWait;
        yield return threeSecWait;

        isInvincible = false;

        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                RoundBulletToPlayer(10, 2f, bulletRed, shootPos[0]);
                RoundBulletToPlayer(10, 2f, bulletRed, shootPos[1]);

                yield return oneSecWait;
                yield return oneSecWait;
            }

            yield return oneSecWait;

            for (int j = 0; j < 20; j++)
            {
                RoundBullet(4, 2f, 0, sharpYellow, shootPos[0]);
                RoundBullet(6, 2f, j * 10f + 10f, sharpRed, shootPos[1]);
                yield return halfSecWait;
            }

            yield return oneSecWait;

            for (int j = 0; j < 20; j++)
            {
                RoundBullet(4, 2f, 0, sharpYellow, shootPos[1]);
                RoundBullet(6, 2f, j * 10f + 10f, sharpRed, shootPos[0]);
                yield return halfSecWait;
            }

            yield return oneSecWait;

            for (int j = 0; j < 10; j++)
            {
                RoundBullet(4, 2f, -j * 5f, sharpRed, shootPos[0]);
                RoundBullet(4, 2f, j * 5f, sharpRed, shootPos[1]);
                yield return halfSecWait;
            }

            for (int j = 0; j < 10; j++)
            {
                RoundBullet(4, 2f, j * 5f, sharpRed, shootPos[0]);
                RoundBullet(4, 2f, -j * 5f, sharpRed, shootPos[1]);
                yield return halfSecWait;
            }

            yield return oneSecWait;

            for (int j = 0; j < 60; j++)
            {
                RoundBullet(6, 2f, j * 5f, sharpRed, shootPos[0]);
                RoundBullet(6, 2f, -j * 5f, sharpRed, shootPos[1]);
                yield return halfSecWait;
            }

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

    private void RoundGoAndStop(int bulletCount, float bulletSpeed, float rotate, float stopTime, string bulletType, Transform shootPos)
    {
        attack_RoundGoAndStop.SetValue(bulletCount, bulletSpeed, rotate, stopTime, bulletType, shootPos);
        dicState[State.Attack] = attack_RoundGoAndStop;
        PlayState(State.Attack);
    }

    private void RoundStopAndGo(int bulletCount, float bulletSpeed, float rotate, float stopTime, string bulletType, Transform shootPos, float goSpeed = 3f, float goStopTime = 0.5f)
    {
        attack_RoundStopAndGo.SetValue(bulletCount, bulletSpeed, rotate, stopTime, bulletType, shootPos, goSpeed, goStopTime);
        dicState[State.Attack] = attack_RoundStopAndGo;
        PlayState(State.Attack);
    }

    private void RoundHoming(int bulletCount, float bulletSpeed, float rotate, float radius, string bulletType, Transform shootPos)
    {
        attack_RoundHoming.SetValue(bulletCount, bulletSpeed, rotate, radius, bulletType, shootPos);
        dicState[State.Attack] = attack_RoundHoming;
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

        GameManager.Instance.uiHandler.ShowMessage($"Defeat Bonus! + 200000", fontSize: 40, waitTime: 1.5f);
        GameManager.Instance.uiHandler.AddScore(200000);

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        currentPhase = 0;
    }
}
