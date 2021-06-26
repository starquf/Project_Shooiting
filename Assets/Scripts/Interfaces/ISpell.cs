using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpell
{
    public bool can_Spell { get; set; }

    public void SetBomb(int bomb);
    public void AddBomb();
}
