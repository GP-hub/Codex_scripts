using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable
{
    void Use();

    float MyCooldown
    {
        get;
    }
    bool OnCooldown
    {
        set;

        get;
    }
}
