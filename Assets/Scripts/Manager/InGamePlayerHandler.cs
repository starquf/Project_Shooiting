using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePlayerHandler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> playerObjs = new List<GameObject>();

    private void Start()
    {
        switch (GameManager.Instance.playerType)
        {
            case PlayerType.Tech:

                playerObjs[0].SetActive(true);
                break;

            case PlayerType.Knife:

                playerObjs[1].SetActive(true);
                break;
        }
    }
}
