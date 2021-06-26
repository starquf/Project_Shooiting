using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankUI : MonoBehaviour
{
    [SerializeField]
    private Text playerTypeText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text stageText;

    public void SetRankUI(string playerType, string score, string stage)
    {
        playerTypeText.text = playerType;
        scoreText.text = score;
        stageText.text = stage;
    }
}
