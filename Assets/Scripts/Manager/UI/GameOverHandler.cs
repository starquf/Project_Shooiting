using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverHandler : MonoBehaviour
{
    [Header("게임오버 텍스트들")]
    [SerializeField]
    private Text gameOverText;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text newRecordText;

    [SerializeField]
    private Text pressEnterText;

    private Image myImage = null;

    private Sequence gameOverSeq = null;

    private bool isOver = false;

    private readonly Color invisibleColor = new Color(1f, 1f, 1f, 0f);

    private void Start()
    {
        myImage = GetComponent<Image>();
        myImage.enabled = false;

        gameOverText.color = invisibleColor;

        scoreText.enabled = false;
        pressEnterText.enabled = false;
        newRecordText.enabled = false;

        GameManager.Instance.OnGameOver.AddListener(ShowGameOver);
    }

    private void Update()
    {
        if (isOver && Input.GetKeyDown(KeyCode.Return))
        {
            isOver = false;
            PoolManager.Instance.ResetPool();
            GameManager.Instance.LoadMainScene();
        }
    }

    private void ShowGameOver()
    {
        myImage.enabled = true;
        myImage.DOFade(0f, 0.5f).From();

        gameOverSeq.Kill();

        gameOverSeq = DOTween.Sequence()
            .Append(transform.DOLocalMoveY(1000f, 1.5f).From(true).SetEase(Ease.OutBounce))
            .AppendCallback(() => 
            {
                PoolManager.Instance.ResetPool();
            })
            .Append(gameOverText.DOFade(1f, 2f))
            .AppendInterval(0.7f)
            .Join(gameOverText.transform.DOLocalMoveY(220f, 0.5f))
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

        GameManager.Instance.AddCurrentRank();
        yield return new WaitForSeconds(1f);

        pressEnterText.enabled = true;
        isOver = true;

        pressEnterText.DOFade(1f, 1f)
                .From(0f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
    }
}
