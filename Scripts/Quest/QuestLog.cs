using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questParent;

    private Quest selected;

    [SerializeField]
    //private Text questDescription;
    private TextMeshProUGUI questDescription;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    //private Text questCountTxt;
    private TextMeshProUGUI questCountTxt;

    [SerializeField]
    private int maxCount;

    private int currentCount;

    private List<QuestScript> questScripts = new List<QuestScript>();

    private List<Quest> quests = new List<Quest>();
    
    private static QuestLog instance;

    public static QuestLog MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestLog>();
            }
            return instance;
        }
    }

    public List<Quest> MyQuests { get => quests; set => quests = value; }

    private void Start()
    {
        questCountTxt.text = currentCount + "/" + maxCount;
    }

    public void AcceptQuest(Quest quest)
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            questCountTxt.text = currentCount + "/" + maxCount;

            // Checking all object that we have to collect from the quest
            // We are recognising each one as o
            foreach (CollectObjective o in quest.MyCollectObjectives)
            {
                // Assigning item count function to the itemcountchangedevent
                InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(o.UpdateItemCount);
                o.UpdateItemCount();
            }
            foreach (KillObjective o in quest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmedEvent += new KillConfirmed(o.UpdateKillCount);
            }

            MyQuests.Add(quest);

            // Creating a quest script
            GameObject go = Instantiate(questPrefab, questParent);

            // Getting the component from the quest
            QuestScript qs = go.GetComponent<QuestScript>();

            // Making sure questscript has a reference to function ref quest and VICE VERSA
            quest.MyQuestScript = qs;
            qs.MyQuest = quest;
            questScripts.Add(qs);
            //go.GetComponent<Text>().text = quest.MyTitle;
            go.GetComponent<TextMeshProUGUI>().text = quest.MyTitle;

            CheckCompletion();
        }        
    }

    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.Deselect();
            }

            string objectives = string.Empty;

            selected = quest;

            string title = quest.MyTitle;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }
            foreach (Objective obj in quest.MyKillObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            questDescription.text = string.Format("<b>{0}</b>\n<size=12>{1}</size>\n\n<b>Objectives</b>\n{2}", title, quest.MyDescription, objectives);

        }
    }



    public void CheckCompletion()
    {
        foreach (QuestScript qs in questScripts)
        {
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
            qs.IsComplete();
        }
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha == 1 )
        {
            Close();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void AbandonQuest()
    {
        foreach (CollectObjective o in selected.MyCollectObjectives)
        {
            // Unassigning event on completed quest
            InventoryScript.MyInstance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
        }
        foreach (KillObjective o in selected.MyKillObjectives)
        {
            // Unassigning event on completed quest
            GameManager.MyInstance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);
        }
        RemoveQuest(selected.MyQuestScript);
    }

    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        MyQuests.Remove(qs.MyQuest);
        questDescription.text = string.Empty;
        selected = null; // Deselecting the quest
        currentCount--;
        questCountTxt.text = currentCount + "/" + maxCount;
        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
        qs = null;
    }

    public bool HasQuest(Quest quest)
    {
        return MyQuests.Exists(x => x.MyTitle == quest.MyTitle);
    }
}
