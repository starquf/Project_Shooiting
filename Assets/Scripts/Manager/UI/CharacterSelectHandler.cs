using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterSelectHandler : MonoBehaviour
{
    [SerializeField]
    private MainUIHandler mainUIHandler = null;

    [SerializeField]
    private Transform selectTrans = null;
    private CanvasGroup[] cts = null;

    private bool can_Select = false;

    private int selectedIdx = 0;

    private bool isMove = false;

    private Tweener moveTween = null;
    private Tweener selectTween = null;

    private readonly Vector3 smallSize = new Vector3(0.5f, 0.5f, 1f);

    [SerializeField]
    private Transform selectText;
    [SerializeField]
    private Transform centerLoadingImg;

    private Sequence gameStartSeq;

    private void Start()
    {
        cts = selectTrans.GetComponentsInChildren<CanvasGroup>();
    }

    private void Update()
    {
        if (!can_Select) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopSelect();
            mainUIHandler.SetSelect(true);
        }

        MoveSelect();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }

    private void MoveSelect()
    {
        isMove = false;

        if (Input.GetKeyDown(KeyCode.DownArrow) && selectedIdx < cts.Length - 1)
        {
            selectedIdx++;
            isMove = true;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow) && selectedIdx > 0)
        {
            selectedIdx--;
            isMove = true;
        }

        if (isMove)
        {
            selectTween.Kill();
            selectTween = selectTrans.DOLocalMoveY(350f * selectedIdx, 0.5f)
                .SetEase(Ease.OutQuart);

            HighlightSelect();
        }
    }

    private void StartGame()
    {
        can_Select = false;

        switch (selectedIdx)
        {
            case 0:
                GameManager.Instance.playerType = PlayerType.Tech;
                break;

            case 1:
                GameManager.Instance.playerType = PlayerType.Knife;
                break;
        }

        for (int i = 0; i < cts.Length; i++)
        {
            cts[i].transform.DOLocalMoveX(i.Equals(selectedIdx) ? -771f : 920f, 1f)
                .SetEase(Ease.OutQuad);
        }

        cts[selectedIdx].DOFade(0f, 2f).SetEase(Ease.Linear);

        gameStartSeq.Kill();

        gameStartSeq = DOTween.Sequence()
            .Append(selectText.DOLocalMoveY(650f, 0.3f))
            .AppendInterval(1f)
            .Append(centerLoadingImg.DOLocalMoveY(0f, 1.5f))
            .AppendInterval(0.5f)
            .AppendCallback(() => { GameManager.Instance.LoadInGameScene(); });
        
    }

    private void HighlightSelect()
    {
        for (int i = 0; i < cts.Length; i++)
        {
            cts[i].DOFade(i.Equals(selectedIdx) ? 1f : 0.5f, 0.5f)
                .SetEase(Ease.OutQuart);

            cts[i].transform.DOScale(i.Equals(selectedIdx) ? Vector3.one : smallSize, 0.5f)
                .SetEase(Ease.OutQuart);
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
