using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardSpell : AoESpell
{
    [SerializeField]
    protected float slow;

    public override void Enter(Enemy enemy)
    {
        enemy.CurrentSpeed = enemy.Speed / 2;
        //Debug.Log(enemy.Speed);
        //Debug.Log(enemy.CurrentSpeed);
        //Debug.Log(enemy.name);
        base.Enter(enemy);
    }

    public override void Exit(Enemy enemy)
    {
        enemy.CurrentSpeed = enemy.Speed;
        //Debug.Log("Exit1");        
        //Debug.Log(enemy.Speed);
        //Debug.Log(enemy.name);
        base.Exit(enemy);
    }

    public override void Remove()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].CurrentSpeed = enemies[i].Speed;
        }
        base.Remove();
    }

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
