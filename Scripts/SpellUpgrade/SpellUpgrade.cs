using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellUpgrade : MonoBehaviour
{
    [SerializeField]
    private int points = 10;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public void IncreaseDamage()
    {
        SpellBook.MyInstance.GetSpell("Fireball").MyDamageMin += 50;
        Debug.Log("fireball damage :" + SpellBook.MyInstance.GetSpell("Fireball").MyDamageMin);
    }
    public void DecreaseManacost()
    {
        SpellBook.MyInstance.GetSpell("Fireball").MyManaCost -= 50;
    }
    public void DecreaseCooldown()
    {
        SpellBook.MyInstance.GetSpell("Fireball").MyCooldown -= 50;
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
