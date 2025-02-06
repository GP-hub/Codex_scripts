using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    private static CharacterPanel instance;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private CharButton head, shoulders, chest, hands, feet, legs, wrists, waist, grimoire, neck, finger1, finger2;

    public CharButton MySelectedButton { get; set; }

    public static CharacterPanel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CharacterPanel>();
            }
            return instance;
        }
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }

    public void EquipArmor(Armor armor)
    {
        switch (armor.MyArmorType)
        {
            case ArmorType.Head:
                head.EquipArmor(armor);
                break;
            case ArmorType.Shoulders:
                shoulders.EquipArmor(armor);
                break;
            case ArmorType.Chest:
                chest.EquipArmor(armor);
                break;
            case ArmorType.Hands:
                hands.EquipArmor(armor);
                break;
            case ArmorType.Feet:
                feet.EquipArmor(armor);
                break;
            case ArmorType.Legs:
                legs.EquipArmor(armor);
                break;
            case ArmorType.Wrists:
                wrists.EquipArmor(armor);
                break;
            case ArmorType.Waist:
                waist.EquipArmor(armor);
                break;
            case ArmorType.Grimoire:
                grimoire.EquipArmor(armor);
                break;
            case ArmorType.Neck:
                neck.EquipArmor(armor);
                break;
            case ArmorType.Fingers:
                if (finger1.MyEquippedArmor == null)
                {
                    finger1.EquipArmor(armor);
                    break;
                }
                if (finger1.MyEquippedArmor != null && finger2.MyEquippedArmor == null)
                {
                    finger2.EquipArmor(armor);
                    break;
                }
                if (finger1.MyEquippedArmor != null && finger2.MyEquippedArmor != null)
                {
                    finger1.EquipArmor(armor);
                    break;
                }             
                break;
        }
    }
}
