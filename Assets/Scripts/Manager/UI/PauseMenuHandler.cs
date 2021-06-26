using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenuHandler : MonoBehaviour
{
    [SerializeField]
    private Transform menuTrans = null;
    [SerializeField]
    private Text pauseText = null;
    [SerializeField]
    private Transform loadingImg = null;

    private Image myImage = null;
    private Text[] menus;

    private bool isMenuOpen = false;
    private int currentMenuIdx = 0;

    private readonly Color nonHighlighted_Color = new Color(193f / 255f, 193f / 255f, 193f / 255f);

    public bool canSelect = true;

    private void Start()
    {
        myImage = GetComponent<Image>();
        menus = menuTrans.GetComponentsInChildren<Text>();

        canSelect = false;

        GameManager.Instance.OnStageStart.AddListener(() => { canSelect = true; });
        GameManager.Instance.OnStageClear.AddListener(() => { canSelect = false; });
        GameManager.Instance.OnGameOver.AddListener(() => { canSelect = false; });
        GameManager.Instance.OnGameRestart.AddListener(() => { canSelect = false; });
        GameManager.Instance.OnGameClear.AddListener(() => { canSelect = false; });

        SetPauseMenu(false);
    }

    private void Update()
    {
        if (!canSelect) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOpen = !isMenuOpen;
            SetPauseMenu(isMenuOpen);
        }

        if (isMenuOpen) // �޴��� �����ִ� ���¸�
        {
            SelectMenu();
        }
    }

    private void SetPauseMenu(bool isOpen)
    {
        myImage.enabled = isOpen;
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].enabled = isOpen;
        }
        pauseText.enabled = isOpen;

        Time.timeScale = isOpen ? 0f : 1f;

        GameManager.Instance.OnGamePause.Invoke(!isOpen);

        if (isOpen)
        {
            currentMenuIdx = 0;
            HighlightMenu();
        }
    }

    private void SelectMenu()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && currentMenuIdx < (menus.Length - 1))
        {
            currentMenuIdx++;

            HighlightMenu();
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow) && currentMenuIdx > 0)
        {
            currentMenuIdx--;

            HighlightMenu();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentMenuIdx)
            {
                case 0:

                    isMenuOpen = false;
                    SetPauseMenu(isMenuOpen);

                    break;

                case 1:

                    Time.timeScale = 1f;

                    PoolManager.Instance.ResetPool();
                    GameManager.Instance.LoadMainScene();

                    break;

                case 2:

                    SetPauseMenu(false);
                    GameManager.Instance.OnGameRestart.Invoke();

                    loadingImg.DOLocalMoveY(0f, 1f)
                        .SetEase(Ease.OutQuart)
                        .OnComplete(() => {

                            PoolManager.Instance.ResetPool();
                            GameManager.Instance.LoadInGameScene();
                        });

                    break;
            }
        }
    }

    private void HighlightMenu()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].fontSize = 50;
            menus[i].color = nonHighlighted_Color;
        }

        menus[currentMenuIdx].fontSize = 60;
        menus[currentMenuIdx].color = Color.white;
    }
}
