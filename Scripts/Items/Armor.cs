using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

enum ArmorType {Head, Shoulders, Chest, Hands, Feet, Legs, Wrists, Waist, Grimoire, Neck, Fingers}
public enum LegendaryPower {Frostball = 01, POWER_02 = 02, POWER_03 = 03, POWER_04 = 04, POWER_05 = 05 }

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]
public class Armor : Item
{
    [SerializeField]
    private ArmorType armorType;

    //[SerializeField]
    //public List<LegendaryPower> legendaryPower = new List<LegendaryPower>();

    public int legendaryPower;

    public int intellect;

    public int stamina;

    public int firedamage;

    public int frostdamage;

    public int lightningdamage;

    public int corrosivedamage;

    public bool randomStat = false;

    
    #region ANIMATION 2D TUTORIAL

    [SerializeField]
    private AnimationClip[] animationClips;

    public AnimationClip[] MyAnimationClips { get => animationClips; }

    #endregion


    internal ArmorType MyArmorType
    {
        get
        {
            return armorType;
        }
    }

    public override string GetDescription()
    {
        if (MyArmorType == ArmorType.Head)
        {

        }
        switch (MyArmorType)
        {
            case ArmorType.Head:
                break;
            case ArmorType.Shoulders:
                break;
            case ArmorType.Chest:
                break;
            case ArmorType.Hands:
                break;
            case ArmorType.Feet:
                break;
            case ArmorType.Legs:
                break;
            case ArmorType.Wrists:
                break;
            case ArmorType.Waist:
                break;
            case ArmorType.Grimoire:
                break;
            case ArmorType.Neck:
                break;
            case ArmorType.Fingers:
                break;
            default:
                break;
        }
        if (randomStat == false)
        {
            switch (MyQuality)
            {
                case Quality.Common:
                    intellect = Random.Range(0, 21);
                    stamina = Random.Range(10, 51);

                    firedamage = Random.Range(-15, 26);
                    frostdamage = Random.Range(-15, 26);
                    lightningdamage = Random.Range(-15, 26);
                    corrosivedamage = Random.Range(-15, 26);
                    break;
                case Quality.Uncommon:
                    intellect = Random.Range(20, 51);
                    stamina = Random.Range(50, 76);

                    firedamage = Random.Range(-30, 51);
                    frostdamage = Random.Range(-30, 51);
                    lightningdamage = Random.Range(-30, 51);
                    corrosivedamage = Random.Range(-30, 51);
                    break;
                case Quality.Rare:
                    intellect = Random.Range(50, 76);
                    stamina = Random.Range(75, 101);

                    firedamage = Random.Range(-55, 76);
                    frostdamage = Random.Range(-55, 76);
                    lightningdamage = Random.Range(-55, 76);
                    corrosivedamage = Random.Range(-55, 76);
                    break;
                case Quality.Epic:
                    intellect = Random.Range(75, 101);
                    stamina = Random.Range(100, 126);

                    firedamage = Random.Range(-75, 101);
                    frostdamage = Random.Range(-75, 101);
                    lightningdamage = Random.Range(-75, 101);
                    corrosivedamage = Random.Range(-75, 101);
                    break;
                case Quality.Legendary:
                    intellect = Random.Range(100, 151);
                    stamina = Random.Range(125, 151);

                    firedamage = Random.Range(-100, 151);
                    frostdamage = Random.Range(-100, 151);
                    lightningdamage = Random.Range(-100, 151);
                    corrosivedamage = Random.Range(-100, 151);

                    legendaryPower = (int)(LegendaryPower)Random.Range(0, 6);

                    break;
            }
            randomStat = true;
        }

        string stats = string.Empty;

        if (intellect > 0)
        {
            stats += string.Format("\n +{0} intellect", intellect);
        }
        if (stamina > 0)
        {
            stats += string.Format("\n +{0} stamina", stamina);
        }
        if (firedamage >= 0)
        {
            stats += string.Format("\n\n +{0}% fire damage", firedamage);
        }
        else if (firedamage < 0)
        {
            stats += string.Format("\n\n {0}% fire damage", firedamage);
        }
        if (frostdamage >= 0)
        {
            stats += string.Format("\n +{0}% frost damage", frostdamage);
        }
        else if(frostdamage < 0)
        {
            stats += string.Format("\n {0}% frost damage", frostdamage);
        }
        if (lightningdamage >= 0)
        {
            stats += string.Format("\n +{0}% lightning damage", lightningdamage);
        }
        else if (lightningdamage < 0)
        {
            stats += string.Format("\n {0}% lightning damage", lightningdamage);
        }
        if (corrosivedamage >= 0)
        {
            stats += string.Format("\n +{0}% corrosive damage", corrosivedamage);
        }
        else if (corrosivedamage < 0)
        {
            stats += string.Format("\n {0}% corrosive damage", corrosivedamage);
        }
        if (this.MyQuality == Quality.Legendary)
        {
            stats += string.Format("\n {0} ", (LegendaryPower)legendaryPower);
        }

        return base.GetDescription() + stats;
    }

    public void Equip()
    {
        CharacterPanel.MyInstance.EquipArmor(this);
    }
}