using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jumper : Enemy
{
    public int phaseWaitTime = 40;

    private Vector2 shootDir = Vector2.zero;
    private float angle = 0;

    private Create_MoveCenter create = null;

    private Move_Back move_Back = null;
    private Move_SetPosition move_Position = null;

    private Attack_RoundBullet attack_Round = null;
    private Attack_SpreadBulletToPlayer attack_SpreadToPlayer = null;

    private readonly string bulletBW = typeof(Bullet_RoundBlackWhite).ToString();
    private readonly string bulletRed = typeof(Bullet_RoundRed).ToString();
    private readonly string bulletBlue = typeof(Bullet_RoundBlue).ToString();
    private readonly string bulletCyan = typeof(Bullet_RoundCyan).ToString();

    private readonly WaitForSeconds ppFiveSecWait = new WaitForSeconds(0.05f);
    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds halfSecWait = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

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

        dicState[State.Attack] = attack_SpreadToPlayer;

        // 죽음
        dicState[State.Die] = gameObject.AddComponent<Die_RemoveAndGiveBomb>();


    }

    protected override IEnumerator LifeTime()
    {
        yield return null;

        GameManager.Instance.uiHandler.ShowHpBar();
        GameManager.Instance.uiHandler.SetHpBar(1f);

        StartCoroutine(WaitForPhaseEnd());

        yield return oneSecWait;
        yield return halfSecWait;

        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                RoundBullet(10, 6f, 60f * i, bulletRed, transform);
                RoundBullet(12, 4f, 80f * i, bulletBlue, transform);

                yield return pOneSecWait;
            }

            SpreadBulletToPlayer(7, 5f, 120f, bulletCyan, transform);
            SpreadBulletToPlayer(7, 6f, 120f, bulletCyan, transform);
            SpreadBulletToPlayer(7, 7f, 120f, bulletCyan, transform);

            yield return oneSecWait;
            yield return halfSecWait;

            MovePosition(new Vector2(-3f, 0f), 1.5f);

            yield return oneSecWait;
            yield return halfSecWait;

            for (int i = 0; i < 20; i++)
            {
                RoundBullet(4, 5f, 20f * i, bulletRed, transform);
                RoundBullet(3, 5f, -10f * i, bulletBlue, transform);

                yield return ppFiveSecWait;
            }

            SpreadBulletToPlayer(7, 5f, 120f, bulletCyan, transform);
            SpreadBulletToPlayer(7, 6f, 120f, bulletCyan, transform);
            SpreadBulletToPlayer(7, 7f, 120f, bulletCyan, transform);

            yield return oneSecWait;
            yield return halfSecWait;

            MovePosition(new Vector2(3f, 0f), 1.5f);

            yield return oneSecWait;
            yield return halfSecWait;

            for (int i = 0; i < 3; i++)
            {
                SpreadBulletToPlayer(7, 5f, 120f, bulletCyan, transform);
                SpreadBulletToPlayer(7, 6f, 120f, bulletCyan, transform);
                SpreadBulletToPlayer(7, 7f, 120f, bulletCyan, transform);
                yield return halfSecWait;
            }

            for (int i = 0; i < 20; i++)
            {
                RoundBullet(4, 5f, 40f * i, bulletRed, transform);
                yield return ppFiveSecWait;
            }

            SpreadBulletToPlayer(7, 5f, 120f, bulletCyan, transform);
            SpreadBulletToPlayer(7, 6f, 120f, bulletCyan, transform);
            SpreadBulletToPlayer(7, 7f, 120f, bulletCyan, transform);

            yield return oneSecWait;
            yield return halfSecWait;

            MovePosition(Vector2.zero, 1.5f);

            yield return oneSecWait;
            yield return halfSecWait;
        }
    }

    private IEnumerator WaitForPhaseEnd()
    {
        yield return null;

        GameManager.Instance.uiHandler.ShowOrHideTimer(true);

        for (int i = 0; i < phaseWaitTime; i++)
        {
            GameManager.Instance.uiHandler.SetTimerText(phaseWaitTime - i);
            yield return oneSecWait;
        }

        GameManager.Instance.uiHandler.ShowOrHideTimer(false);
        StopCoroutine(lifeTime);
        PhaseEnd();
    }

    private void PhaseEnd()
    {
        if (currentState != State.Default)
        {
            SetState(State.Default);
            dicState[State.Move] = move_Back;
            SetState(State.Move);
        }
    }

    public override void GetDamage(float damage)
    {
        if (currentState.Equals(State.Die)) return;

        currHp -= damage;
        GameManager.Instance.uiHandler.SetHpBar(currHp / maxHp);

        StartCoroutine(Blinking());

        CheckHp();
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
        shootDir = transform.position - GameManager.Instance.playerPos.position;
        angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        attack_Round.SetValue(bulletCount, bulletSpeed, angle, bulletType, shootPos);
        dicState[State.Attack] = attack_Round;
        PlayState(State.Attack);
    }

    private void RoundBullet(int bulletCount, float bulletSpeed, float rotate, string bulletType, Transform shootPos)
    {
        attack_Round.SetValue(bulletCount, bulletSpeed, rotate, bulletType, shootPos);
        dicState[State.Attack] = attack_Round;
        PlayState(State.Attack);
    }

    private void SpreadBulletToPlayer(int bulletCount, float bulletSpeed, float arc, string bulletType, Transform shootPos)
    {
        attack_SpreadToPlayer.SetValue(bulletCount, bulletSpeed, arc, bulletType, shootPos);
        dicState[State.Attack] = attack_SpreadToPlayer;
        PlayState(State.Attack);
    }

    public override void SetDisable()
    {
        is_Die = true;
        GameManager.Instance.uiHandler.StopHpBar();
        GameManager.Instance.uiHandler.ShowOrHideTimer(false);

        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.enemies.Remove(this);
        }

        gameObject.SetActive(false);
    }
}
