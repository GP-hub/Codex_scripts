using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class IgniteDebuff : Debuff
{
    public float MyTickDamage { get; set; }

    public override string Name
    {
        get { return "Ignite"; }
    }
    private float elapsed;
    private float tickNbr = 0;

    public IgniteDebuff()
    {
        //MyDuration = 20;
        MyDuration = 7;
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;

        // Dot proccing every 1 second.
        if (elapsed >= 1)
        {
            tickNbr += 1;
            character.TakeDamage(MyTickDamage, null);
            elapsed = 0;
        }

        base.Update();
    }

    public override void Apply(Character character)
    {
        tickNbr += 1;
        character.TakeDamage(MyTickDamage, null);
        base.Apply(character);
    }

    public override void Remove()
    {
        elapsed = 0;
        base.Remove();
    }

    public override Debuff Clone()
    {
        IgniteDebuff clone = (IgniteDebuff)this.MemberwiseClone();
        return clone;
    }
}
