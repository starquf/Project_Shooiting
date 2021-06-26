using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Create_MoveDown : MonoBehaviour, IState
{
    public float moveDur = 0.5f;
    public float moveY = 3f;

    Tweener moveTween = null;

    public void OnEnter()
    {
        moveTween = transform.DOMoveY(transform.position.y - moveY, moveDur).SetEase(Ease.OutQuint);
    }

    public void OnEnd()
    {
        moveTween.Kill();
    }
}
