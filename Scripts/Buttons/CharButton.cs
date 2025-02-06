using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IBeginDragHandler
{
    [SerializeField]
    private ArmorType armorType;

    private Armor equippedArmor;

    [SerializeField]
    private Image icon;

    public Armor MyEquippedArmor { get => equippedArmor; }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable is Armor)
            {
                Armor tmpOld = MyEquippedArmor;

                // Cast possible cause Armor > Item which is IMoveable
                Armor tmp = (Armor)HandScript.MyInstance.MyMoveable;

                if (tmp.MyArmorType == armorType)
                {
                    if (tmpOld != null)
                    {
                        CharacterPanel.MyInstance.MySelectedButton = this;
                        CharacterPanel.MyInstance.MySelectedButton.DequipArmor();

                        InventoryScript.MyInstance.AddItem(tmpOld);
                    }

                    EquipArmor(tmp);

                }

                UIManager.MyInstance.RefreshTooltip(tmp);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Checking if hand is empty AND if we are clicking on a slot with a armor equipped
            if (HandScript.MyInstance.MyMoveable == null && MyEquippedArmor != null)
            {
                icon.color = Color.gray;
                HandScript.MyInstance.TakeMoveable(MyEquippedArmor);

                CharacterPanel.MyInstance.MySelectedButton = this;
                CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && HandScript.MyInstance.MyMoveable == null && MyEquippedArmor != null)
        {
            UIManager.MyInstance.HideTooltip();            
            InventoryScript.MyInstance.AddItem(MyEquippedArmor);            
            CharacterPanel.MyInstance.MySelectedButton = this;
            CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable is Armor)
            {
                Armor tmpOld = MyEquippedArmor;

                // Cast possible cause Armor > Item which is IMoveable
                Armor tmp = (Armor)HandScript.MyInstance.MyMoveable;

                if (tmp.MyArmorType == armorType)
                {
                    if (tmpOld != null)
                    {
                        CharacterPanel.MyInstance.MySelectedButton = this;
                        CharacterPanel.MyInstance.MySelectedButton.DequipArmor();

                        InventoryScript.MyInstance.AddItem(tmpOld);
                    }
                    
                    EquipArmor(tmp);
                    
                }

                UIManager.MyInstance.RefreshTooltip(tmp);
            }
            // Checking if hand is empty AND if we are clicking on a slot with a armor equipped
            else if (HandScript.MyInstance.MyMoveable == null && MyEquippedArmor != null)
            {
                icon.color = Color.gray;
                HandScript.MyInstance.TakeMoveable(MyEquippedArmor);
                CharacterPanel.MyInstance.MySelectedButton = this;
                CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
            }         
        }
    }
    public void EquipArmor(Armor armor) // Equipping new piece of armor
    {
        //Debug.Log("EquipArmor");
        if (armor.MyArmorType == ArmorType.Fingers)
        {
            // We can check here if we're equipping a ring
        }
        armor.Remove();

        icon.enabled = true;
        icon.sprite = armor.MyIcon;
        icon.color = Color.white;
        
        if (equippedArmor != null)
        {
            Player.MyInstance.OnEquipementRemoved(equippedArmor);

            if (equippedArmor != armor)
            {
                armor.MySlot.AddItem(equippedArmor);
                // Old emplacement
                //Player.MyInstance.OnEquipementRemoved(equippedArmor);
            }
            
            UIManager.MyInstance.RefreshTooltip(equippedArmor);
        }
        else if (true)
        {
            UIManager.MyInstance.HideTooltip();
        }


        // ???
        //InventoryScript.MyInstance.FromSlot = null;

        if (InventoryScript.MyInstance.FromSlot != null)
        {
            // We can check here if the slot contain smth already

            InventoryScript.MyInstance.FromSlot.MyCover.enabled = false;
        }

        this.equippedArmor = armor; // Reference to the equipped armor
        this.MyEquippedArmor.MyCharButton = this;
        Player.MyInstance.OnEquipementChanged(armor);

        //
        if (HandScript.MyInstance.MyMoveable == (armor as IMoveable))
        {
            HandScript.MyInstance.Drop();
        }

        // ANIMATION 2D
        //if (gearSocket != null && MyEquippedArmor.MyAnimationClips != null)
        //{
        //    gearSocket.Equip(MyEquippedArmor.MyAnimationClips);
        //}        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MyEquippedArmor!= null)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1, 1), transform.position, MyEquippedArmor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

    public void DequipArmor(/*Armor armor*/)
    {
        // Old emplacement
        //Player.MyInstance.OnEquipementRemoved(MyEquippedArmor);


        // ANIMATION 2D
        if (MyEquippedArmor != null)
        {
            Player.MyInstance.OnEquipementRemoved(MyEquippedArmor);
        }

        icon.color = Color.white;
        icon.enabled = false;

        // Randomly fixed problems but no idea if the guy from the tutorial have this line or not
        //equippedArmor.MyCharButton = null;

        equippedArmor = null;
        
    }
}
