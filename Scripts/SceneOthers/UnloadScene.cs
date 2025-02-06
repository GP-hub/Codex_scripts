using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadScene : MonoBehaviour
{
    public int scene;

    bool unloaded;
    private void OnTriggerEnter()
    {
        if (!unloaded)
        {
            unloaded = true;
            LoaderManager.UnloadScene(scene);   
        }
    }
}