using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TrashPlant : Enemy
{
    public float moveDur = 3f; // 움직이는 속도

    private Move_BezierCurve move = null;
    private Attack_SpreadBulletToPlayer attack = null;

    private readonly string bulletCyan = typeof(Bullet_RoundCyan).ToString();

    private readonly WaitForSeconds ppOneSecWait = new WaitForSeconds(0.01f);
    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds halfSecWait = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    private void Awake()
    {
        // 생성
        dicState[State.Create] = gameObject.AddComponent<State_Empty>();

        // 이동
        move = gameObject.AddComponent<Move_BezierCurve>();
        move.moveDur = moveDur;
        dicState[State.Move] = move;

        // 공격
        attack = gameObject.AddComponent<Attack_SpreadBulletToPlayer>();
        dicState[State.Attack] = attack;

        // 죽음
        dicState[State.Die] = gameObject.AddComponent<Die_GivePoint>();
    }

    protected override IEnumerator LifeTime()
    {
        SetState(State.Move);
        yield return oneSecWait;

        SpreadBulletToPlayer(1, 5f, 0f, bulletCyan, transform);
        yield return pOneSecWait;
        SpreadBulletToPlayer(1, 5.5f, 0f, bulletCyan, transform);
        yield return pOneSecWait;
        SpreadBulletToPlayer(1, 6f, 0f, bulletCyan, transform);
    }

    private void SpreadBulletToPlayer(int bulletCount, float bulletSpeed, float arc, string bulletType, Transform shootPos)
    {
        attack.SetValue(bulletCount, bulletSpeed, arc, bulletType, shootPos);
        dicState[State.Attack] = attack;
        PlayState(State.Attack);
    }
}
