using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item);
public class InventoryScript : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }
            return instance;
        }
    }
    private SlotScript fromSlot;

    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;

    // For debugging
    [SerializeField]
    private Item[] items;

    public bool CanAddBag
    {
        get { return MyBags.Count < 5; }
    }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (Bag bag in MyBags)
            {
                count += bag.MyBagScript.MyEmptySlotCount;
            }

            return count;
        }
    }

    public int MyTotalSlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in MyBags)
            {
                count += bag.MyBagScript.MySlots.Count;
            }

            return count;
        }
    }

    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }
    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }
        set
        {
            fromSlot = value;

            if (value !=null)
            {
                fromSlot.MyCover.enabled = true;
            }
        }
    }

    public List<Bag> MyBags { get => bags; }

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[13]);
        bag.Initialize(30);
        bag.Use();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //AddItem((Armor)Instantiate(items[0]));
            AddItem((Armor)Instantiate(items[1]));
            //AddItem((Armor)Instantiate(items[2]));
            //AddItem((Armor)Instantiate(items[3]));
            //AddItem((Armor)Instantiate(items[4]));
            //AddItem((Armor)Instantiate(items[5]));
            //AddItem((Armor)Instantiate(items[6]));
            //AddItem((Armor)Instantiate(items[7]));
            AddItem((Armor)Instantiate(items[8]));
            //AddItem((Armor)Instantiate(items[9]));
            //AddItem((Armor)Instantiate(items[10]));
            //AddItem((Armor)Instantiate(items[11]));
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[12]);
            AddItem(potion);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GoldNugget nugget = (GoldNugget)Instantiate(items[14]);
            AddItem(nugget);
        }

    }
    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                MyBags.Add(bag);
                bag.MyBagButton = bagButton;
                bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
                break;
            }
        }
    }

    public void AddBag(Bag bag, BagButton bagButton)
    {
        MyBags.Add(bag);
        bagButton.MyBag = bag;
        bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
    }

    public void AddBag(Bag bag, int bagIndex)
    {
        bag.SetupScript();
        MyBags.Add(bag);
        bag.MyBagScript.MyBagIndex = bagIndex;
        bag.MyBagButton = bagButtons[bagIndex];
        bagButtons[bagIndex].MyBag = bag;
    }

    public void RemoveBag(Bag bag)
    {
        MyBags.Remove(bag);
        Destroy(bag.MyBagScript.gameObject);
    }

    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (MyTotalSlotCount - oldBag.MySlotCount) + newBag.MySlotCount;
        
        if (newSlotCount - MyFullSlotCount >= 0)
        {
            // do swap
            List<Item> bagItems = oldBag.MyBagScript.GetItems();

            RemoveBag(oldBag);

            newBag.MyBagButton = oldBag.MyBagButton;

            newBag.Use();
            foreach (Item item in bagItems)
            {
                if (item != newBag) // no duplicate bag
                {
                    AddItem(item);
                }
            }
            AddItem(oldBag);
            HandScript.MyInstance.Drop();
            MyInstance.fromSlot = null;
        }
    }

    // Add an item to the inventory
    public bool AddItem (Item item)
    {
        if (item.MyStackSize > 0)
        {
            // Check if we can stack the item om a stack
            if (PlaceInStack(item))
            {
                return true;
            }
        }
        // Check if we can place it in an empty slot
        return PlaceInEmpty(item);
    }

    public bool AddItemBack(Item item, SlotScript slot)
    {
        if (item.MyStackSize > 0)
        {
            // Check if we can stack the item om a stack
            if (PlaceInStack(item))
            {
                return true;
            }
        }
        // Check if we can place it in an empty slot
        return PlaceInEmpty(item);
    }


    private bool PlaceInEmpty(Item item)
    {
        foreach (Bag bag in MyBags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                OnItemCountChanged(item);
                return true;
            }
        }

        // Inventory is full
        return false;
    }
    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slots in bag.MyBagScript.MySlots)
            {
                if (slots.StackItem(item))
                {
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }
        return false;
    }

    public void PlaceInSpecific(Item item, int slotIndex, int bagIndex)
    {
        bags[bagIndex].MyBagScript.MySlots[slotIndex].AddItem(item);
    }


    public void OpenClose()
    {
        bool closeBag = MyBags.Find(x => !x.MyBagScript.IsOpen);
        // if closed bag is true then open all close bags
        // if no closed bags, then close all bags

        foreach (Bag bag in MyBags)
        {
            if (bag.MyBagScript.IsOpen != closeBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }
    public void Open()
    {
        foreach (Bag bag in MyBags)
        {
            bag.MyBagScript.Open();
        }
    }

    public void Close()
    {
        foreach (Bag bag in MyBags)
        {
            bag.MyBagScript.Close();
        }
    }

    public Stack<IUseable> GetUseables (IUseable type)
    {
        // Creating stack of useables
        Stack<IUseable> useables = new Stack<IUseable>();

        // Looping into all the bags
        foreach (Bag bag in MyBags)
        {
            // Everytime we look at each bag we check all the slots
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                // If we have an item into a slot we check if its the same type as the one we looking for
                if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
                {
                    // If thats the case we take all the items and put them into the stack
                    foreach (Item item in slot.MyItems)
                    {
                        useables.Push(item as IUseable);
                    }    
                }
            }
        }
        // When we done
        return useables;
    }

    public List<SlotScript> GetAllItems()
    {
        List<SlotScript> slots = new List<SlotScript>();
        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty)
                {
                    slots.Add(slot);
                }
            }
        }
        return slots;
    }

    public IUseable GetUseable(string type)
    {
        // Creating stack of useables
        Stack<IUseable> useables = new Stack<IUseable>();

        // Looping into all the bags
        foreach (Bag bag in MyBags)
        {
            // Everytime we look at each bag we check all the slots
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                // If we have an item into a slot we check if its the same type as the one we looking for
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    return (slot.MyItem as IUseable);
                }
            }
        }
        // When we done
        return null;
    }

    public int GetItemCount(string type)
    {
        int itemCount = 0;

        // Checking in the bag
        foreach (Bag bag in MyBags)
        {
            // Checking each slot of the bags
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                // If slot is not empty, and the title of the item in it is the type we are looking for
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    // We add to the count the number of item on the slot
                    itemCount += slot.MyItems.Count;
                }
            }
        }
        return itemCount;
    }


    public Stack<Item> GetItems(string type, int count)
    {
        // Creating a stack of items
        Stack<Item> items = new Stack<Item>();

        // Checking in the bag
        foreach (Bag bag in MyBags)
        {
            // Checking each slot of the bags
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                // If slot is not empty, and the title of the item in it is the type we are looking for
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    // For each item that are the one we looking for, we add it to the stack
                    foreach (Item item in slot.MyItems)
                    {
                        items.Push(item);

                        if (items.Count == count)
                        {
                            return items;
                        }
                    }
                }
            }
        }

        return items;
    }

    public void RemoveItem(Item item)
    {
        // Checking in the bag
        foreach (Bag bag in MyBags)
        {
            // Checking each slot of the bags
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                // If slot is not empty, and the title of the item in it is the type we are looking for
                if (!slot.IsEmpty && slot.MyItem.MyTitle == item.MyTitle)
                {
                    slot.RemoveItem(item);
                    break;
                }
            }
        }
    }

    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }
}
