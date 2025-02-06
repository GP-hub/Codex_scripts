using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Spell : IUseable, IMoveable, IDescribable, ICastable
{
    [SerializeField]
    private string title;

    [SerializeField]
    private float damageMin;

    [SerializeField]
    private float damageMax;

    [SerializeField]
    private float duration;

    [SerializeField]
    private float range;

    [SerializeField]
    private float lifetime;

    [SerializeField]
    private bool needsTarget;

    [SerializeField]
    private int manaCost; 

    public enum SpellType { self, AOE_self, At_cursor, thrown };
    public enum SpellElem { Fire, Electricity, Frost, Corrosive };

    [SerializeField]
    private SpellType spellType;

    [SerializeField]
    private SpellElem spellElem;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float size;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private float cooldown;

    private bool onCooldown = false;

    [SerializeField]
    private GameObject spellPrefab;

    [SerializeField]
    private GameObject alternateSpellPrefab;

    [SerializeField]
    private string description;

    [SerializeField]
    private Color barColor;

    [SerializeField]
    private bool isDot;

    public Debuff MyDebuff { get; set; }
    public string MyTitle { get => title;}
    public float MyDamageMin { get => Mathf.Ceil(damageMin); set => damageMin = value; }
    public float MyDamageMax { get => Mathf.Ceil(damageMax); set => damageMax = value; }
    public Sprite MyIcon { get => icon;}
    public float MySpeed { get => Speed; set => Speed = value; }
    public float MySize { get => Size;}
    public float MyCastTime { get => castTime; set => castTime = value; }
    public GameObject MySpellPrefab { get => spellPrefab; set => spellPrefab = value; }
    public Color MyBarColor { get => barColor;}
    public float MyRange { get => range; set => range = value; }
    public bool NeedsTarget { get => needsTarget; }
    public float MyDuration { get => duration; set => duration = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Size { get => size; set => size = value; }
    public SpellType MySpellType { get => spellType;}
    public SpellElem MySpellElem { get => spellElem;}
    public float MyCooldown { get => cooldown; set => cooldown = value; }
    public bool OnCooldown { get => onCooldown; set => onCooldown = value; }
    public bool IsDot { get => isDot; set => isDot = value; }
    public float MyLifetime { get => lifetime; set => lifetime = value; }
    public int MyManaCost { get => manaCost; set => manaCost = value; }

    public string GetDescription()
    {
        if (!needsTarget)
        {
            return $"{title}<color=#ffd100>\n{description}\nthat does {damageMin / MyDuration} {MySpellElem} damage every second for {MyDuration} second</color>";
        }
        else
        {
            return string.Format("{0}\nCast time: {1} second(s)\n<color=#ffd111>{2}\nthat deals {3}{4} damages</color>", title, castTime, description, MyDamageMin, MySpellElem);

        }
    }

    public void Use()
    {
        if (!OnCooldown)
        {            
            Player.MyInstance.CastSpell(this);
        }
        else
        {
            Debug.Log("ON COOLDOWN");
        }
        
    } 
}