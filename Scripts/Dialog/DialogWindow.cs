using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogWindow : Window
{
    private static DialogWindow instance;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private float speed;

    private Dialog dialog;

    private DialogNode currentNode;

    [SerializeField]
    private GameObject answerButtonPrefab;

    [SerializeField]
    private Transform answerTransform;

    private List<DialogNode> answers = new List<DialogNode>();

    private List<GameObject> buttons = new List<GameObject>(); 


    public static DialogWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogWindow>();
            }
            return instance;
        }
    }


    public void SetDialogue(Dialog dialog)
    {
        // Solution to clear in case walking away from NPC
        Clear();

        // Clearing text field
        text.text = string.Empty;

        this.dialog = dialog;

        // Making sure that our current node is equal to the root
        this.currentNode = dialog.Nodes[0];

        StartCoroutine(RunDialog(currentNode.Text));
    }

    // SAVE FOR INSTANT DIALOG
    private IEnumerator RunDialog(string dialog)
    {
        text.text = dialog;
        yield return new WaitForSeconds(speed);
        ShowAnswers();
    }

    // SAVED TO MAKE DIALOG APPEARING SLOWLY
    //private IEnumerator RunDialog(string dialog)
    //{
    //    for (int i = 0; i < dialog.Length; i++)
    //    {
    //        text.text += dialog[i];
    //        yield return new WaitForSeconds(speed);
    //    }

    //    ShowAnswers();
    //}

    private void ShowAnswers()
    {
        answers.Clear();

        foreach (DialogNode node in dialog.Nodes)
        {
            if (node.Parent == currentNode.Name)
            {
                answers.Add(node);
            }
        }

        if (answers.Count > 0)
        {
            answerTransform.gameObject.SetActive(true);

            foreach (DialogNode node in answers)
            {
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                go.GetComponentInChildren<TextMeshProUGUI>().text = node.Answer;
                go.GetComponent<Button>().onClick.AddListener(delegate { PickAnswer(node); });            
            }
        }
        else
        {
            answerTransform.gameObject.SetActive(true);
            GameObject go = Instantiate(answerButtonPrefab, answerTransform);
            buttons.Add(go);
            go.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
            go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialog(); });
        }
    }


    private void PickAnswer(DialogNode node)
    {
        this.currentNode = node;
        Clear();
        StartCoroutine(RunDialog(currentNode.Text));
    }

    public void CloseDialog()
    {
        Close();
        Clear();
    }

    private void Clear()
    {
        text.text = string.Empty;
        answerTransform.gameObject.SetActive(false);

        foreach (GameObject gameObject in buttons)
        {
            Destroy(gameObject);
        }

        buttons.Clear();
    }
}
