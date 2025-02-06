using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestGiverWindow : Window
{
    private static QuestGiverWindow instance;

    [SerializeField]
    private GameObject backBtn, acceptBtn, completeBtn, questDescription;

    private QuestGiver questGiver;

    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questArea;

    private List<GameObject> quests = new List<GameObject>();

    private Quest selectedQuest;

    public static QuestGiverWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestGiverWindow>();
            }
            return instance;
        }
    }

    public void ShowQuest(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach (GameObject go in quests)
        {
            Destroy(go);
        }

        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);

        foreach (Quest quest in questGiver.MyQuests)
        {
            if (quest != null)
            {
                GameObject go = Instantiate(questPrefab, questArea);
                //go.GetComponent<Text>().text =  "[" + quest.MyLevel + "]" + quest.MyTitle + "<color=#ffbb04> !</color>";             
                go.GetComponent<TextMeshProUGUI>().text =  "[" + quest.MyLevel + "]" + quest.MyTitle + "<color=#ffbb04> !</color>";             
                go.GetComponent<QGQuestScript>().MyQuest = quest;
                quests.Add(go);

                if (QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
                {
                    //go.GetComponent<Text>().text = quest.MyTitle + "<color=#ffbb04> ?</color>";
                    go.GetComponent<TextMeshProUGUI>().text = quest.MyTitle + "<color=#ffbb04> ?</color>";
                }
                else if (QuestLog.MyInstance.HasQuest(quest))
                {
                    //Color c = go.GetComponent<Text>().color;
                    Color c = go.GetComponent<TextMeshProUGUI>().color;
                    c.a = 0.5f;
                    //go.GetComponent<Text>().color = c;
                    go.GetComponent<TextMeshProUGUI>().color = c;
                    //go.GetComponent<Text>().text = quest.MyTitle + "<color=#c0c0c0ff> ?</color>";
                    go.GetComponent<TextMeshProUGUI>().text = quest.MyTitle + "<color=#c0c0c0ff> ?</color>";
                }
            }            
        }
    }

    public override void Open(NPC npc)
    {
        ShowQuest((npc as QuestGiver));
        base.Open(npc);
    }

    public void ShowQuestInfo(Quest quest)
    {
        this.selectedQuest = quest;

        if (QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
        {
            acceptBtn.SetActive(false);
            completeBtn.SetActive(true);
        }
        else if (!QuestLog.MyInstance.HasQuest(quest))
        {
            acceptBtn.SetActive(true);
        }
        backBtn.SetActive(true);
        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);

        string objectives = string.Empty;

        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
        }

        //questDescription.GetComponent<Text>().text = string.Format("<b>{0}</b>\n<size=20>{1}</size>", quest.MyTitle, quest.MyDescription);
        questDescription.GetComponent<TextMeshProUGUI>().text = string.Format("<b>{0}</b>\n<size=20>{1}</size>", quest.MyTitle, quest.MyDescription);
    }

    public void Back()
    {
        backBtn.SetActive(false);
        acceptBtn.SetActive(false);
        ShowQuest(questGiver);
        completeBtn.SetActive(false);
    }

    public void Accept()
    {
        QuestLog.MyInstance.AcceptQuest(selectedQuest);
        Back();
    }

    public override void Close()
    {
        completeBtn.SetActive(false);
        base.Close();
    }

    public void CompleteQuest()
    {
        if (selectedQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.MyQuests.Length; i++)
            {
                if (selectedQuest == questGiver.MyQuests[i])
                {
                    questGiver.MyCompletedQuests.Add(selectedQuest.MyTitle);
                    questGiver.MyQuests[i] = null;
                    selectedQuest.MyQuestGiver.UpdateQuestStatus();
                }
            }
            foreach (CollectObjective o in selectedQuest.MyCollectObjectives)
            {
                // Unassigning event on completed quest
                InventoryScript.MyInstance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
                o.Complete();
            }
            foreach(KillObjective o in selectedQuest.MyKillObjectives)
            {
                // Unassigning event on completed quest
                GameManager.MyInstance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);
            }

            Player.MyInstance.GainXP(XPManager.CalculateXP(selectedQuest));

            QuestLog.MyInstance.RemoveQuest(selectedQuest.MyQuestScript);
            Back();
        }
    }
}