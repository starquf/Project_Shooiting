using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Uni_DownAndRound : Enemy
{
    public float moveSpeed = 3f; // �����̴� �ӵ�

    private Die_SpreadBullet die = null;
    private Create_MoveDown create = null;
    private Move_Down move = null;
    private Attack_RoundBullet attack = null;

    private readonly string bulletBW = typeof(Bullet_RoundBlackWhite).ToString();
    private readonly string bulletRed = typeof(Bullet_RoundRed).ToString();

    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    private Vector2 shootDir = Vector2.zero;
    private float angle;

    private void Awake()
    {
        // ����
        create = gameObject.AddComponent<Create_MoveDown>();
        create.moveY = 4f;
        create.moveDur = 1f;

        dicState[State.Create] = create;

        // �̵�
        move = gameObject.AddComponent<Move_Down>();
        move.speed = moveSpeed;
        dicState[State.Move] = move;

        // ����
        attack = gameObject.AddComponent<Attack_RoundBullet>();
        dicState[State.Attack] = attack;

        // ����
        die = gameObject.AddComponent<Die_SpreadBullet>();
        die.bulletCount = 4;
        die.bulletSpeed = 3f;
        die.bulletType = bulletRed;
        dicState[State.Die] = die;
    }

    protected override IEnumerator LifeTime()
    {
        SetState(State.Move);

        while (true)
        {
            RoundBulletToPlayer(8, 3f, bulletBW, transform);

            yield return oneSecWait;
        }
    }

    private void RoundBulletToPlayer(int bulletCount, float bulletSpeed, string bulletType, Transform shootPos)
    {
        shootDir = GameManager.Instance.playerPos.position - transform.position;
        angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        attack.SetValue(bulletCount, bulletSpeed, angle, bulletType, shootPos);
        dicState[State.Attack] = attack;
        PlayState(State.Attack);
    }
}
