﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IBeginDragHandler
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackSize;

    [SerializeField]
    private Image cover;

    // reference to the bag that this slot belong to
    public BagScript MyBag { get; set; }

    public int MyIndex { get; set; }

    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }
    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            return true;
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }
            return null;
        }
    }

    public Image MyIcon
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }
    }

    public int MyCount
    {
        get
        {
            return MyItems.Count;
        }
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    public ObservableStack<Item> MyItems
    {
        get
        {
            return items;
        }
    }

    public Image MyCover { get => cover; }

    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty) // if we dont have anything in hand
            {
                // checking if moving something
                if (HandScript.MyInstance.MyMoveable != null)
                {
                    if (HandScript.MyInstance.MyMoveable is Armor)
                    {
                        if (MyItem is Armor && (MyItem as Armor).MyArmorType == (HandScript.MyInstance.MyMoveable as Armor).MyArmorType)
                        {
                            (MyItem as Armor).Equip();
                            // We need to instantiate the previous item from hand in the inventory 
                            if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Item)
                            {
                                InventoryScript.MyInstance.AddItem(HandScript.MyInstance.MyMoveable as Item);
                            }

                            HandScript.MyInstance.Drop();
                        }
                    }

                }
                else
                {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                    InventoryScript.MyInstance.FromSlot = this;
                }
            }
            else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty)
            {
                if (HandScript.MyInstance.MyMoveable is Armor)
                {
                    Armor armor = (Armor)HandScript.MyInstance.MyMoveable;

                    Debug.Log(armor);
                    AddItem(armor);
                    //DEQUIP SIMPLE FUNCTION   
                    CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
                    HandScript.MyInstance.Drop();
                }
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (HandScript.MyInstance.MyMoveable != null && InventoryScript.MyInstance.FromSlot == null && !IsEmpty)
        {
            if (HandScript.MyInstance.MyMoveable is Armor)
            {
                if (MyItem is Armor && (MyItem as Armor).MyArmorType == (HandScript.MyInstance.MyMoveable as Armor).MyArmorType)
                {
                    (MyItem as Armor).Equip();
                    // We need to instantiate the previous item from hand in the inventory 
                    if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Item)
                    {
                        InventoryScript.MyInstance.AddItem(HandScript.MyInstance.MyMoveable as Item);
                    }
                    HandScript.MyInstance.Drop();
                }
                //AddArmor(HandScript.MyInstance.MyMoveable as Item, this);
                //HandScript.MyInstance.Drop();
            }
        }
        else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty)
        {
            if (HandScript.MyInstance.MyMoveable is Armor)
            {
                Armor armor = (Armor)HandScript.MyInstance.MyMoveable;

                AddItem(armor);
                //DEQUIP SIMPLE FUNCTION   
                CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
                HandScript.MyInstance.Drop();
            }
        }
        if (InventoryScript.MyInstance.FromSlot != null) // if we have smth to move
        {
            // we will try to do differentst things to place the item back into the inventory
            if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) || SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
            {
                HandScript.MyInstance.Drop();
                InventoryScript.MyInstance.FromSlot = null;
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty) // if we dont have anything in hand
            {
                // checking if moving smth
                if (HandScript.MyInstance.MyMoveable != null)
                {
                    if (HandScript.MyInstance.MyMoveable is Armor)
                    {
                        if (MyItem is Armor && (MyItem as Armor).MyArmorType == (HandScript.MyInstance.MyMoveable as Armor).MyArmorType)
                        {
                            Debug.Log("Check 1, Equip");
                            (MyItem as Armor).Equip();
                            // We need to instantiate the previous item from hand in the inventory 
                            if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Item)
                            {
                                Debug.Log("Check 2, AddItem");
                                InventoryScript.MyInstance.AddItem(HandScript.MyInstance.MyMoveable as Item);
                            }

                            HandScript.MyInstance.Drop();
                        }
                    }

                }
                else
                {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                    InventoryScript.MyInstance.FromSlot = this;
                }
            }
            else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty)
            {
                if (HandScript.MyInstance.MyMoveable is Armor)
                {
                    Armor armor = (Armor)HandScript.MyInstance.MyMoveable;

                    AddItem(armor);
                    //DEQUIP SIMPLE FUNCTION   
                    CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
                    HandScript.MyInstance.Drop();
                }
            }
            else if (InventoryScript.MyInstance.FromSlot != null) // if we have smth to move
            {
                // we will try to do differentst things to place the item back into the inventory
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) || SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right && InventoryScript.MyInstance.FromSlot == null && HandScript.MyInstance.MyMoveable == null) // if we rightclick on the slot & nothing in hand
        {
            UseItem();
        }
    }
    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        MyCover.enabled = false;
        item.MySlot = this;

        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;
            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }
            Debug.Log("AddItems");
            return true;
        }

        return false;
    }
    public bool AddArmor(Item armor, SlotScript slot)
    {
        if (slot.IsEmpty)
        {
            if (slot.IsFull)
            {
                return false;
            }

            AddItem(armor);
            
            Debug.Log("AddArmor");
            return true;
        }

        return false;
    }

    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
        }
    }

    public void Clear()
    {
        int initCount = MyItems.Count;
        MyCover.enabled = false;

        if (initCount > 0)
        {
            for (int i = 0; i < initCount; i++)
            {
                InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            }
            //MyItems.Clear();
        }
    }

    public void UseItem()
    {
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
        else if (MyItem is Armor)
        {
            (MyItem as Armor).Equip();
        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            Debug.Log("StackItem");
            return true;
        }
        return false;
    }

    private bool PutItemBack()
    {
        MyCover.enabled = false;

        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.enabled = true;
            Debug.Log("PutItemBack");
            return true;
        }
        return false;
    }

    private bool SwapItems(SlotScript from)
    {
        from.MyCover.enabled = false;

        if (IsEmpty)
        {
            return false;
        }

        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.MyStackSize)
        {
            // copy all the items we need to swap from A
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            // clear slot A
            from.MyItems.Clear();
            // all items from slot B and copy them into A
            from.AddItems(MyItems);

            // clear B
            MyItems.Clear();

            //Move the items from A copy to B
            AddItems(tmpFrom);

            Debug.Log("SwapItems");
            return true;
        }

        return false;
    }

    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        // is it the same type and is the stack not full
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            // how many free slots do we have in the stack
            int free = MyItem.MyStackSize - MyCount;

            for (int i = 0; i < free; i++)
            {
                if (from.MyCount > 0)
                {
                    AddItem(from.MyItems.Pop());
                    Debug.Log(from.MyItems.Count);
                }
            }
            Debug.Log("MergeItems");
            return true;
        }

        return false;
    }
    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Showing tooltip
        if (!IsEmpty)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1, 0), transform.position, MyItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hiding tooltip
        UIManager.MyInstance.HideTooltip();
    }
}
