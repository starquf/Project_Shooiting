using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoint : MonoBehaviour
{
    private IAttack playerAttack = null;
    private ISpell playerSpell = null;

    [SerializeField]
    private PlayerDamaged playerDamaged = null;

    public bool can_Earn = true;

    private void Start()
    {
        playerAttack = transform.parent.GetComponent<IAttack>();
        playerSpell = transform.parent.GetComponent<ISpell>();
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        Point point = coll.GetComponent<Point>();

        if (point != null && can_Earn)
        {
            EarnPoint(point.pointType);
            point.SetDisable();
        }
    }

    private void EarnPoint(PointType point)
    {
        switch (point)
        {
            case PointType.Score:
                GameManager.Instance.uiHandler.AddScore(100);
                break;

            case PointType.AutoScore:
                GameManager.Instance.uiHandler.AddScore(50);
                break;

            case PointType.Upgrade:

                playerAttack.Upgrade(1);
                break;

            case PointType.BigUpgrade:

                playerAttack.Upgrade(5);
                break;

            case PointType.SpellUp:

                playerSpell.AddBomb();
                break;

            case PointType.LifeUp:

                playerDamaged.AddHealth();
                break;

        }
    }
}
