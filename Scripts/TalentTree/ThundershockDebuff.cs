using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThundershockDebuff : Debuff
{
    public float MySpeedReduction { get; set; }

    public override string Name => "Thundershock";

    public ThundershockDebuff()
    {
        MyDuration = 1;
    }

    public override void Apply(Character character)
    {
        (character as Enemy).ChangeState(new StunnedState());
        base.Apply(character);
    }

    public override void Remove()
    {
        (character as Enemy).ChangeState(new FollowState());
        base.Remove();
    }

    public override Debuff Clone()
    {
        ThundershockDebuff clone = (ThundershockDebuff)this.MemberwiseClone();
        return clone;
    }
}
