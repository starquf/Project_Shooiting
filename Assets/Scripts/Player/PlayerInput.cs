using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool canInput = true;

    public bool Attack { get; private set; }
    public bool Skill { get; private set; }
    public bool MoveSlow { get; private set; }

    private Vector3 mousePos = Vector3.zero;
    public Vector3 MousePos {
        get { 
            mousePos.z = transform.position.z;
            return mousePos;
        }
        
        private set { 
            mousePos = value; 
        } 
    }

    public float x { get; private set; }
    public float y { get; private set; }

    private void Start()
    {
        GameManager.Instance.OnStageClear.AddListener(() => {
            canInput = false;
            ResetValue();
        });

        GameManager.Instance.OnStageEnd.AddListener(() => {
            canInput = true;
        });

        GameManager.Instance.OnGamePause.AddListener((enable) => 
        {
            canInput = enable;
        });

        GameManager.Instance.OnGameRestart.AddListener(() => {
            canInput = false;
            ResetValue();
        });

        GameManager.Instance.OnGameClear.AddListener(() => {
            canInput = false;
            ResetValue();
        });
    }

    void Update()
    {
        if (!canInput) return;

        Attack = Input.GetButton("Fire1") || Input.GetButton("Attack");
        Skill = Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.X);
        MoveSlow = Input.GetButton("MoveSlow");

        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void ResetValue()
    {
        Attack = false;
        Skill = false;
        MoveSlow = false;
        x = 0;
        y = 0;
    }
}
