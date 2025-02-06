using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

public class Player : Character
{
    private static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    //private List<Enemy> attackers = new List<Enemy>();
    [SerializeField]
    private CanvasGroup respawnCanvasGroup;

    [SerializeField]
    private float damageTakenMultiplier;

    [SerializeField]
    private float damageDoneMultiplier;

    [SerializeField]
    private float healthRegenMultiplier;

    [SerializeField]
    private Stats mana;

    [SerializeField]
    private Stats xpStat;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI goldText;

    [SerializeField] protected Stats fireDamage;
    [SerializeField] protected Stats frostDamage;
    [SerializeField] protected Stats lightningDamage;
    [SerializeField] protected Stats corrosiveDamage;


    [SerializeField] protected Stats fireStat;
    [SerializeField] protected Stats frostStat;
    [SerializeField] protected Stats lightningStat;
    [SerializeField] protected Stats corrosiveStat;

    [SerializeField] protected Powers legendaryPower;

    [SerializeField] protected List<CharButton> charSlots = new List<CharButton>();

    [SerializeField] public List<string> spellList = new List<string>();


     
    [SerializeField]
    private float initialMana;

    [SerializeField]
    private Vector3 initialPosition;

    [SerializeField]
    private AnimationClip castingAnim;

    private float lengthAnim;

    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private LayerMask losMask;

    [SerializeField]
    private Transform exitPoint;

    [SerializeField]
    private GameObject levelUp;

    [SerializeField]
    private Profession profession;

    private GameObject unusedSpell;

    private Spell aoeSpell;

    [SerializeField]
    private Camera mainCamera;

    [Space(10), SerializeField]
    private GameObject portal;

    private int exitIndex = 0;

    public Coroutine MyInitRoutine { get; set; }
    public Coroutine MyCooldownRoutine { get; set; }

    private List<IInteractable> interactables = new List<IInteractable>();

    private Vector3 min, max;

    public int MyGold { get; set; }

    private bool wantToRespawn = false;
    public bool InCombat { get; set; } = false;
    public List<IInteractable> MyInteractables { get => interactables; set => interactables = value; }
    public Stats MyXpStat { get => xpStat; set => xpStat = value; }
    public Stats MyMana { get => mana; set => mana = value; }
    //public List<Enemy> MyAttackers { get => attackers; set => attackers = value; }

    public Stats MyFireStat
    {
        get { return fireStat; }
    }
    public Stats MyFrostStat
    {
        get { return frostStat; }
    }
    public Stats MyLightningStat
    {
        get { return lightningStat; }
    }
    public Stats MyCorrosiveStat
    {
        get { return corrosiveStat; }
    }

    public Stats MyFireDamage
    {
        get { return fireDamage; }
    }
    public Stats MyFrostDamage
    {
        get { return frostDamage; }
    }
    public Stats MyLightningDamage
    {
        get { return lightningDamage; }
    }
    public Stats MyCorrosiveDamage
    {
        get { return corrosiveDamage; }
    }

    public float MyDamageTakenMultiplier { get => damageTakenMultiplier; set => damageTakenMultiplier = value; }
    public float MyDamageDoneMultiplier { get => damageDoneMultiplier; set => damageDoneMultiplier = value; }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Regen());
    }


    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        // SUPPOSED TO STICK THE UNUSED SPELL TO THE CURSOR TO DROP IT SOMEWHERE
        if (unusedSpell != null)
        {
            Debug.Log("UNUSED_SPELL");
            Vector3 mouseScreenPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            unusedSpell.transform.position = new Vector3(mouseScreenPosition.x, 0, mouseScreenPosition.z);

            float distance = Vector3.Distance(transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log("DISTANCE: " + distance);

            if (Input.GetMouseButton(0) && distance <= aoeSpell.MyRange)
            {
                Debug.Log("SPELL RANGE: " + aoeSpell.MyRange);
                AoESpell s = Instantiate(aoeSpell.MySpellPrefab, unusedSpell.transform.position, Quaternion.identity).GetComponent<AoESpell>();
                Destroy(unusedSpell);
                unusedSpell = null;

                s.Initialize(aoeSpell.MyDamageMin, aoeSpell.MyDuration, aoeSpell.IsDot);
                // consuming mana
                mana.MyCurrentValue -= aoeSpell.MyManaCost;
            }
        }

        base.Update();
    }


    public void SetDefaultValues()
    {
        MyGold = 100;
        health.Initialize(initialHealth, initialHealth);
        //float r = Random.value;
        //float g = Random.value;
        //float b = Random.value;
        ////Debug.Log(r + " " + g + " " + b);
        //// Changing character color 
        //charRenderer.SetColor("_Color", new Color(r, g, b));

        MyMana.Initialize(initialMana, initialMana);

        MyFireDamage.Initialize(0);
        MyFrostDamage.Initialize(0);
        MyLightningDamage.Initialize(0);
        MyCorrosiveDamage.Initialize(0);

        MyFireStat.Initialize(0);
        MyFrostStat.Initialize(0);
        MyLightningStat.Initialize(0);
        MyCorrosiveStat.Initialize(0);

        // XP FORMULA
        MyXpStat.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));

        levelText.text = MyLevel.ToString();
        goldText.text = (MyGold.ToString()/* + " Golds"*/);
    }
    private void GetInput()
    {
        ////////  TESTING HEALTH & MANA & XP  //// //// 
        ///
        if (Input.GetKey(KeyCode.Y))
        {
            SpawnBossPortal();
        }
        if (Input.GetKey(KeyCode.I))
        {
            health.MyCurrentValue += 100;
            MyMana.MyCurrentValue += 100;
        }
        if (Input.GetKey(KeyCode.O))
        {
            health.MyCurrentValue -= 100;
            MyMana.MyCurrentValue -= 100;
        }
        foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);
            }
        }
    }

    public void AttackValidation()
    {
        //Block();
        //Debug.Log(myTarget);
        //Debug.Log(IsAttacking);
        //Debug.Log(IsMoving);
        //Debug.Log(InLineOfSight);

        //if (myTarget != null && !IsAttacking && /*!IsMoving &&*/ InLineOfSight())
        //{
        //    attackRoutine = StartCoroutine(Attack());
        //}
    }

    private IEnumerator AttackRoutine(ICastable castable)
    {
        Transform currentTarget = MyTarget.MyHitBox;

        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        if (currentTarget != null && InLineOfSight())
        {
            Spell newSpell = SpellBook.MyInstance.GetSpell(castable.MyTitle);
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();            
            s.Iniatilize(currentTarget, newSpell.MyDamageMin, newSpell.MyDamageMax, this, newSpell.MyDebuff, newSpell);
            // consuming mana
            mana.MyCurrentValue -= newSpell.MyManaCost;
        }

        StopAction();

        foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);
            }
        }
    }

    private IEnumerator AttackRoutineNoTarget(ICastable castable)
    {
        StopAction();
        this.MyTarget = null;

        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("ground");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity/*, 1024*/, mask))
        {
            transform.LookAt(new Vector3(hit.point.x, 0f, hit.point.z));

            // +0.5 on X & Y to account for projectile Y position
            Vector3 currentTargetLocation = (new Vector3(hit.point.x + 0.5f, 0f, hit.point.z + 0.5f) - this.transform.position)/*.normalized*/;
            MyTargetPos = currentTargetLocation;
            //Debug.Log("currentTargetLocation/ NoTarget" + currentTargetLocation);

            yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

            // WE ARE ABLE TO THROW SPELL EVEN AT POINT WE CANT SEE, they will just get destroyed by walls

            if (currentTargetLocation != null /*&& InLineOfSight()*/)
            {
                Spell newSpell = SpellBook.MyInstance.GetSpell(castable.MyTitle);
                //Changer rotation from Quaternion.identity to transform rotation.
                SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoint.position, Quaternion.LookRotation(currentTargetLocation)).GetComponent<SpellScript>();
              
                s.Iniatilize(currentTargetLocation, newSpell.MyDamageMin, newSpell.MyDamageMax, newSpell.MySpeed, newSpell.MyLifetime, this, newSpell.MyDebuff, newSpell);
                // consuming mana
                mana.MyCurrentValue -= newSpell.MyManaCost;
            }

            foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
            {
                if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
                {
                    UIManager.MyInstance.ClickActionButton(action);
                }
            }
        }

        //yield return new WaitForSeconds(castingAnim.length - castable.MyCastTime);

        StopAction();
    }

    private IEnumerator AttackRoutineAoEself(ICastable castable)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        Spell newSpell = SpellBook.MyInstance.GetSpell(castable.MyTitle);
        SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();
        s.Initialize(this.transform.position, newSpell.MyDamageMin, newSpell.MyDamageMax, this, newSpell.MyDebuff, newSpell);
        // consuming mana
        mana.MyCurrentValue -= newSpell.MyManaCost;

        StopAction();

        foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);
            }
        }
    }

    private IEnumerator TestRaycast(ICastable castable)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity/*, 1024*/))
        {
            Debug.Log("bug here");
            yield return actionRoutine = StartCoroutine(ActionRoutine(castable));
        

            StopAction();

            foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
            {
                if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
                {
                    UIManager.MyInstance.ClickActionButton(action);
                }
            }
        }

    }

    private IEnumerator AttackRoutineAtCursor(ICastable castable)
    {
        this.MyTarget = null;

        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("ground");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // I dont remember what the bug is 
        Debug.Log("bug here");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity/*, 1024*/, mask))
        {
            transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
            Vector3 currentTargetLocation = new Vector3(hit.point.x, /*exitPoint.position.y*/0, hit.point.z);
            MyTargetPos = currentTargetLocation;

            //yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

            if (currentTargetLocation != null && InLineOfSight())
            {
                yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

                Spell newSpell = SpellBook.MyInstance.GetSpell(castable.MyTitle);               

                Debug.Log((this.transform.position - MyTargetPos).magnitude);
                //Debug.DrawRay(transform.position, MyTargetPos - transform.position, Color.red);

                AoESpell s = Instantiate(aoeSpell.MySpellPrefab, new Vector3(hit.point.x, 0, hit.point.z), Quaternion.identity).GetComponent<AoESpell>();
                s.Initialize(aoeSpell.MyDamageMin, aoeSpell.MyDuration, aoeSpell.IsDot);
                // consuming mana
                mana.MyCurrentValue -= newSpell.MyManaCost;

                Debug.Log("IF");

                // BEWARE, the TargetPos is calculated related to the character, thats why we calculate the distance starting from 0,0,0
                //if (Physics.Raycast(transform.position, MyTargetPos, out hit1))
                //{
                //    AoESpell s = Instantiate(aoeSpell.MySpellPrefab, new Vector3(hit1.point.x, 0, hit1.point.z), Quaternion.identity).GetComponent<AoESpell>();
                //    s.Initialize(aoeSpell.MyDamage, aoeSpell.MyDuration, aoeSpell.IsDot);
                //    Debug.Log("IF");
                //}
                //else
                //{
                //    AoESpell s = Instantiate(aoeSpell.MySpellPrefab, new Vector3(hit1.point.x, 0, hit1.point.z), Quaternion.identity).GetComponent<AoESpell>();
                //    s.Initialize(aoeSpell.MyDamage, aoeSpell.MyDuration, aoeSpell.IsDot);
                //    Debug.Log("ELSE");
                //}

                //SpellScript s = Instantiate(newSpell.MySpellPrefab, new Vector3(hit.point.x, 0, hit.point.z), Quaternion.identity).GetComponent<SpellScript>();

                //s.Initialize(currentTargetLocation, newSpell.MyDamage, this, newSpell.MyDebuff);
            }
            else
            {
                yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

                Spell newSpell = SpellBook.MyInstance.GetSpell(castable.MyTitle);

                //Debug.DrawRay(transform.position, MyTargetPos-transform.position, Color.red);

                RaycastHit hit1;
                Physics.Raycast(transform.position, MyTargetPos - transform.position, out hit1);

                AoESpell s = Instantiate(aoeSpell.MySpellPrefab, new Vector3(hit1.point.x, 0, hit1.point.z), Quaternion.identity).GetComponent<AoESpell>();
                s.Initialize(aoeSpell.MyDamageMin, aoeSpell.MyDuration, aoeSpell.IsDot);
                // consuming mana
                mana.MyCurrentValue -= newSpell.MyManaCost;
                Debug.Log("ELSE IF");
            }

            foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
            {
                if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
                {
                    UIManager.MyInstance.ClickActionButton(action);
                }
            }
        }        

        StopAction();        
    }

    private IEnumerator GatherRoutine(ICastable castable, List<Drop> items)
    {
        //Debug.Log(lengthAnim);

        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        StopAction();

        LootWindow.MyInstance.CreatePages(items);

        foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);
            }
        }
    }
   
    public IEnumerator CraftRoutine(ICastable castable)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        profession.AddItemsToInventory();    
    }


    private IEnumerator ActionRoutine(ICastable castable)
    {
        StopMovement();

        SpellBook.MyInstance.Cast(castable);        

        IsAttacking = true;

        if (MyTarget != null)
        {
            transform.LookAt(MyTarget.transform);
        }

        //lengthAnim = MyAnimator.GetCurrentAnimatorStateInfo(2).length / castable.MyCastTime;
        //lengthAnim = (castingAnim.length - 1.5f) / castable.MyCastTime;

        // LINE THAT MAKE THE ANIMATION THE LENGTH OF THE SPELL CASTING TIME
        lengthAnim = castingAnim.length;

        //Debug.Log(castingAnim.length);
        //Debug.Log(lengthAnim);
        //Debug.Log(castingAnim.length * castable.MyCastTime);

        MyAnimator.SetFloat("length", lengthAnim / castable.MyCastTime);
        MyAnimator.SetBool("attack", IsAttacking);

        // Fix to replaying the attack animation from the start, instead of continuing the potential ongoing one.
        MyAnimator.Play("wait", 2);
        
        yield return new WaitForSeconds(castable.MyCastTime);

        // COMMENTED BUT WAS NEEDED BEFORE ?
        //StopAction();
    }


    public void CastSpell(Spell spell)
    {
        if (spell.MyManaCost <= mana.MyCurrentValue)
        {
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("ground");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                float distance = Vector3.Distance(this.transform.position, new Vector3(hit.point.x, 0, hit.point.z));
                //Debug.Log("DISTANCE: " + distance);
            }

            // RANGE OU PAS RANGE SUR LES SORTS ??? 

            if (!spell.OnCooldown && !IsAttacking && IsAlive && unusedSpell == null /*&& distance <= spell.MyRange*/)
            {
                // Debug.Log("SPELL RANGE: " + spell.MyRange);

                if (spell.MySpellType == Spell.SpellType.self)
                {
                    Debug.Log("AttackRoutineSelf");
                }
                if (spell.MySpellType == Spell.SpellType.AOE_self)
                {
                    MyInitRoutine = StartCoroutine(AttackRoutineAoEself(spell));
                    Debug.Log("AttackRoutineAoEself");
                }
                if (spell.MySpellType == Spell.SpellType.At_cursor)
                {
                    MyInitRoutine = StartCoroutine(AttackRoutineAtCursor(spell));
                    //MyInitRoutine = StartCoroutine(TestRaycast(spell));
                    Debug.Log("AttackRoutineAtCursor");
                    aoeSpell = spell;
                }
                if (spell.MySpellType == Spell.SpellType.thrown)
                {
                    MyInitRoutine = StartCoroutine(AttackRoutineNoTarget(spell));
                    Debug.Log("AttackRoutineNoTarget");
                }
                //if (!spell.NeedsTarget)
                //{
                //    MyInitRoutine = StartCoroutine(AttackRoutineNoTarget(spell));
                //}
                if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && /*!IsMoving &&*/ InLineOfSight() && InRange(spell, MyTarget.transform.position))
                {
                    MyInitRoutine = StartCoroutine(AttackRoutine(spell));
                    Debug.Log("AttackRoutine");
                }
            }
            else
            {
                Destroy(unusedSpell);
            }

            // WAS NEEDED BEFORE DUNNO WHY CAUSE IT SEEM USEFUL TO KNOW WHEN SPELL IS ON CD, BUT REMOVING IT SOLVE DOUBLE CAST BUG
            //MyCooldownRoutine = StartCoroutine(Cooldown(spell));
        }
    }

    private IEnumerator Regen()
    {
        while (true)
        {
            //if (!InCombat)
            //{
                if (health.MyCurrentValue < health.MyMaxValue && IsAlive)
                {
                    // 5% max health
                    int value = Mathf.FloorToInt(health.MyMaxValue * healthRegenMultiplier); /*.05f*/

                    health.MyCurrentValue += value;

                    CombatTextManager.MyInstance.CreateText(transform.position, value.ToString(), SCTTYPE.HEAL, false);
                }

            //}

            // Wait for .5s between health & mana regen
            //yield return new WaitForSeconds(.5f);

            //if (!InCombat)
            //{
                if (mana.MyCurrentValue < mana.MyMaxValue && IsAlive)
                {
                    // 5% max maan
                    int value = Mathf.FloorToInt(mana.MyMaxValue * .05f);

                    mana.MyCurrentValue += value;

                    // To display mana regen
                    //CombatTextManager.MyInstance.CreateText(transform.position, value.ToString(), SCTTYPE.MANA, false);
                }

            //}

            // This define how often the regen ticks
            yield return new WaitForSeconds(1.5f);
        }     
    }


    private bool InRange(Spell spell, Vector3 targetPos)
    {
        if (Vector3.Distance(targetPos, transform.position) <= spell.MyRange)
        {
            return true;
        }
        MessageFeedManager.MyInstance.WriteMessage("OUT OF RANGE", Color.red);

        return false;
    }


    public void Gather(ICastable castable, List<Drop> items)
    {
        if (!IsAttacking)
        {
            MyInitRoutine = StartCoroutine(GatherRoutine(castable, items));
        }
    }
    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            RaycastHit los;

            if (Physics.Raycast(transform.position, targetDirection, out los, Vector3.Distance(transform.position, MyTarget.transform.position), losMask))
            {
                return false;
            }
        }
        if (MyTargetPos != null)
        {
            //Vector3 targetDirection = (MyTargetPos - transform.position).normalized;
            Debug.DrawRay(transform.position, MyTargetPos - transform.position, Color.red);
            RaycastHit los;

            //Debug.Log((new Vector3(0, 0, 0) - MyTargetPos).magnitude);

            // BEWARE, the TargetPos is calculated related to the character, thats why we calculate the distance starting from 0,0,0
            if (Physics.Raycast(transform.position, MyTargetPos-transform.position, out los, (new Vector3(0,0,0) - MyTargetPos).magnitude, losMask))
            {
                //Debug.Log("los : " + los.collider.name);
                return false;
            }
        }
        //Debug.DrawRay(transform.position, MyTarget.transform.position - transform.position, Color.red);
        return true;

    }

    //private void Block()
    //{
    //    foreach (Block b in blocks)
    //    {
    //        b.Deactivate();
    //    }
    //    blocks[exitIndex].Activate();
    //}

    public void StopAction()
    {
        SpellBook.MyInstance.stopCasting();       

        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
            IsAttacking = false;
            MyAnimator.SetBool("attack", IsAttacking);
        }
    }

    private void StopInit()
    {
        if (MyInitRoutine != null)
        {
            StopCoroutine(MyInitRoutine);
        }
    }

    public void SpawnBossPortal()
    {
        Vector3 spawnPosition = this.transform.position + Random.insideUnitSphere * 2;
        NavMeshHit hitG;
        NavMesh.SamplePosition(spawnPosition, out hitG, 10, 1);
        Instantiate(portal, new Vector3(spawnPosition.x, 0f, spawnPosition.z), portal.transform.rotation);
    }

    //public void Interact()
    //{
    //    if (MyInteractable != null)
    //    {
    //        MyInteractable.Interact();
    //        StopMovement();
    //    }
    //}

    public void GainXP(int xp)
    {
        MyXpStat.MyCurrentValue += xp;

        // Xp displaying above character
        //CombatTextManager.MyInstance.CreateText(transform.position, xp.ToString(), SCTTYPE.XP, false);

        if (MyXpStat.MyCurrentValue >= MyXpStat.MyMaxValue)
        {
            StartCoroutine(LevelUp());
        }
    }

    //public void AddAttacker(Enemy enemy)
    //{
    //    if (!MyAttackers.Contains(enemy))
    //    {
    //        MyAttackers.Add(enemy);
    //    }
    //}

    public IEnumerator Cooldown(Spell spell)
    {
        Debug.Log("CD routine");
        spell.OnCooldown = true;
        yield return new WaitForSeconds(spell.MyCooldown);
        spell.OnCooldown = false;
    }

    private IEnumerator LevelUp()
    {
        while (!MyXpStat.IsFull)
        {
            yield return null;
        }
        MyLevel++;
        GameObject lvlup = Instantiate(levelUp, transform.position, Quaternion.identity , this.transform);

        Destroy(lvlup, 3);
        levelText.text = MyLevel.ToString();
        MyXpStat.MyMaxValue = 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f);
        MyXpStat.MyMaxValue = Mathf.Floor(MyXpStat.MyMaxValue);
        MyXpStat.MyCurrentValue = MyXpStat.MyOverflow;
        MyXpStat.Reset();

        if (MyXpStat.MyCurrentValue >= MyXpStat.MyMaxValue)
        {
            StartCoroutine(LevelUp());
        }
    }

    public void OnEquipementChanged(Armor newArmor)
    {
        MyHealth.MyMaxValue += newArmor.stamina;
        MyHealth.MyCurrentValue += newArmor.stamina;

        MyMana.MyMaxValue += newArmor.intellect;
        MyMana.MyCurrentValue += newArmor.intellect;

        fireDamage.AddModifier(newArmor.firedamage);
        frostDamage.AddModifier(newArmor.frostdamage);
        lightningDamage.AddModifier(newArmor.lightningdamage);
        corrosiveDamage.AddModifier(newArmor.corrosivedamage);

        if (newArmor.MyQuality == Quality.Legendary)
        {
            Debug.Log(newArmor.legendaryPower);

            legendaryPower.AddPower(newArmor.legendaryPower);

            //if (newArmor.legendaryPower == LegendaryPower.FireballSize)
            //{
            //    // We should manipulate the prefab instead of using an alternate one in case we need to add up more effects
            //    //SpellBook.MyInstance.GetSpell("Fireball").MySpellPrefab = newArmor.legendaryPowerPrefab;
            //    SpellBook.MyInstance.GetSpell("Fireball").MySpellPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            //}            
        }
    }
    public void OnEquipementRemoved(Armor oldArmor)
    {
        Debug.Log("OnEquipementRemoved");
        MyHealth.MyMaxValue -= oldArmor.stamina;
        //TO AVOID DYING FROM CHANGING GEAR
        if (MyHealth.MyCurrentValue < oldArmor.stamina)
        {
            MyHealth.MyCurrentValue = 1;
        }
        else
        {
            MyHealth.MyCurrentValue -= oldArmor.stamina;
        }

        MyMana.MyMaxValue -= oldArmor.intellect;
        MyMana.MyCurrentValue -= oldArmor.intellect;

        fireDamage.RemoveModifier(oldArmor.firedamage);
        frostDamage.RemoveModifier(oldArmor.frostdamage);
        lightningDamage.RemoveModifier(oldArmor.lightningdamage);
        corrosiveDamage.RemoveModifier(oldArmor.corrosivedamage);

        if (oldArmor.MyQuality == Quality.Legendary)
        {
            legendaryPower.RemovePower(oldArmor.legendaryPower);

            //if (oldArmor.legendaryPower == LegendaryPower.FireballSize)
            //{
            //    // We should manipulate the prefab instead of using an alternate one in case we need to add up more effects
            //    SpellBook.MyInstance.GetSpell("Fireball").MySpellPrefab.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); ;
            //}
        }
    }


    public void UpdateLevel()
    {
        levelText.text = MyLevel.ToString();
    }

    public void UpdateGold()
    {
        goldText.text = MyGold.ToString();
    }

    public IEnumerator Respawn()
    {
        // Disabling navmesh & player gameobject
        this.MyNavMeshAgent.enabled = true;
        this.transform.GetChild(0).gameObject.SetActive(false);

        // Opening Respawn button 
        UIManager.MyInstance.OpenClose(respawnCanvasGroup);
        
        while (!wantToRespawn)
        {
            yield return null;
        }

        // Closing Respawn button 
        UIManager.MyInstance.OpenClose(respawnCanvasGroup);

        // Waiting 5s to respawn
        yield return new WaitForSeconds(5f);
        Debug.Log("waiting 5secs");

        // Putting back player to its supposed health & mana level
        health.Initialize(MyHealth.MyMaxValue, MyHealth.MyMaxValue);
        MyMana.Initialize(MyMana.MyMaxValue, MyMana.MyMaxValue);

        // Placing player to its inital position & setting it back to active while reseting its pathing
        transform.position = initialPosition;
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.MyNavMeshAgent.ResetPath();

        wantToRespawn = false;
        MyAnimator.SetTrigger("respawn");
    }

    // Is triggered via respawn button canvas 
    public void RespawnButton()
    {
        wantToRespawn = true;
    }

    public override void AddAttacker(Character attacker)
    {
        int count = Attackers.Count;
        base.AddAttacker(attacker);

        if (count == 0)
        {
            InCombat = true;
            // In combat text above head
            //CombatTextManager.MyInstance.CreateText(transform.position, "+COMBAT", SCTTYPE.TEXT, false);
        }        
    }
    public override void RemoveAttacker(Character attacker)
    {
        base.RemoveAttacker(attacker);
        if (Attackers.Count == 0)
        {
            InCombat = false;
            // Out of combat text above head
            //CombatTextManager.MyInstance.CreateText(transform.position, "-COMBAT", SCTTYPE.TEXT, false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Checking if we have the IInteractable we are colliding with
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (other.tag == "enemy")
        {
            if (!MyInteractables.Contains(interactable))
            {
                MyInteractables.Add(interactable);
            }
            //MyInteractables = other.GetComponent<IInteractable>();
        }
        if (other.tag == "interactable")
        {           
            if (!MyInteractables.Contains(interactable))
            {
                MyInteractables.Add(interactable);
            }
            //MyInteractables = other.GetComponentInParent<IInteractable>();
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "enemy" || other.tag == "interactable")
        {
            // If we have something in our IInteractable list
            if (MyInteractables.Count > 0)
            {
                // Checking if we have the IInteractable we are colliding with
                IInteractable interactable = other.GetComponentInParent<IInteractable>();
                if (interactable != null)
                {
                    interactable.StopInteract();                    
                }
                MyInteractables.Remove(interactable);
            }
        
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "interactable")
        {
            //interactable = collision.gameObject.GetComponent<IInteractable>();
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "interactable")
        {

        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "interactable")
        {
        }
    }
}