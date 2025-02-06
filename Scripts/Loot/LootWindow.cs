using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    private static LootWindow instance;
    public static LootWindow MyInstance
    { 
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<LootWindow>();
            }
            return instance;
        }
    }

    [SerializeField]
    private LootButton[] lootButtons;

    private CanvasGroup canvasGroup;

    private List<List<Drop>> pages = new List<List<Drop>>();

    private List<Drop> droppedLoot = new List<Drop>();

    private int pageIndex = 0;

    [SerializeField]
    private Text pageNumber;

    [SerializeField]
    private GameObject nextButton, previousButton;

    public IInteractable MyInteractable { get; set; }

    // FOR DEBUG ONLY
    [SerializeField]
    private Item[] items;

    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    // List is an object with a reference type
    public void CreatePages(List<Drop> items)
    {
        if (!IsOpen)
        {
            List<Drop> page = new List<Drop>();

            // If changes are made to droppedloot, it will change items
            droppedLoot = items;

            for (int i = 0; i < items.Count; i++)
            {
                page.Add(items[i]);

                // Making new page every 4 item
                // Making sure we making new page even if less than 4 items left
                if (page.Count == 4 || i == items.Count - 1)
                {
                    // Adding the page to the list
                    pages.Add(page);
                    // Creating a new page
                    page = new List<Drop>();
                }
            }

            AddLoot();

            Open();
        }
    }

    private void AddLoot()
    {
        if (pages.Count > 0)
        {
            // Handle page numbers
            pageNumber.text = pageIndex + 1 + "/" + pages.Count;
            // Handle next and previous buttons
            previousButton.SetActive(pageIndex > 0);
            nextButton.SetActive(pages.Count > 1 && pageIndex < pages.Count -1);

            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    // Set the loot buttons icon
                    lootButtons[i].MyIcon.sprite = pages[pageIndex][i].MyItem.MyIcon;

                    lootButtons[i].MyLoot = pages[pageIndex][i].MyItem;

                    // Make sure the loot buttons is visible
                    lootButtons[i].gameObject.SetActive(true);

                    // KEY LINE TO REFERENCE ITEM TITLE & COLOR
                    string title = string.Format("<color={0}> {1}</color>", QualityColor.MyColors[pages[pageIndex][i].MyItem.MyQuality], pages[pageIndex][i].MyItem.MyTitle);
                    // Set the title
                    lootButtons[i].MyTitle.text = title;
                }   
            }
        }
    }

    public void ClearButtons()
    {
        foreach (LootButton btn in lootButtons)
        {
            btn.gameObject.SetActive(false);
        }    
    }

    public void NextPage()
    {
        // We check if we have more pages
        if (pageIndex < pages.Count -1)
        {
            pageIndex++;
            ClearButtons();
            AddLoot();
        }
    }

    public void PreviousPage()
    {
        // Checking if we have more pages to go backwards to
        if (pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }
    }

    public void TakeLoot(Item loot)
    {

        Drop drop = pages[pageIndex].Find(x => x.MyItem == loot);

        pages[pageIndex].Remove(drop);

        drop.Remove();

        // We will also have to remove the loot from the loot table

        if (pages[pageIndex].Count == 0)
        {
            // Remove the empty page
            pages.Remove(pages[pageIndex]);
            // If we are at last count, we are not on first page
            if (pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }

            AddLoot();
        }
    }
    public void Close()
    {
        while (pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }

        // THIS IS THE FIX
        pageIndex = 0;

        pages.Clear();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        ClearButtons();

        if (MyInstance != null)
        {
            // INTERACT ENEMY
            //MyInteractable.StopInteract();
        }

        MyInteractable = null;
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
}
