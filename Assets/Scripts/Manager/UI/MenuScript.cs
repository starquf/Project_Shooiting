using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuScript : MonoBehaviour
{
    private float startPosX = 0f;
    private Tweener moveTweener = null;

    private void Start()
    {
        startPosX = transform.localPosition.x;
    }

    public void Select(bool isSelected)
    {
        if (isSelected)
        {
            moveTweener.Kill();
            moveTweener = transform.DOLocalMoveX(startPosX - 100f, 0.5f);
        }
        else
        {
            moveTweener.Kill();
            moveTweener = transform.DOLocalMoveX(startPosX, 0.5f);
        }
    }
}
