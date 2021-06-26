using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageClearHandler : MonoBehaviour
{
    [Header("스테이지 클리어 텍스트들")]
    [SerializeField]
    private Text stageClearText;

    [SerializeField]
    private Text pressEnterText;

    [SerializeField]
    private Text loadingText;

    [SerializeField]
    private Image playerImage = null;

    private Image myImage = null;

    private Sequence stageClearSeq = null;

    private bool isOver = false;

    private readonly Color invisibleColor = new Color(255f / 255f, 226f / 255f, 107f / 255f, 0f);
    private readonly WaitForSeconds halfSecWait = new WaitForSeconds(0.5f);

    private void Start()
    {
        myImage = GetComponent<Image>();
        myImage.enabled = false;

        stageClearText.color = invisibleColor;

        pressEnterText.enabled = false;
        loadingText.enabled = false;
        playerImage.enabled = false;

        GameManager.Instance.OnStageClear.AddListener(ShowStageClear);
    }

    private void Update()
    {
        if (isOver && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(LoadNextStage());
        }
    }

    private void ShowStageClear()
    {
        myImage.enabled = true;
        playerImage.enabled = true;
        myImage.DOFade(0f, 0.5f).From();

        stageClearSeq.Kill();

        stageClearSeq = DOTween.Sequence()
        .Append(transform.DOLocalMoveX(1000f, 1f).From(true))
        .Append(stageClearText.DOFade(1f, 1f))
        .AppendCallback(() =>
        {
            pressEnterText.enabled = true;
            pressEnterText.DOFade(1f, 1f)
            .From(0f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);

            isOver = true;
        });
    }

    private IEnumerator LoadNextStage()
    {
        isOver = false;
        loadingText.enabled = true;

        yield return halfSecWait;

        GameManager.Instance.OnStageEnd.Invoke();

        Vector3 startPos = transform.localPosition;

        transform.DOLocalMoveX(1000f, 0.75f)
            .OnComplete(() => {
                transform.localPosition = startPos;
                myImage.enabled = false;
                stageClearText.color = invisibleColor;
                pressEnterText.enabled = false;
                loadingText.enabled = false;
                playerImage.enabled = false;
            });
    }
}
