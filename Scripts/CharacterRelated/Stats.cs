using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stats : MonoBehaviour
{
    private Image content;

    //[SerializeField]
    //private Text statValue;

    [SerializeField]
    private TextMeshProUGUI statValue;

    [SerializeField]
    private bool NoMaxValue;

    private List<int> modifiers = new List<int>();

    [SerializeField]
    private float LerpSpeed;

    private float CurrentFill;

    private float overflow;
    public float MyMaxValue { get; set; }

    public bool IsFull
    {
        get
        {
            return content.fillAmount == 1;
        }
    }

    public float MyOverflow
    {
        get
        {
            float tmp = overflow;
            overflow = 0;
            return tmp;
        }
    }

    private float currentValue;
    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }

        set 
        {
            //if (currentValue == MyMaxValue)
            //{
            //    currentValue += value;
            //    MyMaxValue = currentValue;
            //    //Debug.Log("stats check");
            //}
            if (value > MyMaxValue)
            {
                overflow = value - MyMaxValue;
                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            CurrentFill = currentValue / MyMaxValue;

            if (statValue != null)
            {
                if (NoMaxValue == true)
                {
                    statValue.text = currentValue.ToString();
                }
                else
                {
                    statValue.text = currentValue + "/" + MyMaxValue;
                }                
            }
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
    }


    // Update is called once per frame
    void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        if (CurrentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, CurrentFill, Time.deltaTime * LerpSpeed);
        }
    }
    public void Initialize(float currentValue)
    {
        MyMaxValue = int.MaxValue;
        MyCurrentValue = currentValue;        
    }

    public void Initialize (float currentValue, float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }

        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    public void Reset()
    {
        content.fillAmount = 0;
    }

    public void AddModifier(int modifier)
    {
        modifiers.Add(modifier);
        MyCurrentValue = modifiers.Sum();
    }

    public void RemoveModifier(int modifier)
    {
        modifiers.Remove(modifier);
        MyCurrentValue = modifiers.Sum();
    }

    //public void AddPower(int power)
    //{
    //    Debug.Log(power);
    //    powers.Add(power);
    //}

    //public void RemovePower(int power)
    //{
    //    powers.Remove(power);
    //    Debug.Log("powers : " + powers);
    //}
}
