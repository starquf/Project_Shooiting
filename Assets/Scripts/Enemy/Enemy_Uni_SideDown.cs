using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Uni_SideDown : Enemy
{
    public float moveSpeed = 3f; // �����̴� �ӵ�

    private Create_MoveDown create = null;

    private Move_GoSideDown move = null;
    private Attack_SpreadBulletToPlayer attack = null;

    private readonly string bulletBW = typeof(Bullet_RoundBlackWhite).ToString();

    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds halfSecWait = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    private void Awake()
    {
        // ����
        create = gameObject.AddComponent<Create_MoveDown>();
        create.moveY = 5f;
        create.moveDur = 1f;
        dicState[State.Create] = create;


        // �̵�
        move = gameObject.AddComponent<Move_GoSideDown>();
        move.speed = moveSpeed;
        dicState[State.Move] = move;

        // ����
        attack = gameObject.AddComponent<Attack_SpreadBulletToPlayer>();
        dicState[State.Attack] = attack;

        // ����
        dicState[State.Die] = gameObject.AddComponent<Die_GivePoint>();
    }

    protected override IEnumerator LifeTime()
    {
        yield return oneSecWait;
        SetState(State.Move);

        yield return oneSecWait;

        SpreadBulletToPlayer(2, 5f, 30f, bulletBW, transform);
    }

    private void SpreadBulletToPlayer(int bulletCount, float bulletSpeed, float arc, string bulletType, Transform shootPos)
    {
        attack.SetValue(bulletCount, bulletSpeed, arc, bulletType, shootPos);
        dicState[State.Attack] = attack;
        PlayState(State.Attack);
    }
}
