using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField]
    private List<MenuScript> menus = new List<MenuScript>();
    [SerializeField]
    private GameObject selectMenu = null;


    private bool canSelect = false;
    private int currentMenu = 0;

    [Space(10f)]
    [SerializeField]
    private Image title = null;
    [SerializeField]
    private Text subTitle = null;
    [SerializeField]
    private Image fadePanel = null;


    [Header("핸들러들")]
    [SerializeField]
    private CharacterSelectHandler charSelectHandler = null;
    [SerializeField]
    private RankingBoardHandler rankingHandler = null;
    [SerializeField]
    private SettingHandler settingHandler = null;

    [Space(10f)]
    [SerializeField]
    private GameObject mainMusic;

    private void Start()
    {
        StartCoroutine(Production());
    }

    private IEnumerator Production()
    {
        yield return null;
        title.transform.DOLocalMoveY(-200, 1.5f)
            .From(true);
        title.DOFade(0f, 1f)
            .From();
        

        subTitle.transform.DOLocalMoveY(-300, 1.5f)
            .From(true)
            .SetDelay(0.5f).OnComplete(() => {
                fadePanel.color = Color.white;
                fadePanel.DOFade(0f, 3f);
            });

        subTitle.DOFade(0f, 1f)
            .From()
            .SetDelay(1f);

        selectMenu.transform.DOLocalMoveX(900, 2f)
            .From(true)
            .SetDelay(1.5f)
            .SetEase(Ease.OutQuart);

        yield return new WaitForSeconds(2.5f);

        menus[currentMenu].Select(true);
        canSelect = true;

        mainMusic.SetActive(true);
    }

    private void Update()
    {
        if (!canSelect) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpOrDownMenu(true);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            UpOrDownMenu(false);
        }

        else if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectMenu();
        }
    }

    private void UpOrDownMenu(bool isUp)
    {
        bool isChange = false;

        if (isUp && currentMenu > 0)
        {
            currentMenu--;

            isChange = true;
        }
        else if (!isUp && currentMenu < (menus.Count - 1))
        {
            currentMenu++;

            isChange = true;
        }

        if (isChange)
        {
            foreach (var menu in menus)
            {
                menu.Select(false);
            }

            menus[currentMenu].Select(true);
        }
    }

    private void SelectMenu()
    {
        switch (currentMenu)
        {
            case 0:

                canSelect = false;
                charSelectHandler.StartSelect();
                break;

            case 1:

                canSelect = false;
                settingHandler.StartSelect();
                break;

            case 2:

                canSelect = false;
                rankingHandler.StartSelect();
                break;
        }
    }

    public void SetSelect(bool value)
    {
        canSelect = value;
    }
}
