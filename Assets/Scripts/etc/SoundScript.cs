using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    [SerializeField]
    private float waitTime = 1f;

    private WaitForSeconds activeWait;

    private void Start()
    {
        activeWait = new WaitForSeconds(waitTime);
    }

    void OnEnable()
    {
        StartCoroutine(SetDisable());
    }

    private IEnumerator SetDisable()
    {
        yield return null;
        yield return activeWait;

        gameObject.SetActive(false);
    }
}
