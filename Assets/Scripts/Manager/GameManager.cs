using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

[System.Serializable]
public class OnPauseHandler : UnityEvent<bool>
{
    
}

public enum PlayerType
{
    Tech,
    Knife
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    Debug.LogError("게임매니져가 비어있습니다!!");
                }
            }

            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        GetRankingBoard();
        GetHighScore();
    }

    public Vector2 maxSize = Vector2.zero;
    public Vector2 minSize = Vector2.zero;

    public Vector2 mapCenter = Vector2.zero;

    [HideInInspector]
    public Transform playerPos;

    [HideInInspector]
    public UIHandler uiHandler;

    [Space(10f)]
    public int score = 0;
    public int highScore = 0;
    public int currentStage = 1;

    public bool isNewRecord = false;

    [HideInInspector]
    public UnityEvent OnStageClear;
    [HideInInspector]
    public UnityEvent OnStageEnd;
    [HideInInspector]
    public UnityEvent OnStageStart;

    [HideInInspector]
    public UnityEvent OnGameOver;
    [HideInInspector]
    public OnPauseHandler OnGamePause;
    [HideInInspector]
    public UnityEvent OnGameRestart;
    [HideInInspector]
    public UnityEvent OnGameClear;

    public PlayerType playerType = PlayerType.Tech;

    public RankBoard rankBoard;

    private void GetRankingBoard()
    {
        string path = Path.Combine(Application.persistentDataPath, "rankBoard.txt");

        rankBoard = new RankBoard();

        if (File.Exists(path))
        {
            LoadRankBoard();
        }
        else
        {
            SaveRankBoard();
        }
    }

    private void GetHighScore()
    {
        if (rankBoard.ranks.Count > 0)
        {
            highScore = rankBoard.ranks[0].score;
        }
    }

    public void LoadInGameScene()
    {
        DOTween.KillAll();
        ResetValue();

        SceneManager.LoadScene("InGame");
    }

    public void LoadMainScene()
    {
        DOTween.KillAll();
        ResetValue();

        SceneManager.LoadScene("Main");
    }

    private void ResetValue()
    {
        score = 0;
        currentStage = 1;
        isNewRecord = false;
    }

    #region JsonSaveAndLoad
    private void LoadRankBoard()
    {
        string path = Path.Combine(Application.persistentDataPath, "rankBoard.txt");
        string json = File.ReadAllText(path);

        rankBoard = JsonUtility.FromJson<RankBoard>(json);
    }

    private void SaveRankBoard()
    {
        string json = JsonUtility.ToJson(rankBoard, true);
        string path = Path.Combine(Application.persistentDataPath, "rankBoard.txt");

        File.WriteAllText(path, json);
    }
    #endregion

    public void AddCurrentRank()
    {
        Rank rank = new Rank(playerType, currentStage, score);

        rankBoard.ranks.Add(rank);
        rankBoard.ranks = rankBoard.ranks.OrderByDescending(x => x.score).ToList();

        if (rankBoard.ranks.Count > 8)
        {
            for (int i = rankBoard.ranks.Count - 1; i >= 8; i--)
            {
                rankBoard.ranks.RemoveAt(i);
            }
        }

        SaveRankBoard();
    }
}
