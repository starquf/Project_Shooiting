using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_LazerGuy_Hard : Enemy
{
    public float moveSpeed = 3f; // 움직이는 속도

    private Create_MoveDown create = null;

    private Move_Back move = null;

    private Attack_RoundBullet attack_RoundBullet = null;
    private Attack_SpreadBulletToPlayer attack_SpreadToPlayer = null;

    private readonly string roundRed = typeof(Bullet_RoundRed).ToString();
    private readonly string roundBlue = typeof(Bullet_RoundBlue).ToString();
    private readonly string sharpRed = typeof(Bullet_SharpRed).ToString();
    private readonly string sharpBlue = typeof(Bullet_SharpBlue).ToString();

    private readonly WaitForSeconds ppOneSecWait = new WaitForSeconds(0.01f);
    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds halfSecWait = new WaitForSeconds(0.5f);
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
        attack_RoundBullet = gameObject.AddComponent<Attack_RoundBullet>();
        attack_SpreadToPlayer = gameObject.AddComponent<Attack_SpreadBulletToPlayer>();
        dicState[State.Attack] = attack_SpreadToPlayer;

        // 죽음
        dicState[State.Die] = gameObject.AddComponent<Die_RemoveAndGiveBomb>();
    }

    protected override IEnumerator LifeTime()
    {
        for (int x = 0; x < 5; x++)
        {
            yield return oneSecWait;

            for (int i = 0; i < 10; i++)
            {
                RoundBullet(Random.Range(5, 10), Random.Range(3f, 5f), Random.Range(0f, 360f), roundRed, transform);
                yield return ppOneSecWait;
                RoundBullet(Random.Range(5, 10), Random.Range(3f, 5f), Random.Range(0f, 360f), roundBlue, transform);
                yield return ppOneSecWait;
            }

            yield return oneSecWait;

            float speed = 4f;

            for (int j = 0; j < 20; j++)
            {
                SpreadBulletToPlayer(1, speed, 0f, sharpRed, transform);
                speed += 0.5f;
            }

            yield return pOneSecWait;

            speed = 4f;

            for (int j = 0; j < 20; j++)
            {
                SpreadBulletToPlayer(1, speed, 0f, sharpBlue, transform);
                speed += 0.5f;
            }

            yield return pOneSecWait;
        }

        yield return oneSecWait;
        yield return oneSecWait;
        yield return oneSecWait;

        if (!currentState.Equals(State.Die))
            SetState(State.Move);
    }

    private void SpreadBulletToPlayer(int bulletCount, float bulletSpeed, float arc, string bulletType, Transform shootPos)
    {
        attack_SpreadToPlayer.SetValue(bulletCount, bulletSpeed, arc, bulletType, shootPos);
        dicState[State.Attack] = attack_SpreadToPlayer;
        PlayState(State.Attack);
    }

    private void RoundBullet(int bulletCount, float bulletSpeed, float bulletTwist, string bulletType, Transform shootPos)
    {
        attack_RoundBullet.SetValue(bulletCount, bulletSpeed, bulletTwist, bulletType, shootPos);
        dicState[State.Attack] = attack_RoundBullet;
        PlayState(State.Attack);
    }
}
