using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    private static KeybindManager instance;
    public static KeybindManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
            }
            return instance;
        }
    }

    public Dictionary<string, KeyCode> Keybinds { get; private set; }
    public Dictionary<string, KeyCode> ActionBinds { get; private set; }

    private string bindName;
    
    // Start is called before the first frame update
    void Start()
    {
        Keybinds = new Dictionary<string, KeyCode>();
        ActionBinds = new Dictionary<string, KeyCode>();

        BindKey("MOVE", KeyCode.Mouse0);
        BindKey("PLACEHOLDER", KeyCode.Mouse1);

        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);
        BindKey("ACT4", KeyCode.Alpha4);

        BindKey("ACT5", KeyCode.Q);

        BindKey("ITEM", KeyCode.LeftAlt);
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionnary = Keybinds;

        if (key.Contains("ACT"))
        {
            currentDictionnary = ActionBinds;
        }
        if (!currentDictionnary.ContainsKey(key))
        {
            currentDictionnary.Add(key, keyBind);
            UIManager.MyInstance.UpdateKeyText(key, keyBind);
        }
        else if (currentDictionnary.ContainsValue(keyBind))
        {
            string myKey = currentDictionnary.FirstOrDefault(x => x.Value == keyBind).Key;
            currentDictionnary[myKey] = KeyCode.None;
            UIManager.MyInstance.UpdateKeyText(key, KeyCode.None);
        }
        currentDictionnary[key] = keyBind;
        UIManager.MyInstance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }
    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }

    private void OnGUI()
    {
        if (bindName != string.Empty)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
