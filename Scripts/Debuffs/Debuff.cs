using System;
using UnityEngine;

[Serializable]
public abstract class Debuff
{
    public float MyDuration { get; set; }

    public float MyDurationLeft { get; set; }
    
    public float ProcChance { get; set; }

    public float elapsed;

    protected Character character;

    public abstract string Name
    {
        get;
    }

    public virtual void Apply(Character character)
    {
        this.character = character;
        character.ApplyDebuff(this);
    }

    public virtual void Remove()
    {
        character.RemoveDebuff(this);
        elapsed = 0;
    }

    public virtual void Update()
    {
        elapsed += Time.deltaTime;
        MyDurationLeft = MyDuration - elapsed;

        if (elapsed >= MyDuration)
        //if (MyDurationLeft <= 0) 
        {
            Debug.Log("removing debuff");
            Remove();
        }
    }

    public abstract Debuff Clone();
}
