using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ICastable
{
    string MyTitle
    {
        get;
    }

    Sprite MyIcon
    {
        get;
    }

    float MyCastTime
    {
        get;
    }

    Color MyBarColor
    {
        get;
    }
}