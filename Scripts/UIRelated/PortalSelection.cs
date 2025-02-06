using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSelection : MonoBehaviour, IInteractable
{
    private CanvasGroup canvasGroup;

    //[SerializeField]
    //private List<int> sceneToLoadIndex = new List<int> { 0, 1, 2, 3, 4, 5 };

    private bool isOpen;

    [SerializeField, Header("TP Location")] private TpLocation[] tpLocation;

    public void OpenClose()
    {
        //canvasGroup = portalUI.GetComponent<CanvasGroup>();
        canvasGroup = UIManager.MyInstance.portal_UI;

        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }

    public void WarpPlayerHome()
    {
        string NameScene = SceneManager.GetSceneByBuildIndex(tpLocation[0].locationIndex).name;

        if (!SceneManager.GetSceneByName(NameScene).isLoaded)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(tpLocation[0].locationIndex, LoadSceneMode.Additive);
            //UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
        }

        Player.MyInstance.MyNavMeshAgent.Warp(tpLocation[0].locationPosition);
        OpenClose();
    }

    public void WarpPlayerLevelOne()
    {
        string NameScene = SceneManager.GetSceneByBuildIndex(tpLocation[1].locationIndex).name;
        // Display loading Screen
        UIManager.MyInstance.LoadingScreen();
        //Loading the scene 
        StartCoroutine(LoadScene(tpLocation[1].locationIndex));
        
        //UIManager.MyInstance.LoadingScreen();

        //if (!SceneManager.GetSceneByName(NameScene).isLoaded)
        //{
        //    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(level1Index, LoadSceneMode.Additive);
        //    //UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
        //}

        //Player.MyInstance.MyNavMeshAgent.Warp(level1);

        //if (!Player.MyInstance.MyNavMeshAgent.isOnNavMesh)
        //{
        //    Player.MyInstance.MyNavMeshAgent.enabled = false;
        //    Player.MyInstance.MyNavMeshAgent.enabled = true;
        //}

        //OpenClose();
        //GameManager.MyInstance.StartLevel(level1Index);
    }

    IEnumerator LoadScene(int sceneNumber)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);

        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;

        //Debug.Log("Pro :" + asyncOperation.progress);

        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;

                Player.MyInstance.MyNavMeshAgent.Warp(tpLocation[1].locationPosition);

                if (!Player.MyInstance.MyNavMeshAgent.isOnNavMesh)
                {
                    Player.MyInstance.MyNavMeshAgent.enabled = false;
                    Player.MyInstance.MyNavMeshAgent.enabled = true;
                }

                //OpenClose();
                if (sceneNumber != tpLocation[0].locationIndex)
                {
                    GameManager.MyInstance.StartLevel(0);                    
                }                
            }
            
            yield return null;
        }

        // Remove Loading screen
        UIManager.MyInstance.LoadingScreen();
    }


    public void Interact()
    {
        //canvasGroup = portalUI.GetComponent<CanvasGroup>();
        canvasGroup = UIManager.MyInstance.portal_UI;
        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            isOpen = true;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void StopInteract()
    {
        //canvasGroup = portalUI.GetComponent<CanvasGroup>();
        canvasGroup = UIManager.MyInstance.portal_UI;

        if (isOpen)
        {
            isOpen = false;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }
}

[System.Serializable]
public class TpLocation
{
    public string locationName;
    [SerializeField] public int locationIndex;
    [SerializeField] public Vector3 locationPosition;
}
