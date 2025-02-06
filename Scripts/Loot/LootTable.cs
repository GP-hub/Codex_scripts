using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LootTable : MonoBehaviour
{
    [SerializeField]
    protected Loot[] loot;

    protected Coroutine distanceRoutine;

    private ColorBlock lootColor;
    public List<Drop> MyDroppedItems { get; set; }

    private bool rolled = false;
    public List<Drop> GetLoot()
    {
        if (!rolled)
        {
            MyDroppedItems = new List<Drop>();
            RollLoot();
        }
        return MyDroppedItems;
    }

    protected virtual void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);

            if (roll <= item.MyDropChance)
            {
                MyDroppedItems.Add(new Drop(item.MyItem, this));

                item.MyItem.MyItemGameObject.GetComponent<ItemButton>().itemButtonTextMP.text = item.MyItem.MyTitle;
                item.MyItem.MyItemGameObject.GetComponent<ItemButton>().itemButtonTextMP.color = Color.black;

                Color qualityColor;
                ColorUtility.TryParseHtmlString(QualityColor.MyColors[item.MyItem.MyQuality], out qualityColor);
                qualityColor.a = .5f;
                item.MyItem.MyItemGameObject.GetComponent<ItemButton>().itemButtonImage.color = qualityColor;

                item.MyItem.MyItemGameObject.GetComponent<ItemButton>().itemToLoot = item.MyItem;

                Vector3 point = transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(2f, -2f));
                
                // Make sure the location is on the NavMesh
                NavMeshHit hit;
                if (NavMesh.SamplePosition(point, out hit, 100, 1))
                {
                    point = hit.position;
                }

                Instantiate(item.MyItem.MyItemGameObject, point, Quaternion.identity);
                
            }
        }

        rolled = true;
    }
    public void Interact(GameObject itemGo)
    {
        // Take item
        InventoryScript.MyInstance.AddItem(Instantiate(itemGo.GetComponent<ItemButton>().itemToLoot));
        // Destroy ground item after its been taken
        Destroy(transform.root.gameObject);
    }

    public void GoToItem(GameObject itemGo)
    {          
        float dist = Vector3.Distance(Player.MyInstance.transform.position, itemGo.transform.position);           

        if (distanceRoutine != null)
        {
            StopCoroutine(distanceRoutine);
        }        

        if (dist >= 1.5f && Player.MyInstance.IsAlive)
        {
            Player.MyInstance.SetDestination();
            distanceRoutine = StartCoroutine(Distance(Player.MyInstance, itemGo));
        }

        else
        {
            Interact(itemGo);            
        }
    }

    IEnumerator Distance(Player player, GameObject itemGo)
    {
        float dist = Vector3.Distance(Player.MyInstance.transform.position, itemGo.transform.position);

        while (dist >= 1.5f)
        {
            dist = Vector3.Distance(player.transform.position, itemGo.transform.position);
            yield return dist;
        }
        player.StopMovement();
        Interact(itemGo);
        yield break;
    }

    public void TestButton()
    {
        Debug.Log("You clicked the button");
    }
}
