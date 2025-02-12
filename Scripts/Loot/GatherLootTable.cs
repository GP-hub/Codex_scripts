﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherLootTable : LootTable, IInteractable
{
    [SerializeField]
    private GameObject openChest, closedChest;

    [SerializeField]
    private GameObject gatherIndicator;

    private void Start()
    {
        RollLoot();
    }


    protected override void RollLoot()
    {
        MyDroppedItems = new List<Drop>();
        foreach (Loot l in loot)
        {
            int roll = Random.Range(0, 100);

            if ( roll <= l.MyDropChance)
            {
                int itemCount = Random.Range(1, 6);
                for (int i = 0; i < itemCount; i++)
                {
                    MyDroppedItems.Add(new Drop(Instantiate(l.MyItem), this));
                }
                // GATHER SPRITE IF 2D
                gatherIndicator.SetActive(true);

            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Interact()
    {
        // NEED TO REFLECT SPELLBOOK SKILL
        Player.MyInstance.Gather(SpellBook.MyInstance.GetSpell("Gather"), MyDroppedItems);
        LootWindow.MyInstance.MyInteractable = this;
    }

    public void StopInteract()
    {
        LootWindow.MyInstance.MyInteractable = null;
        if (MyDroppedItems.Count == 0)
        {
            // GATHER SPRITE IF 2D
            gameObject.SetActive(false);
            gatherIndicator.SetActive(false);
        }

        LootWindow.MyInstance.Close();
    }
}
