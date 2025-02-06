using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NUKEspell : AoESpell
{
    public override void Execute()
    {

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].TakeDamage(damage / duration, Player.MyInstance);
            }

    }
}
