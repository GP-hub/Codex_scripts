using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript instance;

    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }
            return instance;
        }
    }
    public IMoveable MyMoveable { get; set; }

    //public RectTransform rectTransform;

    private Image icon;

    [SerializeField]
    private Canvas parentCanvas;

    [SerializeField]
    private Vector3 offset;

    private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // ONLINE TUTORIAL
        icon.transform.position = Input.mousePosition + offset;

        ////////////// ONLINE SOLUTION BECAUSE 3D ASSIGNED CAMERA
        ////////////Vector2 movePos;

        ////////////RectTransformUtility.ScreenPointToLocalPointInRectangle(
        ////////////    parentCanvas.transform as RectTransform,
        ////////////    Input.mousePosition + offset, parentCanvas.worldCamera,
        ////////////    out movePos);

        ////////////Vector3 mousePos = parentCanvas.transform.TransformPoint(movePos);

        //////////////Set fake mouse Cursor KEEP THIS COMMENTED
        //////////////mouseCursor.transform.position = mousePos;

        //////////////Move the Object/Panel
        ////////////transform.position = mousePos;



        // if I release first mouse button, AND I'm not hoverring over any elements, AND hands is not empty
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable != null)
        {
            
            Item item = (Item)MyMoveable;
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("ground");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if (hit.collider.tag == "ground")
                {
                    // Define the position we clicked
                    targetPosition = new Vector3(hit.point.x, 0f, hit.point.z);

                    // Finding the closest point to the position we clicked that is ON the navmesh
                    NavMeshHit hitG;
                    NavMesh.SamplePosition(targetPosition, out hitG, 100, 1);

                    // Dropping item on navmesh close to cursor position
                    Instantiate(item.MyItemGameObject, hitG.position, transform.rotation);
                }
            }
            // Clearing the item from inventory + hand
            DeleteItem();
        }        
    }

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.MyIcon;
        icon.enabled = true;
    }

    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;
        MyMoveable = null;
        icon.enabled = false;
        icon.sprite = null;
        return tmp;
    }

    public void Drop()
    {
        MyMoveable = null;
        icon.enabled = false;
        icon.sprite = null;
        InventoryScript.MyInstance.FromSlot = null;
    }

    public void DeleteItem()
    {
        if (MyMoveable is Item )
        {
            Item item = (Item)MyMoveable;
            if (item.MySlot != null)
            {
                item.MySlot.Clear();
            }
            else if (item.MyCharButton != null)
            {
                Debug.Log("handscript dequiparmor");

                item.MyCharButton.DequipArmor(/*item as Armor*/);
                //if (item is Armor)
                //{
                //    item.MyCharButton.DequipArmor(/*item as Armor*/);
                //}
                //else
                //{
                //    item.MyCharButton.DequipItem();
                //}
            }
        }

        Drop();

        InventoryScript.MyInstance.FromSlot = null;
    }
}
