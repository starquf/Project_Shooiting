using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RankingBoardHandler : MonoBehaviour
{
    [SerializeField]
    private MainUIHandler mainUIHandler = null;

    private bool is_Selected = false;

    [SerializeField]
    private List<RankUI> rankUIs = new List<RankUI>();

    private string playerType;
    private string score;
    private string stage;

    private Tweener moveTween = null; 

    private void Start()
    {
        for (int i = 0; i < GameManager.Instance.rankBoard.ranks.Count; i++)
        {
            if (i > 7) break;

            var rank = GameManager.Instance.rankBoard.ranks[i];

            switch (rank.playerType)
            {
                case PlayerType.Tech:
                    playerType = "Player Type A";
                    break;

                case PlayerType.Knife:
                    playerType = "Player Type B";
                    break;
            }

            if (rank.stage.Equals(99))
            {
                stage = "ALL Clear";
            }
            else
            {
                stage = $"Stage {rank.stage.ToString("00")}";
            }

            score = rank.score.ToString();

            rankUIs[i].SetRankUI(playerType, score, stage);
        }
    }

    private void Update()
    {
        if (!is_Selected) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopSelect();
            mainUIHandler.SetSelect(true);
        }
    }

    public void StartSelect()
    {
        moveTween.Kill();
        moveTween = transform.DOLocalMoveX(0f, 0.8f)
                    .SetEase(Ease.OutQuart);

        is_Selected = true;
    }

    public void StopSelect()
    {
        moveTween.Kill();
        moveTween = transform.DOLocalMoveX(1920f, 0.5f)
                    .SetEase(Ease.OutQuart);

        is_Selected = false;
    }
}
