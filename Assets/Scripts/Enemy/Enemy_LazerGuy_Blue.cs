using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_LazerGuy_Blue : Enemy
{
    public float moveSpeed = 3f; // 움직이는 속도

    private Create_MoveDown create = null;
    private Move_GoSideDown move = null;
    private Attack_SpreadBulletToPlayer attack = null;

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
        move = gameObject.AddComponent<Move_GoSideDown>();
        move.speed = moveSpeed;
        dicState[State.Move] = move;

        // 공격
        attack = gameObject.AddComponent<Attack_SpreadBulletToPlayer>();
        dicState[State.Attack] = attack;

        // 죽음
        dicState[State.Die] = gameObject.AddComponent<Die_GivePoint>();
    }

    protected override IEnumerator LifeTime()
    {
        yield return halfSecWait;
        SetState(State.Move);

        yield return halfSecWait;
        for (int i = 0; i < 2; i++)
        {
            float speed = 4f;

            for (int j = 0; j < 20; j++)
            {
                SpreadBulletToPlayer(1, speed, 0f, sharpBlue, transform);
                speed += 0.5f;
            }

            yield return pOneSecWait;
        }

    }

    private void SpreadBulletToPlayer(int bulletCount, float bulletSpeed, float arc, string bulletType, Transform shootPos)
    {
        attack.SetValue(bulletCount, bulletSpeed, arc, bulletType, shootPos);
        dicState[State.Attack] = attack;
        PlayState(State.Attack);
    }
}
