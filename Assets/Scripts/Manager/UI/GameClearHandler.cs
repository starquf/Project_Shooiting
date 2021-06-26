using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameClearHandler : MonoBehaviour
{
    [Header("게임 클리어 텍스트들")]
    [SerializeField]
    private Transform texts;
    [SerializeField]
    private Text gameClearText;
    [SerializeField]
    private Text congraText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text newRecordText;
    [SerializeField]
    private Text pressEnterText;

    [SerializeField]
    private Image playerImage = null;

    private Image myImage = null;

    private Sequence gameClearSeq = null;

    private bool isOver = false;

    private readonly Color invisibleColor = new Color(255f / 255f, 226f / 255f, 107f / 255f, 0f);

    private void Start()
    {
        myImage = GetComponent<Image>();
        myImage.enabled = false;

        gameClearText.color = invisibleColor;
        congraText.color = invisibleColor;

        pressEnterText.enabled = false;
        playerImage.enabled = false;
        scoreText.enabled = false;
        newRecordText.enabled = false;

        GameManager.Instance.OnGameClear.AddListener(ShowGameClear);
    }

    private void Update()
    {
        if (isOver && Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Instance.LoadMainScene();
        }
    }

    private void ShowGameClear()
    {
        myImage.enabled = true;
        playerImage.enabled = true;
        myImage.DOFade(0f, 0.5f).From();

        gameClearSeq.Kill();

        gameClearSeq = DOTween.Sequence()
            .Append(transform.DOLocalMoveX(1000f, 1f).From(true))
            .Append(gameClearText.DOFade(1f, 1f))
            .Join(congraText.DOFade(1f, 1f))
            .AppendInterval(0.7f)
            .Join(texts.DOLocalMoveY(237f, 0.5f))
            .AppendCallback(() => 
            {
                scoreText.enabled = true;
                StartCoroutine(IncreaseScore());
            });
    }

    private IEnumerator IncreaseScore()
    {
        int score = 0;

        while (true)
        {
            score += 500;

            scoreText.text = $"Score : {score}";

            if (score >= GameManager.Instance.score || Input.GetKeyDown(KeyCode.Return))
            {
                score = GameManager.Instance.score;
                scoreText.text = $"Score : {score}";

                break;
            }

            yield return null;
        }

        if (GameManager.Instance.isNewRecord)
        {
            newRecordText.enabled = true;
            newRecordText.DOFade(0f, 0.5f).SetEase(Ease.Flash, 6, 0);
        }

        GameManager.Instance.currentStage = 99;
        GameManager.Instance.AddCurrentRank();
        yield return new WaitForSeconds(1f);

        pressEnterText.enabled = true;
        pressEnterText.DOFade(1f, 1f)
        .From(0f)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.Linear);

        isOver = true;
    }
}
