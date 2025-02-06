using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static int characterNumber;

    public void Load()
    {
        LoaderManager.Load(LoaderManager.Scene.GameScene_Hub);
    }
    public void Quit()
    {
        Debug.Log("Quit!");
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
    public void CharacterSelection(int i)
    {
        characterNumber = i;
    }
}
