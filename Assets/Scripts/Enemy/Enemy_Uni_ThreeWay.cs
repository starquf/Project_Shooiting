using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Uni_ThreeWay : Enemy
{
    public float moveSpeed = 3f; // 움직이는 속도

    private Create_MoveDown create = null;
    private Move_Back move = null;
    private Attack_RoundBullet attack = null;

    private readonly string bulletCyan = typeof(Bullet_RoundCyan).ToString();
    private readonly string bulletRed = typeof(Bullet_RoundRed).ToString();

    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
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
        attack = gameObject.AddComponent<Attack_RoundBullet>();
        dicState[State.Attack] = attack;

        // 죽음
        dicState[State.Die] = gameObject.AddComponent<Die_GiveBigUpgrade>();
    }

    protected override IEnumerator LifeTime()
    {
        yield return oneSecWait;

        for (int i = 0; i < 3; i++)
        {
            RoundBulletToPlayer(25, 4f, bulletRed, transform);
            RoundBulletToPlayer(3, 7f, bulletCyan, transform);
            RoundBulletToPlayer(3, 7.5f, bulletCyan, transform);
            RoundBulletToPlayer(3, 8f, bulletCyan, transform);
            RoundBulletToPlayer(3, 8.5f, bulletCyan, transform);
            yield return oneSecWait;
        }

        yield return oneSecWait;
        yield return oneSecWait;
        if (!currentState.Equals(State.Die))
            SetState(State.Move);
    }

    private void RoundBulletToPlayer(int bulletCount, float bulletSpeed, string bulletType, Transform shootPos)
    {
        shootDir = GameManager.Instance.playerPos.position - transform.position;
        angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        attack.SetValue(bulletCount, bulletSpeed, angle, bulletType, shootPos);
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
