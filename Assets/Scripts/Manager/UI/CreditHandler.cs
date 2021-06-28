using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreditHandler : MonoBehaviour
{
    [SerializeField]
    private MainUIHandler mainUIHandler = null;

    private bool can_Select = false;

    private Tweener moveTween = null;

    private void Update()
    {
        if (!can_Select) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopSelect();
            mainUIHandler.SetSelect(true);
        }
    }

    public void StartSelect()
    {
        moveTween.Kill();
        moveTween = transform.DOLocalMoveX(0f, 1f)
                    .SetEase(Ease.OutQuart);

        can_Select = true;
    }

    public void StopSelect()
    {
        moveTween.Kill();
        moveTween = transform.DOLocalMoveX(1920f, 0.5f)
                    .SetEase(Ease.OutQuart);

        can_Select = false;
    }
}
