using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public IUseable MyUseable { get; set; }

    [SerializeField]
    private Text stackSize;

    private Stack<IUseable> useables = new Stack<IUseable>();

    private int count;
    public Button MyButton { get; private set; }
    public Image MyIcon { get => icon; set => icon = value; }

    public int MyCount
    {
        get
        {
            return count;
        }
    }
    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    public Stack<IUseable> MyUseables 
    {
        get
        {
            return useables;
        }
        set
        {
            if (value.Count > 0)
            {
                MyUseable = value.Peek();
            }
            else
            {
                MyUseable = null;
            }
            
            useables = value;
        }
    }

    [SerializeField]
    private Image imageCooldown;
    private List<string> charSpellList = new List<string>();
    public Image MyImageCooldown { get => imageCooldown; set => imageCooldown = value; }

    [SerializeField]
    private Image icon;

    // Start is called before the first frame update
    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
        charSpellList = Player.MyInstance.spellList;
        if (this.name == "ACT1")
        {
            if (charSpellList[0] != "")
            {
                SetUseable(SpellBook.MyInstance.GetSpell(charSpellList[0]) as IUseable);
            }
            if (SpellBook.MyInstance.GetSpell(charSpellList[0]) == null)
            {
                //Debug.Log("Button number 1 doesnt have a spell assigned, or its name is misspelled/doesn't exist");
            }
        }
        if (this.name == "ACT2")
        {
            if (charSpellList[1] != "" && SpellBook.MyInstance.GetSpell(charSpellList[1]) != null)
            {
                SetUseable(SpellBook.MyInstance.GetSpell(charSpellList[1]) as IUseable);
            }
            if (SpellBook.MyInstance.GetSpell(charSpellList[1]) == null)
            {
                //Debug.Log("Button number 2 doesnt have a spell assigned, or its name is misspelled/doesn't exist");
            }
        }
        if (this.name == "ACT3")
        {
            if (charSpellList[2] != "")
            {
                SetUseable(SpellBook.MyInstance.GetSpell(charSpellList[2]) as IUseable);
            }
            if (SpellBook.MyInstance.GetSpell(charSpellList[2]) == null)
            {
                //Debug.Log("Button number 3 doesnt have a spell assigned, or its name is misspelled/doesn't exist");
            }
        }
        if (this.name == "ACT4")
        {
            if (charSpellList[3] != "")
            {
                SetUseable(SpellBook.MyInstance.GetSpell(charSpellList[3]) as IUseable);
            }
            if (SpellBook.MyInstance.GetSpell(charSpellList[3]) == null)
            {
                //Debug.Log("Button number 4 doesnt have a spell assigned, or its name is misspelled/doesn't exist");
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (MyUseable != null && MyUseable.OnCooldown == true)
        {
            //if (MyUseable is Spell && Player.MyInstance.MyMana.MyCurrentValue >= (MyUseable as Spell).MyManaCost)
            //{
            //    //Debug.Log((MyUseable as Spell).MyTitle);
            //    Debug.Log(Player.MyInstance.MyMana.MyCurrentValue);
            //    Debug.Log((MyUseable as Spell).MyManaCost);

            //    Debug.Log("PUT ON CD");
            //    CooldownImage();
            //}
            
            CooldownImage();
        }
    }

    public void OnClick()
    {
        
        // If we have smth in hand
        if (HandScript.MyInstance.MyMoveable == null)
        {
            // If the action button is not empty & the player is not attacking 
            if (MyUseable != null && !Player.MyInstance.IsAttacking)
            {
                // If we clicking a spell 
                if (MyUseable is Spell)
                {
                    // We check that the player have enough mana to cast the spell 
                    if (Player.MyInstance.MyMana.MyCurrentValue >= (MyUseable as Spell).MyManaCost)
                    {
                        MyImageCooldown.enabled = true;

                        if (!MyUseable.OnCooldown)
                        {
                            MyUseable.Use();
                            MyUseable.OnCooldown = true;
                            imageCooldown.fillAmount = 1;
                        }
                    }
                }
                else
                {
                    MyImageCooldown.enabled = true;

                    if (!MyUseable.OnCooldown)
                    {
                        MyUseable.Use();
                        MyUseable.OnCooldown = true;
                        imageCooldown.fillAmount = 1;
                    }
                }                
            }
            // If we have a stack of smth we need to use, and theres more than 0
            else if (MyUseables != null && MyUseables.Count > 0)
            {
                MyUseables.Peek().Use();
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUseable)
            {
                // Checking if the slot exist, if it exist, is it in cooldown ?
                if (MyUseable == null || !MyUseable.OnCooldown)
                {
                    // If we have a potion in the hand and click on something else than the dedicated potion slot
                    if (MyButton.transform.name != "ACT5" && HandScript.MyInstance.MyMoveable.ToString().Contains("Potion"))
                    {
                        if (InventoryScript.MyInstance.FromSlot != null)
                        {
                            InventoryScript.MyInstance.FromSlot.MyCover.enabled = false;
                            InventoryScript.MyInstance.FromSlot.MyIcon.enabled = true;
                            InventoryScript.MyInstance.FromSlot = null;
                        }

                        HandScript.MyInstance.Drop();
                    }
                    else
                    {
                        SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                    }
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {        
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUseable)
            {
                // Checking if the slot exist, if it exist, is it in cooldown ?
                if (MyUseable == null || !MyUseable.OnCooldown)
                {
                    // If we have a potion in the hand and click on something else than the dedicated potion slot
                    if (MyButton.transform.name != "ACT5" && HandScript.MyInstance.MyMoveable.ToString().Contains("Potion"))
                    {
                        if (InventoryScript.MyInstance.FromSlot != null)
                        {
                            InventoryScript.MyInstance.FromSlot.MyCover.enabled = false;
                            InventoryScript.MyInstance.FromSlot.MyIcon.enabled = true;
                            InventoryScript.MyInstance.FromSlot = null;
                        }

                        HandScript.MyInstance.Drop();
                    }
                    else
                    {
                        SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
                    }
                }            
            }
        }
    }

    public void CooldownImage()
    {
        if (MyUseable.OnCooldown)
        {
            imageCooldown.fillAmount -= 1 / MyUseable.MyCooldown * Time.deltaTime;

            if (imageCooldown.fillAmount <= 0)
            {
                imageCooldown.fillAmount = 0;
                MyUseable.OnCooldown = false;
            }
        }
    }

    public void SetUseable(IUseable useable)
    {
        if (useable is Item)
        {
            // Feeding the specific item to get all of the same kind
            MyUseables = InventoryScript.MyInstance.GetUseables(useable);

            if (InventoryScript.MyInstance.FromSlot != null)
            {
                InventoryScript.MyInstance.FromSlot.MyCover.enabled = false;
                InventoryScript.MyInstance.FromSlot.MyIcon.enabled = true;
                InventoryScript.MyInstance.FromSlot = null;
            }            
        }
        else
        {
            MyUseables.Clear();
            this.MyUseable = useable;
        }

        count = MyUseables.Count;
        UpdateVisual(useable as IMoveable);
        UIManager.MyInstance.RefreshTooltip(MyUseable as IDescribable);
    }

    public void UpdateVisual(IMoveable moveable)
    {
        if (HandScript.MyInstance.MyMoveable != null)
        {
            HandScript.MyInstance.Drop();
        }

        MyIcon.sprite = moveable.MyIcon;
        MyIcon.enabled = true;

        if (count > 0)
        {
            UIManager.MyInstance.UpdateStackSize(this);
        }
        else if (MyUseable is Spell)
        {
            UIManager.MyInstance.ClearStackCount(this);
        }
    }

    public void UpdateItemCount(Item item)
    {
        if (item is IUseable && MyUseables.Count > 0)
        {
            if (MyUseables.Peek().GetType() == item.GetType())
            {
                MyUseables = InventoryScript.MyInstance.GetUseables(item as IUseable);

                count = MyUseables.Count;
                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        IDescribable tmp = null;
        // Showing tooltip
        if (MyUseable != null && MyUseable is IDescribable)
        {
            tmp = (IDescribable)MyUseable;
            //UIManager.MyInstance.ShowTooltip(transform.position);
        }
        else if (MyUseables.Count > 0)
        {
            //UIManager.MyInstance.ShowTooltip(transform.position);
        }
        if (tmp != null)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1, 0), transform.position, tmp);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hiding tooltip
        UIManager.MyInstance.HideTooltip();
    }
}
