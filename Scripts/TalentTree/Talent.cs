﻿using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Talent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDescribable
{
    private Image sprite;

    [SerializeField]
    private Text countText;

    [SerializeField]
    private int maxCount;

    private int currentCount;

    [SerializeField]
    private bool unlocked;

    [SerializeField]
    private Talent childTalent;

    [SerializeField]
    private Sprite arrowSpriteLocked;

    [SerializeField]
    private Sprite arrowSpriteUnlocked;

    [SerializeField]
    private Image arrowImage;

    public int MyCurrentCount { get => currentCount; set => currentCount = value; }

    private void Awake()
    {
        sprite = GetComponent<Image>();

        countText.text = $"{MyCurrentCount}/{maxCount}";

        if (unlocked)
        {
            Unlock();  
        }        
    }

    public virtual bool Click()
    {
        if (MyCurrentCount < maxCount && unlocked)
        {
            MyCurrentCount++;
            countText.text = $"{MyCurrentCount}/{maxCount}";

            if (MyCurrentCount == maxCount)
            {
                if (childTalent != null)
                {
                    childTalent.Unlock();
                }
            }
            
            return true;
        }

        return false;
    }

    public void Lock()
    {
        sprite.color = Color.gray;
        countText.color = Color.gray;

        if (arrowImage != null)
        {
            arrowImage.sprite = arrowSpriteLocked;
        }
        if (countText != null)
        {
            countText.color = Color.gray;
        }
    }

    public void Unlock()
    {
        sprite.color = Color.white;
        countText.color = Color.white;

        if (arrowImage != null)
        {
            arrowImage.sprite = arrowSpriteUnlocked;
        }
        if (countText != null)
        {
            countText.color = Color.white;
        }
        unlocked = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.MyInstance.ShowTooltip(new Vector2(1, 0), transform.position, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

    public virtual string GetDescription()
    {
        return string.Empty;
    }
}
