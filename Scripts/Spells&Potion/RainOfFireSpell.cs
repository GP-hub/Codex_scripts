using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainOfFireSpell : AoESpell
{
    public override void Execute()
    {
        tickElapsed += Time.deltaTime;

        if (tickElapsed >= 1)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].TakeDamage(damage / duration, Player.MyInstance);
            }

            tickElapsed = 0;
        }
    }
}
