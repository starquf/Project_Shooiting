using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointMagnet : MonoBehaviour
{
    public bool can_Magnet = true;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!can_Magnet) return;

        Point point = coll.GetComponent<Point>();

        if (point != null)
        {
            point.SetSpeed(10f);
            point.is_Auto = true;
        }
    }
}
