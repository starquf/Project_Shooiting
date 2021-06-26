using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    public bool can_shoot { get; set; }

    public void SetWeaponEnable(bool enable);
    public void Upgrade(int value);
}
