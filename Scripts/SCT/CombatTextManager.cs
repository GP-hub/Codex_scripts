using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum SCTTYPE { DAMAGE, HEAL, XP, TEXT, MANA}
public class CombatTextManager : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraTransform;

    [SerializeField]
    private Vector3 offset;

    private static CombatTextManager instance;
    public static CombatTextManager MyInstance
    { 
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CombatTextManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject combatTextPrefab;

    private Queue<SCTObject> SCTQueue = new Queue<SCTObject>();

    public void Start()
    {
        StartCoroutine(WriteText());
    }

    public void CreateText(Vector3 position, string text, SCTTYPE type, bool crit)
    {
        SCTQueue.Enqueue(new SCTObject() { Crit = crit, Position = position, Text = text, SCTTYPE = type });
    }


    public IEnumerator WriteText()
    {
        while (true)
        {
            if (SCTQueue.Count > 0)
            {
                SCTObject sctObject = SCTQueue.Dequeue();

                Vector3 sctPosition = sctObject.Position;

                TextMeshProUGUI sct = Instantiate(combatTextPrefab, transform).GetComponent<TextMeshProUGUI>();

                sct.transform.position = sctPosition + offset;

                string before = string.Empty;
                string after = string.Empty;

                switch (sctObject.SCTTYPE)
                {
                    case SCTTYPE.DAMAGE:
                        before = "-";
                        sct.color = Color.red;
                        break;
                    case SCTTYPE.HEAL:
                        before = "+";
                        sct.color = Color.green;
                        break;
                    case SCTTYPE.XP:
                        before = "+";
                        after = " XP";
                        sct.color = Color.yellow;
                        break;
                    case SCTTYPE.TEXT:
                        sct.color = Color.white;
                        break;
                    case SCTTYPE.MANA:
                        before = "+";
                        sct.color = Color.blue;
                        break;
                }

                sct.text = before + sctObject.Text + after;

                if (sctObject.Crit)
                {
                    sct.GetComponent<Animator>().SetBool("Crit", sctObject.Crit);
                }
            }
            // We wait 0.5s before writing the next message.
            yield return new WaitForSeconds(.2f);
        }
        
        //sct.transform.LookAt(cameraTransform.transform);
        //sct.transform.Rotate(new Vector3(0f, 180f, 0f));
        //sct.transform.position = Player.MyInstance.transform.position + offset;
  
    }
}


public class SCTObject
{
    public Vector3 Position { get; set; }
    public string Text { get; set; }
    public SCTTYPE SCTTYPE { get; set; }
    public bool Crit { get; set; }
}