using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerOption : MonoBehaviour
{
    [SerializeField] public Button button;
    [SerializeField] public TextMeshProUGUI powerName;
    [SerializeField] public TextMeshProUGUI powerDescription;
    [SerializeField] public Image powerIcon;
    [HideInInspector] public PowerCard powerCard;

    public void UpdatePowerCard()
    {
        powerName.text = powerCard.powerName;
        powerDescription.text = powerCard.powerDescription;
        powerIcon.sprite = powerCard.powerImage;
    }
}