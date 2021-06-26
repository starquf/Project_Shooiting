using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum BulletState
{
    Stop,
    MoveForward,
    RotateAround
}

public abstract class Bullet : MonoBehaviour, IDamage
{
    private BulletState currentState = BulletState.MoveForward;

    public float Damage { get; set; }
    public float bulletDamage = 1f;
    

    public float bulletSpeed = 30f;
    private float currSpeed = 0f;

    // Rotate Settings
    private Transform center = null;
    private float angle = 30f;

    // 적의 총알인가?
    public bool is_EnemyBullet = true;

    private readonly string autoScore = typeof(Point_AutoScore).ToString();

    private Tweener rotateTween = null;

    protected void Start()
    {
        Damage = bulletDamage;
    }

    protected void OnEnable()
    {
        if (is_EnemyBullet)
        {
            PoolManager.Instance.OnUseSpell.AddListener(UseSpell);
        }

        currSpeed = bulletSpeed;
    }

    protected void OnDisable()
    {
        if (is_EnemyBullet && PoolManager.Instance.OnUseSpell != null)
        {
            PoolManager.Instance.OnUseSpell.RemoveListener(UseSpell);
        }

        ChangeState(BulletState.MoveForward);
        rotateTween.Kill();
        center = null;
    }

    protected void Update()
    {
        CheckState();
        CheckTransform();
    }

    protected virtual void BulletMove()
    {
        transform.Translate(Vector3.up * currSpeed * Time.deltaTime);
    }

    protected virtual void RotateAroundTarget()
    {
        if (center == null) return;
        transform.RotateAround(center.position, Vector3.forward, angle * Time.deltaTime);
        transform.position += (transform.position - center.position).normalized * currSpeed * Time.deltaTime;
    }

    protected virtual void CheckTransform()     // 화면 밖으로 나갔는가?
    {
        if (transform.position.x > GameManager.Instance.maxSize.x + 5f || transform.position.x < GameManager.Instance.minSize.x - 5f)
        {
            SetDisable();
        }

        if (transform.position.y > GameManager.Instance.maxSize.y + 2f || transform.position.y < GameManager.Instance.minSize.y - 2f)
        {
            SetDisable();
        }
    }

    protected virtual void ChangeState(BulletState state)
    {
        currentState = state;
    }

    protected virtual void CheckState()
    {
        switch (currentState)
        {
            case BulletState.MoveForward:
                BulletMove();
                break;

            case BulletState.RotateAround:
                RotateAroundTarget();
                break;

            case BulletState.Stop:

                break;
        }
    }

    #region ChangeDirections
    public virtual void ChangeDir(Vector3 dir)      // dir방향으로 회전
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public virtual void ChangeDir(Vector3 dir, float t)     // 시간차로 dir방향으로 회전
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        StartCoroutine(Timer(() => transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward), t));
    }

    public virtual void ChangeDirToPlayer()     // 플레이어 방향으로 회전
    {
        Vector3 dir = GameManager.Instance.playerPos.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public virtual void ChangeDirToPlayer(float t)     // 시간차로 플레이어 방향으로 회전
    {
        StartCoroutine(Timer(() => 
        {
            Vector3 dir = GameManager.Instance.playerPos.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        , t));
    }

    public virtual void RotateAngle(float angle)    // angle만큼 z축 회전
    {
        transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + angle, Vector3.forward);
    }

    public virtual void RotateAngle(float angle, float t)   // 시간차로 angle만큼 z축 회전
    {
        StartCoroutine(Timer(() => transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + angle, Vector3.forward), t));
    }

    public virtual void RotateAngleInTime(float angle, float t, float rotateTime)   // 시간차로 angle만큼 z축 회전
    {
        StartCoroutine(Timer(() =>
        rotateTween = transform.DORotateQuaternion(Quaternion.AngleAxis(transform.rotation.eulerAngles.z + angle, Vector3.forward), rotateTime)
        .SetEase(Ease.Linear), t));
    }

    public virtual void ChangeSpeed(float value)
    {
        currSpeed = value;
    }

    public virtual void ChangeSpeed(float value, float t)
    {
        StartCoroutine(Timer(() => currSpeed = value, t));
    }

    public virtual void PlusSpeed(float value)
    {
        currSpeed += value;
    }

    public virtual void PlusSpeed(float value, float t)
    {
        StartCoroutine(Timer(() => currSpeed += value, t));
    }

    public virtual void RotateAround(Transform tar, float angle, float t)
    {
        center = tar;
        this.angle = angle;
        StartCoroutine(Timer(() => ChangeState(BulletState.RotateAround), t));
    }

    public virtual void StopRotateAround(float t)
    {
        StartCoroutine(Timer(() => ChangeState(BulletState.RotateAround), t));
    }
    #endregion

    protected IEnumerator Timer(Action action, float t)
    {
        yield return new WaitForSeconds(t);
        action.Invoke();
    }

    protected void UseSpell()
    {
        // 여기다 점수로 바꾸는 함수 추가
        Point point = PoolManager.Instance.GetQueue(PoolType.Point, autoScore).GetComponent<Point>();
        point.transform.position = transform.position;
        point.is_Auto = true;

        SetDisable();
    }

    public void SetDisable()
    {
        gameObject.SetActive(false);
    }
}
