using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField]
    private Quest[] quests;

    [SerializeField]
    private GameObject question, questionSilver, exclamation;
    
    [SerializeField]
    private Sprite mini_question, mini_questionSilver, mini_exclamation;

    [SerializeField]
    private int questGiverID;

    private List<string> completedQuests = new List<string>();

    [SerializeField]
    private SpriteRenderer minimapRenderer;
    public Quest[] MyQuests { get => quests; }
    public int MyQuestGiverID { get => questGiverID; }
    public List<string> MyCompletedQuests
    {
        get
        {
            return completedQuests;
        }
        set
        {
            completedQuests = value;

            foreach (string title in completedQuests)
            {
                for (int i = 0; i < quests.Length; i++)
                {
                    if (quests[i] != null && quests[i].MyTitle == title)
                    {
                        quests[i] = null;
                    }
                }
            }
        }

    }

    private void Start()
    {
        foreach (Quest quest in quests)
        {
            if (quest != null)
            {
                quest.MyQuestGiver = this;
            }
        }

        MyAnimator = GetComponent<Animator>();
        InvokeRepeating("RandomIdle", 2f, Random.Range(10, 20));
    }

    public void UpdateQuestStatus()
    {
        int count = 0;
        // Running through all the quest
        foreach (Quest quest in quests)
        {
            // If theres some quest
            if (quest != null)
            {
                // Checking if its complete AND if the player has the quest
                if (quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
                {
                    question.SetActive(true);
                    questionSilver.SetActive(false);
                    exclamation.SetActive(false);
                    minimapRenderer.sprite = mini_question;

                    break;
                }
                // If we dont have any completed quest, if quest giver has a quest we dont have already
                else if (!QuestLog.MyInstance.HasQuest(quest))
                {
                    question.SetActive(false);
                    questionSilver.SetActive(false);
                    exclamation.SetActive(true);
                    minimapRenderer.sprite = mini_exclamation;

                    break;
                }
                // Player on the quest but didnt complete yet
                else if (!quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
                {
                    question.SetActive(false);
                    questionSilver.SetActive(true);
                    exclamation.SetActive(false);
                    minimapRenderer.sprite = mini_questionSilver;

                    break;
                }
            }
            else
            {
                count++;

                if (count == quests.Length)
                {
                    question.SetActive(false);
                    questionSilver.SetActive(false);
                    exclamation.SetActive(false);
                    minimapRenderer.enabled = false;
                }
            }
        }
    }
}
