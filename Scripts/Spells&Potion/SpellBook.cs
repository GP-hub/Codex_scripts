using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellBook : MonoBehaviour
{
    private static SpellBook instance;

    public static SpellBook MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpellBook>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Image castingBar;

    [SerializeField]
    private TextMeshProUGUI currentSpell;

    [SerializeField]
    private TextMeshProUGUI castTime;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Spell[] spells;

    private Coroutine spellRoutine;

    private Coroutine fadeRoutine;

    [SerializeField]
    private GameObject[] obtainableSpells;

    public void Cast(ICastable castable)
    {
        //Spell spell = Array.Find(spells, x => x.MyName == spellName);
        castingBar.fillAmount = 0;
        castingBar.color = castable.MyBarColor;

        currentSpell.text = castable.MyTitle;
        icon.sprite = castable.MyIcon;

        spellRoutine = StartCoroutine(Progress(castable));

        fadeRoutine = StartCoroutine(FadeBar());
    }
    
    private IEnumerator Progress(ICastable castable)
    {
        float timePassed = Time.deltaTime;
        float rate = 1.0f / castable.MyCastTime;
        float progress = 0.0f;

        while (progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            timePassed += Time.deltaTime;
            castTime.text = ((castable.MyCastTime - timePassed).ToString("F1") + "/" + castable.MyCastTime).ToString();
            if (castable.MyCastTime - timePassed < 0)
            {
                castTime.text = "0.0";
            }
            yield return null;
        }
        stopCasting();
    }

    private IEnumerator FadeBar()
    {
        float rate = 1.0f / 0.15f;
        float progress = 0.0f;

        while (progress <= 1.0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }
    }
    public void stopCasting()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }
        if (spellRoutine != null)
        {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }

    public void LearnSpell(string name)
    {
        switch (name.ToLower())
        {
            case "rainoffire":
                obtainableSpells[0].SetActive(true);
                break;
            case "blizzard":
                obtainableSpells[1].SetActive(true);
                break;
            case "chainlightning":
                obtainableSpells[2].SetActive(true);
                break;
        }
    }

    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyTitle == spellName);
        return spell;
    }

    public Spell CheckSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyTitle == spellName);
        return spell;
    }

    public void Close()
    {
        GetComponentInParent<CanvasGroup>().alpha = 0;
        GetComponentInParent<CanvasGroup>().blocksRaycasts = false;
    }
}
