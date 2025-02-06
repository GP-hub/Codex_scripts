using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static bool GameIsPaused = false;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private TextMeshProUGUI[] actionButtonsShortcut;

    [SerializeField]
    public CanvasGroup[] menus;

    [SerializeField]
    private GameObject targetFrame;

    private Stats healthStat;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Image completionBar;
    private float maxCompletion = 100;
    private float currentCompletion = 0;

    [SerializeField]
    private Image portraitFrame;

    [SerializeField]
    public GameObject loadingScreen;

    [SerializeField]
    private GameObject tooltip;

    //private Text tooltipText;
    private TextMeshProUGUI tooltipText;

    [SerializeField]
    private RectTransform tooltipRect;

    [SerializeField]
    private CharacterPanel charPanel;

    private GameObject[] keybindButtons;

    [SerializeField]
    public CanvasGroup portal_UI;

    [Space(10), SerializeField]
    public PowerCard[] powerCard;



    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        tooltipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Clearing hand from items, spells etc..
            if (HandScript.MyInstance.MyMoveable != null)
            {
                if (InventoryScript.MyInstance.FromSlot != null)
                {
                    InventoryScript.MyInstance.FromSlot.MyCover.enabled = false;
                    InventoryScript.MyInstance.FromSlot.MyIcon.enabled = true;
                    InventoryScript.MyInstance.FromSlot = null;
                }

                HandScript.MyInstance.Drop();
            }

            // Closing all menus
            foreach (CanvasGroup menus in menus)
            {
                if (menus.name != "MainMenu")
                {
                    CloseSingle(menus);
                }                            
            }

            OpenClose(menus[0]);

            if (menus[0].blocksRaycasts == true && menus[0].alpha == 1) 
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
        if (!GameIsPaused)
        {

            if (Input.GetKeyDown(KeyCode.C))
            {
                // Inventory
                InventoryScript.MyInstance.OpenClose();
                // Character panel
                OpenClose(menus[1]);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                // QuestLog panel
                OpenClose(menus[2]);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                // Spellbook
                OpenClose(menus[3]);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                // Inventory
                InventoryScript.MyInstance.OpenClose();
                // Character panel
                OpenClose(menus[1]);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                // Crafting
                OpenClose(menus[6]);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                // Spellbook
                OpenClose(menus[7]);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                // SpellUpgrade
                OpenClose(menus[8]);
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // Map overlay
                OpenClose(menus[9]);
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);

        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        portraitFrame.sprite = target.MyPortrait;

        levelText.text = target.MyLevel.ToString();

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

        if (target.MyLevel >= Player.MyInstance.MyLevel + 5)
        {
            levelText.color = Color.red;
        }
        else if (target.MyLevel == Player.MyInstance.MyLevel + 3 || target.MyLevel == Player.MyInstance.MyLevel + 4)
        {
            levelText.color = new Color32(255, 124, 0, 255);
        }
        else if (target.MyLevel >= Player.MyInstance.MyLevel + 2 || target.MyLevel >= Player.MyInstance.MyLevel - 2)
        {
            levelText.color = Color.yellow;
        }
        else if (target.MyLevel <= Player.MyInstance.MyLevel - 3 && target.MyLevel > XPManager.CalculateGrayLevel())
        {
            levelText.color = Color.green;
        }
        else
        {
            levelText.color = Color.grey;
        }
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();

        if (key.Contains("ACT"))
        {
            char c = key[3];

            int i;
            int.TryParse("" + c, out i);

            TextMeshProUGUI tmpPro = actionButtonsShortcut[i-1].GetComponent<TextMeshProUGUI>();

            if (code.ToString().Contains("Alpha"))
            {
                char l = code.ToString()[5];
                tmpPro.text = l.ToString(); ;
            }
            else
            {
                tmpPro.text = code.ToString();
            }
        }
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void OpenClose (CanvasGroup canvasGroup)
    {
        // If larger than 0 set to 0, if not, set to 1
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        // If true, set to false, if false, set to true
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void OpenSingle(CanvasGroup canvasGroup)
    {
        foreach (CanvasGroup canvas in menus)
        {
            CloseSingle(canvas);
        }

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void CloseSingle(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.enabled = true;
            clickable.MyIcon.enabled = true;
        }
        else
        {
            clickable.MyStackText.enabled = false;
            clickable.MyIcon.enabled = true;
        }
        if (clickable.MyCount == 0)
        {
            clickable.MyIcon.enabled = false;
            clickable.MyStackText.enabled = false;
            clickable.MyIcon.sprite = null;
        }
    }

    public void ClearStackCount(IClickable clickable)
    {
        clickable.MyStackText.enabled = false;
        clickable.MyIcon.enabled = true;
    }

    public void ShowTooltip(Vector2 pivot,Vector3 position, IDescribable description)
    {
        tooltipRect.pivot = pivot;
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltipText.text = description.GetDescription();
    }
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void RefreshTooltip(IDescribable description)
    {
        tooltipText.text = description.GetDescription();
    }

    public void Load()
    {
        LoaderManager.Load(LoaderManager.Scene.MainMenu);
    }

    public void Quit()
    {
        Debug.Log("Quit!");
        EditorApplication.isPlaying = false;
        Application.Quit();
    }


    public void GetCompletion(int completion)
    {
        if (!completionBar.transform.parent.gameObject.activeSelf)
        {
            completionBar.transform.parent.gameObject.SetActive(true);
            //completionBar.fillAmount += completion / maxCompletion;
            completionBar.fillAmount = currentCompletion;
        }
        else
        {
            completionBar.fillAmount += completion / maxCompletion;
            if (completionBar.fillAmount == 1)
            {
                // Function for game manager to call of boss spawn etc ...
                //GameManager.MyInstance.StartBoss();
                GameManager.MyInstance.StartBoss();
            }
        }
    }

    public void LoadingScreen()
    {
        if (!loadingScreen.activeSelf)
        {
            loadingScreen.SetActive(true);
        }
        else
        {
            loadingScreen.SetActive(false);
        }
    }

    public void TESTING_BUTTONS()
    {
        Debug.Log("button pressed" + Player.MyInstance.MyGold);
    }

}
