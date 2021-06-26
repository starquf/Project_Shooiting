using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Create_MoveCenter : MonoBehaviour, IState
{
    public float moveDur = 1f;

    public void OnEnter()
    {
        transform.DOMove(new Vector3(GameManager.Instance.mapCenter.x, 4f), moveDur);
    }

    public void OnEnd()
    {

    }
}
