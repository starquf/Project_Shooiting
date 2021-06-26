using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Uni : Enemy
{
    public float moveSpeed = 3f; // 움직이는 속도

    private Create_MoveDown create = null;
    private Move_Back move = null;
    private Attack_RoundBullet attack = null;

    private readonly string bulletBW = typeof(Bullet_RoundBlackWhite).ToString();

    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    private void Awake()
    {
        // 생성
        create = gameObject.AddComponent<Create_MoveDown>();
        create.moveY = 5f;
        create.moveDur = 1f;

        dicState[State.Create] = create;

        // 이동
        move = gameObject.AddComponent<Move_Back>();
        move.speed = moveSpeed;
        dicState[State.Move] = move;

        // 공격
        attack = gameObject.AddComponent<Attack_RoundBullet>();
        dicState[State.Attack] = attack;

        // 죽음
        dicState[State.Die] = gameObject.AddComponent<Die_GivePoint>();
    }

    protected override IEnumerator LifeTime()
    {
        yield return oneSecWait;
        RoundBullet(8, 5f, bulletBW, transform);

        yield return oneSecWait;
        yield return oneSecWait;
        if (!currentState.Equals(State.Die))
            SetState(State.Move);
    }

    private void RoundBullet(int bulletCount, float bulletSpeed, string bulletType, Transform shootPos)
    {
        attack.SetValue(bulletCount, bulletSpeed, UnityEngine.Random.Range(0f, 360f), bulletType, shootPos);
        dicState[State.Attack] = attack;
        PlayState(State.Attack);
    }

    /*
    private void SpreadBulletToPlayer(int bulletCount, float bulletSpeed, float arc, string bulletType, Transform shootPos)
    {
        at.SetValue(bulletCount, bulletSpeed, arc, bulletType, shootPos);
        dicState[State.Attack] = at;
        PlayState(State.Attack);
    }
    */
}
