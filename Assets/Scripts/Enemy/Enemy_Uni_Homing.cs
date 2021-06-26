using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Uni_Homing : Enemy
{
    public float moveSpeed = 3f; // 움직이는 속도

    private Create_MoveDown create = null;
    private Move_Back move = null;
    private Attack_RoundHoming attack = null;

    private readonly string bulletCyan = typeof(Bullet_RoundCyan).ToString();
    private readonly string bulletRed = typeof(Bullet_RoundRed).ToString();
    private readonly string bulletBlue = typeof(Bullet_RoundBlue).ToString();

    private readonly WaitForSeconds ppFiveSecWait = new WaitForSeconds(0.05f);
    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds halfSecWait = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    private Vector2 shootDir = Vector2.zero;
    private float angle;

    private void Awake()
    {
        // 생성
        create = gameObject.AddComponent<Create_MoveDown>();
        create.moveY = 4f;
        create.moveDur = 1f;

        dicState[State.Create] = create;

        // 이동
        move = gameObject.AddComponent<Move_Back>();
        move.speed = moveSpeed;
        dicState[State.Move] = move;

        // 공격
        attack = gameObject.AddComponent<Attack_RoundHoming>();
        dicState[State.Attack] = attack;

        // 죽음
        dicState[State.Die] = gameObject.AddComponent<Die_GivePoint>();
    }

    protected override IEnumerator LifeTime()
    {
        yield return oneSecWait;

        for (int i = 0; i < 40; i++)
        {
            RoundHoming(3, Random.Range(7f, 8.5f), i * 15f, 0.7f, bulletRed, transform);
            RoundHoming(3, Random.Range(7f, 8.5f), i * -15f, 0.7f, bulletRed, transform);
            yield return ppFiveSecWait;
        }

        yield return oneSecWait;
        if (!currentState.Equals(State.Die))
            SetState(State.Move);
    }

    private void RoundHoming(int bulletCount, float bulletSpeed, float bulletTwist, float radius, string bulletType, Transform shootPos)
    {
        attack.SetValue(bulletCount, bulletSpeed, bulletTwist, radius, bulletType, shootPos);
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
