using System.Collections.Generic;
using UnityEngine;

public class PowerPick : MonoBehaviour
{
    [SerializeField] public GameObject[] powerOption;
    [SerializeField] List<PowerCard> powerCardList = new List<PowerCard>();

    public void PowerRoll()
    {
        for (int i = 0; i < powerOption.Length; i++)
        {
            powerOption[i].GetComponent<PowerOption>().powerCard = powerCardList[Random.Range(0, powerCardList.Count)];
            powerOption[i].GetComponent<PowerOption>().UpdatePowerCard();
            powerCardList.Remove(powerOption[i].GetComponent<PowerOption>().powerCard);
        }
    }

    // Hard coding a way for me to manualy add each power
    public void powerLogic(int i)
    {
        string ID = powerOption[i].GetComponent<PowerOption>().powerCard.power_ID;

        if (ID == "ID_001")
        {
            MaledictionDebuff malediction = new MaledictionDebuff();
            SpellBook.MyInstance.GetSpell("Fireball").MyDebuff = malediction;            
        }

        if (ID == "ID_002")
        {
            IgniteDebuff ignite = new IgniteDebuff();
            SpellBook.MyInstance.GetSpell("Fireball").MyDebuff = ignite;
        }

        if (ID == "ID_003")
        {
            Debug.Log("improving fireball");
            SpellBook.MyInstance.GetSpell("Fireball").MyCastTime += 0.5f;
            SpellBook.MyInstance.GetSpell("Fireball").MyDamageMax += 100f;
        }

        // Need to add each power ID manually here

        UIManager.MyInstance.CloseSingle(this.GetComponent<CanvasGroup>());
    }

}


