using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LootChest : LootTable, IInteractable
{
    [SerializeField]
    private GameObject openChest, closedChest;

    private bool isOpen;

    private List<Drop> items;

    protected override void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);

            if (roll <= item.MyDropChance)
            {                
                item.MyItem.MyItemGameObject.GetComponent<ItemButton>().itemButtonTextMP.text = item.MyItem.MyTitle;
                item.MyItem.MyItemGameObject.GetComponent<ItemButton>().itemButtonTextMP.color = Color.black;

                Color qualityColor;
                ColorUtility.TryParseHtmlString(QualityColor.MyColors[item.MyItem.MyQuality], out qualityColor);
                qualityColor.a = .5f;
                item.MyItem.MyItemGameObject.GetComponent<ItemButton>().itemButtonImage.color = qualityColor;

                item.MyItem.MyItemGameObject.GetComponent<ItemButton>().itemToLoot = item.MyItem;

                // Instantiating Loot on the ground                
                Instantiate(item.MyItem.MyItemGameObject, transform.position + (transform.forward * Random.Range(1, 5)), transform.rotation);               
            }
        }
    }

    public void Interact()
    {
        if (!isOpen)
        {
            RollLoot();
            isOpen = true;
            closedChest.SetActive(false);
            openChest.SetActive(true);
        }
    }

    public void StopInteract()
    {
        //if (isOpen)
        //{
        //    StopInteract();
        //}
    }

}
