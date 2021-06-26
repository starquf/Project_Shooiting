using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Move_SetPosition : MonoBehaviour, IState
{
    private Vector2 rangeMax = new Vector2(2f, 5f);
    private Vector2 rangeMin = new Vector2(-2f, 6f);

    public float moveDur = 2f;
    public Vector2 movePos = Vector2.zero;
    public bool moveRandom = false;

    Tweener moveTween = null;

    private void Start()
    {
        rangeMin = GameManager.Instance.minSize;
        rangeMax = GameManager.Instance.maxSize;
    }

    public void OnEnter()
    {
        Vector2 target = Vector2.zero;

        if (moveRandom)
        {
            target = new Vector2(Random.Range(rangeMin.x, rangeMax.x), Random.Range(rangeMin.y, rangeMax.y));
            moveTween = transform.DOMove(target, moveDur)
                .SetEase(Ease.OutCubic);
        }

        else
        {
            target = movePos;
            moveTween = transform.DOMove(target, moveDur)
                .SetEase(Ease.OutCubic);
        }
    }

    public void OnEnd()
    {
        moveTween.Kill();
    }
}
