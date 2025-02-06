using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PermafrostDebuff : Debuff
{
    public float MySpeedReduction { get; set; }

    public override string Name => "Permafrost";

    public PermafrostDebuff()
    {
        MyDuration = 3;
    }

    public override void Apply(Character character)
    {
        if (character.CurrentSpeed >= character.Speed)
        {
            //originalSpeed = character.MyNavMeshAgent.speed;
            //character.MyNavMeshAgent.speed = character.MyNavMeshAgent.speed - (character.MyNavMeshAgent.speed * (MySpeedReduction / 100));
            character.CurrentSpeed = character.Speed - (character.Speed * (MySpeedReduction / 100));
            base.Apply(character);
        }        
    }

    public override void Remove()
    {
        //character.MyNavMeshAgent.speed = originalSpeed;
        character.CurrentSpeed = character.Speed;
        base.Remove();
    }

    public override Debuff Clone()
    {
        PermafrostDebuff clone = (PermafrostDebuff)this.MemberwiseClone();
        return clone;
    }
}
