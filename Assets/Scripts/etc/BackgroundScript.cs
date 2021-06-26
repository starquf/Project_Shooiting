using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.maxSize = new Vector2(transform.position.x + (transform.localScale.x / 2f), 
            transform.position.y + (transform.localScale.y / 2f));

        GameManager.Instance.minSize = new Vector2(transform.position.x - (transform.localScale.x / 2f), 
            transform.position.y - (transform.localScale.y / 2f));

        GameManager.Instance.mapCenter = transform.position;
    }
}
