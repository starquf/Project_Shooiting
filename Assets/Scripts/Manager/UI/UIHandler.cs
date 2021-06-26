using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHandler : MonoBehaviour
{
    public Image hpBar = null;

    [Header("Boss Info")]
    [SerializeField]
    private float hpValue = 1f;
    private Coroutine increaseRoutine = null;
    [SerializeField]
    private Text timerText = null;

    [Header("Player Info")]
    [SerializeField]
    private List<Image> playerHpHolder = new List<Image>();
    [SerializeField]
    private List<Image> playerBombHolder = new List<Image>();
    [SerializeField]
    private Text scoreText = null;
    [SerializeField]
    private Text highScoreText = null;
    [SerializeField]
    private Text messageText = null;
    private Vector3 startPos = Vector3.zero;
    private Tweener hideMsgTween = null;
    private readonly Color msgColor = new Color(1f, 1f, 1f, 0.9f);

    [Header("페이드 배경")]
    [SerializeField]
    private SpriteRenderer fadeBG = null;
    private Tweener fadeTween = null;
    private bool isLocked = false;

    [SerializeField]
    private Transform loadingImg;

    void Start()
    {
        GameManager.Instance.uiHandler = this;
        SetHighScore(GameManager.Instance.highScore);

        hpBar.transform.parent.gameObject.SetActive(false);
        ShowOrHideTimer(false);

        startPos = messageText.transform.localPosition;
        messageText.color = new Color(1f, 1f, 1f, 0f);

        SetHide(false);
    }

    private void SetHide(bool isHide)
    {
        if (isHide)
        {
            loadingImg.DOLocalMoveY(0f, 1f);
        }
        else
        {
            loadingImg.DOLocalMoveY(1048f, 1f);
        }
    }

    #region SetSpell

    public void FadeIn()
    {
        if (isLocked) return;

        fadeTween.Kill();
        fadeTween = fadeBG.DOFade(0f, 1f);
    }

    public void FadeOut()
    {
        if (isLocked) return;

        fadeTween.Kill();
        fadeTween = fadeBG.DOFade(0.5f, 1f).SetEase(Ease.OutQuart);
    }

    public void LockFade(bool isLock)
    {
        isLocked = isLock;
    }
    #endregion

    #region SetPlayerInfo
    public void SetPlayerHp(int hpCount)
    {
        for (int i = 0; i < playerHpHolder.Count; i++)
        {
            if (i < hpCount)
            {
                playerHpHolder[i].enabled = true;
            }
            else
            {
                playerHpHolder[i].enabled = false;
            }
        }
    }

    public void SetPlayerBomb(int bombCount)
    {
        for (int i = 0; i < playerBombHolder.Count; i++)
        {
            if (i < bombCount)
            {
                playerBombHolder[i].enabled = true;
            }
            else
            {
                playerBombHolder[i].enabled = false;
            }
        }
    }

    public void AddScore(int score)
    {
        GameManager.Instance.score += score;

        scoreText.text = $"Score\n{GameManager.Instance.score}";

        if (GameManager.Instance.score > GameManager.Instance.highScore)
        {
            GameManager.Instance.isNewRecord = true;
            GameManager.Instance.highScore = GameManager.Instance.score;
            SetHighScore(GameManager.Instance.highScore);
        }
    }

    public void SetHighScore(int score)
    {
        highScoreText.text = $"High Score\n{score}";
    }

    public void ShowMessage(string msg, float waitTime = 1f, float moveTime = 0.3f, int fontSize = 50)
    {
        hideMsgTween.Kill();

        messageText.text = msg;
        messageText.fontSize = fontSize;

        messageText.transform.localPosition = startPos;
        messageText.color = msgColor;

        messageText.transform.DOLocalMoveY(-100, moveTime)
            .From(true);
        messageText.DOFade(0f, moveTime + 0.1f)
            .From();

        hideMsgTween = messageText.DOFade(0f, 0.3f).SetDelay(waitTime);
    }
    #endregion

    #region SetBossInfo
    public void ShowHpBar()
    {
        if (hpBar.transform.parent.gameObject.activeSelf) return;

        hpBar.transform.parent.gameObject.SetActive(true);

        hpValue = 0f;
        hpBar.fillAmount = 0f;

        increaseRoutine = StartCoroutine(IncreaseHpBar());
    }

    private IEnumerator IncreaseHpBar()
    {
        while (true)
        {
            if (hpBar.fillAmount < hpValue)
            {
                hpBar.fillAmount += Time.deltaTime / 3f;
            }
            else if (hpBar.fillAmount > hpValue)
            {
                hpBar.fillAmount -= Time.deltaTime / 3f;
            }

            if (Mathf.Abs(hpBar.fillAmount - hpValue) <= 0.02f)
            {
                hpBar.fillAmount = hpValue;
            }

            yield return null;
        }
    }

    public void StopHpBar()
    {
        hpBar.transform.parent.gameObject.SetActive(false);

        if (increaseRoutine != null)
        {
            StopCoroutine(increaseRoutine);
            increaseRoutine = null;
        }
    }

    public void SetHpBar(float hp)
    {
        hpValue = hp;
    }
    public void ShowOrHideTimer(bool isShow)
    {
        timerText.gameObject.SetActive(isShow);
    }

    public void SetTimerText(int time)
    {
        timerText.text = time.ToString();
    }
    #endregion
}
