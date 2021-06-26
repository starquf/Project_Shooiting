using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    public Transform spawnPoint = null;

    #region Wait ReadOnlys
    private readonly WaitForSeconds ppOneSecWait = new WaitForSeconds(0.01f);
    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds pThreeSecWait = new WaitForSeconds(0.3f);
    private readonly WaitForSeconds halfSecWait = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);
    private readonly WaitForSeconds threeSecWait = new WaitForSeconds(3f);
    private readonly WaitForSeconds fiveSecWait = new WaitForSeconds(5f);
    #endregion
    #region Enemy ReadOnlys
    private Enemy currEnemy = null;

    // 잡몹
    private readonly string enemy_Uni = typeof(Enemy_Uni).ToString();
    private readonly string enemy_Uni_SideDown = typeof(Enemy_Uni_SideDown).ToString();
    private readonly string enemy_Uni_Spread = typeof(Enemy_Uni_Spread).ToString();
    private readonly string enemy_Uni_ThreeWay = typeof(Enemy_Uni_ThreeWay).ToString();
    private readonly string enemy_Uni_GoLeftOrRight = typeof(Enemy_Uni_GoLeftOrRight).ToString();
    private readonly string enemy_Uni_Homing = typeof(Enemy_Uni_Homing).ToString();
    private readonly string enemy_Uni_RoundSpread = typeof(Enemy_Uni_RoundSpread).ToString();
    private readonly string enemy_Uni_DownAndRound = typeof(Enemy_Uni_DownAndRound).ToString();

    private readonly string enemy_TrashPlant = typeof(Enemy_TrashPlant).ToString();

    private readonly string enemy_LazerGuy = typeof(Enemy_LazerGuy).ToString();
    private readonly string enemy_LazerGuy_Blue = typeof(Enemy_LazerGuy_Blue).ToString();
    private readonly string enemy_LazerGuy_Hard = typeof(Enemy_LazerGuy_Hard).ToString();

    // 중보스
    private readonly string enemy_Jumper = typeof(Enemy_Jumper).ToString();
    private readonly string enemy_Tank = typeof(Enemy_Tank).ToString();

    // 보스
    private readonly string enemy_Golem = typeof(Enemy_Golem).ToString();
    private readonly string enemy_TechOne = typeof(Enemy_TechOne).ToString();
    #endregion

    private int currentStage = 0;

    private List<IEnumerator> stageList = new List<IEnumerator>();
    private Coroutine stageRoutine = null;

    private void Start()
    {
        stageList.Add(Stage01());
        stageList.Add(Stage02());

        currentStage = 0;
        stageRoutine = StartCoroutine(stageList[currentStage]);

        GameManager.Instance.OnGameOver.AddListener(GameOver);
        GameManager.Instance.OnGameRestart.AddListener(GameOver);
        GameManager.Instance.OnStageEnd.AddListener(MoveNextStage);
    }

    private IEnumerator TestTestStage()
    {
        yield return threeSecWait;
        GameManager.Instance.OnStageStart.Invoke();

        SpawnEnemy(enemy_Uni_SideDown, new Vector2(5f, 0f));
        yield return pOneSecWait;

        yield return threeSecWait;
        yield return threeSecWait;

        GameManager.Instance.OnGameClear.Invoke();
    }

    private IEnumerator Stage01()
    {
        yield return oneSecWait;
        yield return oneSecWait;
        GameManager.Instance.OnStageStart.Invoke();

        GameManager.Instance.uiHandler.ShowMessage("Stage 1", 2f, 1f, 70);

        yield return oneSecWait;
        yield return oneSecWait;

        GameManager.Instance.OnStageStart.Invoke();

        SpawnEnemy(enemy_Uni_SideDown, new Vector2(5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(4f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(4f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(4f, 0f));

        yield return threeSecWait;

        SpawnEnemy(enemy_Uni_SideDown, new Vector2(-5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(-4f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(-5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(-4f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(-5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(-4f, 0f));

        yield return threeSecWait;

        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));

        yield return threeSecWait;

        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));

        yield return halfSecWait;

        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_SideDown, new Vector2(UnityEngine.Random.Range(-5f, 5f), 0f));

        yield return threeSecWait;

        SpawnEnemy(enemy_TrashPlant, new Vector2(7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(7f, -5f));

        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));

        yield return threeSecWait;

        SpawnEnemy(enemy_Uni, new Vector2(4f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-4f, 0f));

        yield return threeSecWait;

        SpawnEnemy(enemy_Uni, new Vector2(4f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-4f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(2f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-2f, 0f));

        yield return threeSecWait;

        SpawnEnemy(enemy_Uni, new Vector2(4f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(2f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(0f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-2f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-4f, 0f));

        yield return oneSecWait;

        SpawnEnemy(enemy_Uni, new Vector2(5f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(3f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(1f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-3f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-5f, 0f));

        yield return oneSecWait;

        SpawnEnemy(enemy_Uni, new Vector2(4f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(2f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(0f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-2f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-4f, 0f));

        yield return oneSecWait;

        SpawnEnemy(enemy_Uni, new Vector2(4f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(2f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(0f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-2f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-4f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(5f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(3f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(1f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-3f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-5f, 0f));


        yield return threeSecWait;

        SpawnEnemy(enemy_Uni, new Vector2(-4f, UnityEngine.Random.Range(-1f, 0f)));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-3.5f, UnityEngine.Random.Range(-1f, 0f)));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-3f, UnityEngine.Random.Range(-1f, 0f)));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-2.5f, UnityEngine.Random.Range(-1f, 0f)));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-2f, UnityEngine.Random.Range(-1f, 0f)));
        yield return ppOneSecWait;

        SpawnEnemy(enemy_Uni, new Vector2(4f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(2f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-2f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-4f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni_Spread, new Vector2(0f, 0f));

        SpawnEnemy(enemy_TrashPlant, new Vector2(7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(7f, -5f));

        yield return oneSecWait;
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni, new Vector2(4f, UnityEngine.Random.Range(-1f, 0f)));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(3.5f, UnityEngine.Random.Range(-1f, 0f)));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(3f, UnityEngine.Random.Range(-1f, 0f)));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(2.5f, UnityEngine.Random.Range(-1f, 0f)));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(2f, UnityEngine.Random.Range(-1f, 0f)));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni_Spread, new Vector2(0f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(4f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(2f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-2f, 0f));
        yield return ppOneSecWait;
        SpawnEnemy(enemy_Uni, new Vector2(-4f, 0f));

        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));

        yield return threeSecWait;

        currEnemy = SpawnEnemy(enemy_Jumper, new Vector2(-8f, 0f));
        yield return new WaitUntil(() => currEnemy.is_Die);

        yield return oneSecWait;
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-5f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-4f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-3f, 0f));
        yield return halfSecWait;

        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(-7f, -5f));

        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-2f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-1f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(0f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(1f, 0f));
        yield return halfSecWait;

        SpawnEnemy(enemy_TrashPlant, new Vector2(7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(7f, -5f));
        yield return pThreeSecWait;
        SpawnEnemy(enemy_TrashPlant, new Vector2(7f, -5f));

        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(2f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(3f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(4f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(5f, 0f));
        yield return halfSecWait;

        yield return threeSecWait;

        SpawnEnemy(enemy_Uni_ThreeWay, new Vector2(4f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_Uni_ThreeWay, new Vector2(-4f, 0f));

        yield return threeSecWait;

        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_ThreeWay, new Vector2(0f, 0f));

        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-5f, 0f));
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_ThreeWay, new Vector2(0f, 0f));

        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(5f, 0f));
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_ThreeWay, new Vector2(0f, 0f));

        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(5f, 0f));
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_ThreeWay, new Vector2(0f, 0f));

        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(-5f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_DownAndRound, new Vector2(5f, 0f));

        yield return threeSecWait;
        yield return threeSecWait;

        PoolManager.Instance.OnUseSpell.Invoke();

        currEnemy = SpawnEnemy(enemy_Golem, new Vector2(-8f, 0f));
        yield return new WaitUntil(() => currEnemy.is_Die);

        yield return threeSecWait;
        yield return oneSecWait;

        GameManager.Instance.OnStageClear.Invoke();
    }


    private IEnumerator Stage02()
    {
        yield return oneSecWait;
        yield return oneSecWait;
        GameManager.Instance.OnStageStart.Invoke();


        GameManager.Instance.uiHandler.ShowMessage("Stage 2", 2f, 1f, 70);

        yield return oneSecWait;
        yield return oneSecWait;

        /*
        SpawnEnemy(enemy_LazerGuy, new Vector2(5f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-5f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(4f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-4f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(3f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-3f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(2f, 0f));
        yield return halfSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-2f, 0f));
        */
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, -5f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, -5f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, -5f));

        yield return threeSecWait;

        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, -5f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, -5f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, -5f));

        yield return threeSecWait;

        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, -5f));
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, -5f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, -5f));
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, -5f));
        yield return halfSecWait;
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, -5f));
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, -5f));

        yield return threeSecWait;
        yield return oneSecWait;
        yield return oneSecWait;

        SpawnEnemy(enemy_LazerGuy, new Vector2(3f, 0f));
        yield return oneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-3f, 0f));
        yield return oneSecWait;

        yield return threeSecWait;

        for (int i = 0; i < 3; i++)
        {
            SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, UnityEngine.Random.Range(-3f, -6f)));
            yield return oneSecWait;
            SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, UnityEngine.Random.Range(-3f, -6f)));
            yield return oneSecWait;
        }

        SpawnEnemy(enemy_LazerGuy, new Vector2(0f, 0f));
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, UnityEngine.Random.Range(-3f, -6f)));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, UnityEngine.Random.Range(-3f, -6f)));
        yield return oneSecWait;

        for (int i = 0; i < 3; i++)
        {
            SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, UnityEngine.Random.Range(-3f, -6f)));
            yield return oneSecWait;
            SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, UnityEngine.Random.Range(-3f, -6f)));
            yield return oneSecWait;
        }

        SpawnEnemy(enemy_LazerGuy, new Vector2(3f, 0f));
        SpawnEnemy(enemy_LazerGuy, new Vector2(-3f, 0f));
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, UnityEngine.Random.Range(-3f, -6f)));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, UnityEngine.Random.Range(-3f, -6f)));
        yield return oneSecWait;

        for (int i = 0; i < 3; i++)
        {
            SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, UnityEngine.Random.Range(-3f, -6f)));
            yield return oneSecWait;
            SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, UnityEngine.Random.Range(-3f, -6f)));
            yield return oneSecWait;
        }

        SpawnEnemy(enemy_LazerGuy, new Vector2(5f, 0f));
        SpawnEnemy(enemy_LazerGuy, new Vector2(-5f, 0f));
        SpawnEnemy(enemy_LazerGuy, new Vector2(3f, 0f));
        SpawnEnemy(enemy_LazerGuy, new Vector2(-3f, 0f));
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(7f, UnityEngine.Random.Range(-3f, -6f)));
        yield return oneSecWait;
        SpawnEnemy(enemy_Uni_GoLeftOrRight, new Vector2(-7f, UnityEngine.Random.Range(-3f, -6f)));
        yield return oneSecWait;

        yield return threeSecWait;

        SpawnEnemy(enemy_LazerGuy, new Vector2(5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(4f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-4f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(3f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-3f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(2f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-2f, 0f));

        yield return oneSecWait;
        yield return halfSecWait;

        SpawnEnemy(enemy_LazerGuy, new Vector2(5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(4f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-4f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(3f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-3f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(2f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-2f, 0f));

        yield return oneSecWait;
        yield return halfSecWait;

        SpawnEnemy(enemy_LazerGuy, new Vector2(5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-5f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(4f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-4f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(3f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-3f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(2f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-2f, 0f));

        yield return threeSecWait;

        SpawnEnemy(enemy_LazerGuy_Hard, new Vector2(0f, 0f));

        yield return threeSecWait;

        SpawnEnemy(enemy_LazerGuy, new Vector2(2f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-2f, 0f));

        yield return threeSecWait;

        SpawnEnemy(enemy_LazerGuy, new Vector2(2f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-2f, 0f));

        yield return threeSecWait;

        SpawnEnemy(enemy_LazerGuy, new Vector2(2f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-2f, 0f));

        yield return threeSecWait;

        SpawnEnemy(enemy_LazerGuy, new Vector2(2f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-2f, 0f));

        yield return threeSecWait;

        SpawnEnemy(enemy_LazerGuy, new Vector2(2f, 0f));
        yield return pOneSecWait;
        SpawnEnemy(enemy_LazerGuy, new Vector2(-2f, 0f));

        yield return threeSecWait;

        currEnemy = SpawnEnemy(enemy_Tank, new Vector2(-8f, 0f));
        yield return new WaitUntil(() => currEnemy.is_Die);

        yield return threeSecWait;

        SpawnEnemy(enemy_Uni_Homing, new Vector2(0f, 0f));

        yield return threeSecWait;
        yield return oneSecWait;
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_Homing, new Vector2(0f, 0f));

        yield return oneSecWait;
        SpawnEnemy(enemy_LazerGuy_Blue, new Vector2(4f, 1f));
        SpawnEnemy(enemy_LazerGuy_Blue, new Vector2(-4f, 1f));

        yield return threeSecWait;
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_Homing, new Vector2(4f, 0f));
        SpawnEnemy(enemy_Uni_Homing, new Vector2(-4f, 0f));

        yield return oneSecWait;
        SpawnEnemy(enemy_LazerGuy_Blue, new Vector2(1f, 1f));
        SpawnEnemy(enemy_LazerGuy_Blue, new Vector2(-1f, 1f));

        yield return threeSecWait;
        yield return oneSecWait;
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_RoundSpread, new Vector2(0f, 0f));

        yield return threeSecWait;
        yield return oneSecWait;
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_RoundSpread, new Vector2(4f, 0f));
        SpawnEnemy(enemy_Uni_RoundSpread, new Vector2(-4f, 0f));

        yield return threeSecWait;
        yield return oneSecWait;
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_Homing, new Vector2(4f, 0f));
        SpawnEnemy(enemy_Uni_RoundSpread, new Vector2(0f, 0f));
        SpawnEnemy(enemy_Uni_Homing, new Vector2(-4f, 0f));

        yield return threeSecWait;
        yield return oneSecWait;
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_RoundSpread, new Vector2(4f, 0f));
        SpawnEnemy(enemy_Uni_Homing, new Vector2(0f, 0f));
        SpawnEnemy(enemy_Uni_RoundSpread, new Vector2(-4f, 0f));

        yield return threeSecWait;
        yield return oneSecWait;
        yield return oneSecWait;

        SpawnEnemy(enemy_Uni_RoundSpread, new Vector2(4f, 0f));
        SpawnEnemy(enemy_Uni_RoundSpread, new Vector2(0f, 0f));
        SpawnEnemy(enemy_Uni_RoundSpread, new Vector2(-4f, 0f));

        yield return oneSecWait;
        SpawnEnemy(enemy_LazerGuy_Blue, new Vector2(4f, 1f));
        SpawnEnemy(enemy_LazerGuy_Blue, new Vector2(-4f, 1f));

        yield return oneSecWait;
        SpawnEnemy(enemy_LazerGuy_Blue, new Vector2(1f, 1f));
        SpawnEnemy(enemy_LazerGuy_Blue, new Vector2(-1f, 1f));

        yield return threeSecWait;
        yield return oneSecWait;
        yield return oneSecWait;

        PoolManager.Instance.OnUseSpell.Invoke();

        currEnemy = SpawnEnemy(enemy_TechOne, new Vector2(-8f, 0f));
        yield return new WaitUntil(() => currEnemy.is_Die);

        yield return threeSecWait;
        yield return oneSecWait;

        GameManager.Instance.OnGameClear.Invoke();
    }


    private void GameOver()
    {
        StopCoroutine(stageRoutine);
    }

    private void MoveNextStage()
    {
        if (currentStage >= stageList.Count - 1) return;

        GameManager.Instance.currentStage++;
        currentStage++;
        stageRoutine = StartCoroutine(stageList[currentStage]);
    }

    private Enemy SpawnEnemy(string enemyName, Vector3 spawnOffset)
    {
        var obj = PoolManager.Instance.GetQueue(PoolType.Enemy, enemyName);
        obj.transform.position = spawnPoint.position + spawnOffset;
        obj.SetActive(true);

        return obj.GetComponent<Enemy>();
    }
}
