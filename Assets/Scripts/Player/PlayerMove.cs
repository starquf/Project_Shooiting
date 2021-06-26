using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput playerInput = null;

    public float speed = 10f;
    public float slowSpeed = 4f;
    private float currSpeed = 3f;

    private Vector3 dir = Vector3.zero;

    [Space(10f)]
    public Vector2 size;

    public bool can_Move = true;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }
    }

    private void Start()
    {
        GameManager.Instance.playerPos = transform;
    }

    private void Update()
    {
        if (can_Move)
        {
            Move();
            ClampPosition();
        }

        if (playerInput.MoveSlow)
        {
            currSpeed = slowSpeed;
        }
        else
        {
            currSpeed = speed;
        }
    }

    private void Move()
    {
        dir = new Vector3(playerInput.x, playerInput.y);
        transform.position += dir.normalized * currSpeed * Time.deltaTime;
    }

    private void ClampPosition()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, GameManager.Instance.minSize.x + size.x, GameManager.Instance.maxSize.x - size.x), 
            Mathf.Clamp(transform.position.y, GameManager.Instance.minSize.y + size.y, GameManager.Instance.maxSize.y - size.y), transform.position.z);
    }
}
