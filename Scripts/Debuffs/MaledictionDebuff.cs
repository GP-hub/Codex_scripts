using System;
using System.Collections.Generic;
using UnityEngine;

class MaledictionDebuff : Debuff
{
    public float MaledictStack;

    public override string Name => "Malediction";

    private float elapsed;
    private float baseAttackCooldown;

    public MaledictionDebuff()
    {
        MyDuration = 5;
        MaledictStack = 0;
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed >= 1)
        {
            MaledictStack += 1;
            elapsed = 0;
        }
        if (MaledictStack > 10) 
        {
            character.TakeDamage(999999, Player.MyInstance.GetComponent<Character>());
        }

        base.Update();
    }


    public override void Apply(Character character)
    {       
        character.GetComponent<Enemy>().MyAttackCooldown *= 10;
        base.Apply(character);
    }

    public override void Remove()
    {
        character.GetComponent<Enemy>().MyAttackCooldown /= 10;
        elapsed = 0;
        base.Remove();
    }


    public override Debuff Clone()
    {
        MaledictionDebuff clone = (MaledictionDebuff)this.MemberwiseClone();
        return clone;
    }
}
