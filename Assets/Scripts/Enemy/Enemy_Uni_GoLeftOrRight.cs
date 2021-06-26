using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Uni_GoLeftOrRight : Enemy
{
    public float moveSpeed = 3f; // 움직이는 속도

    private Move_GoLeftOrRight move = null;
    private Attack_DelayedRound attack = null;

    private readonly string bulletBW = typeof(Bullet_RoundBlackWhite).ToString();
    private readonly string sharpBlue = typeof(Bullet_SharpBlue).ToString();

    private readonly WaitForSeconds ppOneSecWait = new WaitForSeconds(0.01f);
    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds halfSecWait = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    private void Awake()
    {
        // 생성
        dicState[State.Create] = gameObject.AddComponent<State_Empty>();

        // 이동
        move = gameObject.AddComponent<Move_GoLeftOrRight>();
        move.moveSpeed = moveSpeed;
        dicState[State.Move] = move;

        // 공격
        attack = gameObject.AddComponent<Attack_DelayedRound>();
        dicState[State.Attack] = attack;

        // 죽음
        dicState[State.Die] = gameObject.AddComponent<Die_GivePoint>();
    }

    protected override IEnumerator LifeTime()
    {
        SetState(State.Move);
        yield return oneSecWait;

        DelayedRound(15, 5f, Random.Range(0f, 180f), bulletBW, transform, Random.Range(0.05f, 0.06f));
        DelayedRound(10, 7f, 0f, sharpBlue, transform, Random.Range(0.02f, 0.04f));
        DelayedRound(10, 7f, 180f, sharpBlue, transform, Random.Range(0.02f, 0.04f));
    }

    private void DelayedRound(int bulletCount, float bulletSpeed, float bulletTwist, string bulletType, Transform shootPos, float delay)
    {
        attack.SetValue(bulletCount, bulletSpeed, bulletTwist, bulletType, shootPos, delay);
        dicState[State.Attack] = attack;
        PlayState(State.Attack);
    }
}
