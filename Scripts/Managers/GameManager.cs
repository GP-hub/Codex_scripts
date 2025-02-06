using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public delegate void KillConfirmed(Character character);
public class GameManager : MonoBehaviour
{
    public event KillConfirmed killConfirmedEvent;
    private static GameManager instance;

    [SerializeField]
    private List <GameObject> playableCharacters = new List<GameObject>();

    private Camera mainCamera;
    protected Coroutine distanceRoutine;
    private float dist;

    private int currentLevel;

    private Player player;

    //[SerializeField]
    //private GameObject characters;

    [SerializeField]
    private Texture2D cursor;

    [SerializeField]
    private LayerMask IgnoredMasks;

    private Enemy currentTarget;
    private int targetIndex;

    public static GameManager MyInstance
    { 
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    void Awake()
    {
        //Debug.Log("character number selected : " + MainMenu.characterNumber);

        for (int i = 0; i < playableCharacters.Count; i++)
        {
            if (MainMenu.characterNumber != i)
            {
                playableCharacters[i].transform.gameObject.SetActive(false);
            }
            else if (MainMenu.characterNumber == i)
            {
                playableCharacters[i].transform.gameObject.SetActive(true);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.depthTextureMode = DepthTextureMode.Depth;

        //foreach (GameObject gO in characters.transform)
        //{
        //    if (gO.transform.tag == "Player")
        //    {
        //        player = (Player)gO.GetComponent<Character>();
        //    }
        //}
        player = (Player)GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        //Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
        NextTarget();
        //Debug.Log(LayerMask.GetMask("ground"));
    }

    private void ClickTarget()
    {
        //check overring UI element GET KEY DOWN INSTEAD OF GETKEY
        if (Input.GetKeyDown(KeybindManager.MyInstance.Keybinds["MOVE"]) && !EventSystem.current.IsPointerOverGameObject() && player.IsAlive && HandScript.MyInstance.MyMoveable == null && !player.IsCasting)
        {
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("LineOfSight");
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoredMasks))
            {
                //Debug.Log("hit : " + hit.collider.name);
                if (hit.collider != null)
                {
                    if (player.MyInteractables != null)
                    {
                        foreach (IInteractable item in player.MyInteractables)
                        {
                            if (item is IInteractable)
                            {
                                item.StopInteract();
                            }                            
                        }
                    }

                    IInteractable entity = hit.collider.GetComponent<IInteractable>();

                    if (hit.collider.tag == "enemy")
                    {
                        DeselectTarget();
                        SelectTarget(hit.collider.GetComponent<Enemy>());

                        //currentTarget = hit.collider.GetComponent<Enemy>();
                        //player.MyTarget = currentTarget.Select();
                        //UIManager.MyInstance.ShowTargetFrame(currentTarget);
                    }
                    if (hit.collider.tag == "ground")
                    {
                        player.StopAction();
                        player.SetDestination();
                    }
                    if (hit.collider.tag == "interactable")
                    {                        
                        dist = Vector3.Distance(player.transform.position, hit.transform.position);

                        if (distanceRoutine != null)
                        {
                            StopCoroutine(distanceRoutine);
                        }                        
                        //player.MyInteractables = null;
                        distanceRoutine = StartCoroutine(Distance(player, hit));
                        if (dist >= 1.5f)
                        {
                            player.SetDestination();
                        }
                    }

                    IEnumerator Distance(Player player, RaycastHit hit2)
                    {
                        if (entity != null)
                        {
                            entity.StopInteract();
                        }
                        while (dist >= 1.5f)
                        {
                            dist = Vector3.Distance(player.transform.position, hit2.transform.position);
                            yield return dist;
                        }
                        //player.MyInteractable = hit.collider.GetComponent<IInteractable>();

                        //player.MyInteractables.Contains(entity);
                        player.StopMovement();                        
                        entity.Interact();
                        yield break;
                    }
                }
                else
                {
                    UIManager.MyInstance.HideTargetFrame();
                    
                    DeselectTarget();

                    currentTarget = null;
                    player.MyTarget = null;
                    
                }
            }         
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                IInteractable entity = hit.collider.GetComponent<IInteractable>();
                if (hit.collider != null)
                {
                    // Colliding with enemy, colliding with interactable AND it is the same one as the one I am clicking
                    if (hit.collider.tag == "enemy" || hit.collider.tag == "interactable" && player.MyInteractables.Contains(entity))
                    {
                        // Call the interact function from the enemy we hit
                        entity.Interact();
                    }
                }
            }
        }
    }

    private void NextTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DeselectTarget();

            if (Player.MyInstance.Attackers.Count > 0)
            {
                if (targetIndex < Player.MyInstance.Attackers.Count)
                {
                    SelectTarget(Player.MyInstance.Attackers[targetIndex] as Enemy);
                    targetIndex++;
                    if (targetIndex >= Player.MyInstance.Attackers.Count)
                    {
                        targetIndex = 0;
                    }
                }
                else
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void DeselectTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.DeSelect();
        }
    }

    private void SelectTarget(Enemy enemy)
    {
        //Debug.Log("selected target");
        currentTarget = enemy;
        player.MyTarget = currentTarget.Select();
        UIManager.MyInstance.ShowTargetFrame(currentTarget);
    }

    public void OnKillConfirmed(Character character)
    {
        if (killConfirmedEvent != null)
        {
            killConfirmedEvent(character);
        }
    }

    public void StartLevel(int level)
    {
        currentLevel = level;
        UIManager.MyInstance.GetCompletion(0);
    }

    public void StartBoss()
    {
        foreach (GameObject spawner in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            //if (spawner.name == "BossSpawner")
            //{
            //    spawner.GetComponent<SpawnerManager>().enabled = true;
            //}
            //else
            //{
            //    spawner.SetActive(false);
            //}

            spawner.SetActive(false);
        }

        Player.MyInstance.SpawnBossPortal();
    }

    public void LevelCompletionBarFull()
    {
        
    }

    public void GainCompletion(Enemy enemy)
    {
        //Debug.Log(enemy.completionPoints);
        UIManager.MyInstance.GetCompletion(enemy.completionPoints);
    }

    public void RandomPower()
    {
        UIManager.MyInstance.menus[11].GetComponent<PowerPick>().PowerRoll();
    }
}