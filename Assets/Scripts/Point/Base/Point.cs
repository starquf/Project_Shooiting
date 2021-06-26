using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointType
{
    Upgrade,
    Score,
    AutoScore,
    BigUpgrade,
    LifeUp,
    SpellUp
}

public abstract class Point : MonoBehaviour
{
    public PointType pointType;

    public float speed = -3f;
    private float currSpeed = 0f;

    public bool is_Auto = false;

    private Vector3 dir = Vector3.down;

    private void OnEnable()
    {
        currSpeed = speed;
        is_Auto = false;
    }

    private void Update()
    {
        Move();
        CheckTransform();
    }

    public void SetSpeed(float speed)
    {
        currSpeed = speed;
    }

    private void Move()
    {
        if (is_Auto)
        {
            dir = GameManager.Instance.playerPos.position - transform.position;

            transform.position += dir.normalized * currSpeed * Time.deltaTime;
        }
        else
        {
            dir = Vector3.down;

            transform.position += dir * currSpeed * Time.deltaTime;

            if (currSpeed < 6f)
            {
                currSpeed += 6f * Time.deltaTime;
            }
        }
    }

    public void ChangeDirToPlayer()
    {
        is_Auto = true;
    }

    protected virtual void CheckTransform()     // 화면 밖으로 나갔는가?
    {
        if (transform.position.x > GameManager.Instance.maxSize.x + 5f || transform.position.x < GameManager.Instance.minSize.x - 5f)
        {
            SetDisable();
        }

        if (transform.position.y > GameManager.Instance.maxSize.y + 10f || transform.position.y < GameManager.Instance.minSize.y - 10f)
        {
            SetDisable();
        }
    }

    public virtual void SetDisable()
    {
        is_Auto = false;
        gameObject.SetActive(false);
    }
}
