using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalentTree : MonoBehaviour
{
    [SerializeField]
    private int points = 10;

    [SerializeField]
    private Talent[] talents;

    [SerializeField]
    private Talent[] unlockedByDefault;

    [SerializeField]
    private TextMeshProUGUI talentPoinText;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public int MyPoints
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
            UpdateTalentPointText();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetTalents();
    }

    public void TryUseTalent(Talent talent)
    {
        if (MyPoints > 0 && talent.Click())
        {
            MyPoints--;
        }
        // If we dont have points to spend
        if (MyPoints == 0)
        {
            // We check all talents
            foreach (Talent t in talents)
            {
                // If there is no point spent into them, we lock it
                if (t.MyCurrentCount == 0)
                {
                    t.Lock();
                }
            }
        }
    }


    private void ResetTalents()
    {
        UpdateTalentPointText();

        foreach (Talent talent in talents)
        {
            talent.Lock();
        }

        foreach (Talent talent in unlockedByDefault)
        {
            talent.Unlock();
        }
    }

    private void UpdateTalentPointText()
    {
        talentPoinText.text = points.ToString();
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
