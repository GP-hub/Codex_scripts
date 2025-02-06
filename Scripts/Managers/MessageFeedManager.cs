using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageFeedManager : MonoBehaviour
{

    private static MessageFeedManager instance;

    [SerializeField]
    private GameObject messagePrefab;

    public static MessageFeedManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MessageFeedManager>();
            }
            return instance;
        }
    }
    
    public void WriteMessage(string message)
    {
        GameObject go =Instantiate(messagePrefab, transform);
        //go.GetComponent<Text>().text = message;
        go.GetComponent<TextMeshProUGUI>().text = message;
        go.transform.SetAsFirstSibling();
        // Destroy after 2sec, coroutine if wanting to fade
        Destroy(go, 2);
    }

    public void WriteMessage(string message, Color color)
    {
        GameObject go = Instantiate(messagePrefab, transform);
        //Text t = go.GetComponent<Text>();
        TextMeshProUGUI t = go.GetComponent<TextMeshProUGUI>();

        t.text = message;
        t.color = color;

        go.transform.SetAsFirstSibling();
        // Destroy after 2sec, coroutine if wanting to fade
        Destroy(go, 2);
    }

}
