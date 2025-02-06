using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemButton : MonoBehaviour
{
    public Button itemButton;
    public TMP_Text itemButtonTextMP;
    public Image itemButtonImage;
    public Item itemToLoot;
    public CanvasGroup canvasGroup;
    public int timeBeforeFade = 5;
    private bool isFaded = false;

    private void LateUpdate()
    {
        //transform.rotation = Camera.main.transform.rotation * originalRotation;

        // GOOD : slightly more effective ?
        //transform.forward = Camera.main.transform.forward;

        Vector3 itemNamePos = Camera.main.WorldToScreenPoint(this.transform.position);
        itemButton.transform.position = itemNamePos;

        if (!isFaded) 
        {
            Invoke("ItemNameFade", timeBeforeFade);
            isFaded = true;
        }
        if (Input.GetKeyDown(KeybindManager.MyInstance.Keybinds["ITEM"]) && canvasGroup.alpha == 0)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            Invoke("ItemNameFade", timeBeforeFade);
        }
    }

    void ItemNameFade()
    {
        if (canvasGroup.alpha > 0)
        {
            //canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, 3f * Time.deltaTime);
            canvasGroup.alpha = 0;
        }
        if (canvasGroup.alpha < 1)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}