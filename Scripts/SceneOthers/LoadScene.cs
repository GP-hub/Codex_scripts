using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerName;

    [SerializeField]
    private int sceneToLoadIndex;

    [SerializeField]
    private Vector3 warpPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == PlayerName.name)
        {
            string NameScene = SceneManager.GetSceneByBuildIndex(sceneToLoadIndex).name;

            if (!SceneManager.GetSceneByName(NameScene).isLoaded)
            {
                SceneManager.LoadSceneAsync(sceneToLoadIndex, LoadSceneMode.Additive);                
                //UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
            }
            // MIGHT BE OF USE LATER TO UNLOAD SCENES
            //if (SceneManager.GetSceneByName(NameScene).isLoaded)
            //{
            //    LoaderManager.UnloadScene(sceneToLoadIndex);
            //    //UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
            //}
        }

        WarpPlayer(other);
    }

    private void WarpPlayer(Collider other)
    {
        if (other.name == PlayerName.name)
        {
            Player.MyInstance.MyNavMeshAgent.Warp(warpPosition);
            //Player.MyInstance.MyNavMeshAgent.Warp(new Vector3(-65f, 0f, 0f));


            //Player.MyInstance.MyNavMeshAgent.ResetPath();
            //Player.MyInstance.MyNavMeshAgent.enabled = false;
            //Player.MyInstance.MyNavMeshAgent.enabled = true;

        }
    }
    // Update NavMesh
}
