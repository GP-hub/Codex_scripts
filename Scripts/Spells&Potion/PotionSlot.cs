using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionSlot : MonoBehaviour
{
    public void Use()
    {
        if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUseable)
        {
            if (!HandScript.MyInstance.MyMoveable.ToString().Contains("Potion"))
            {
                HandScript.MyInstance.Drop();
                Debug.Log("potion drop");
            }
        }
    }
}
