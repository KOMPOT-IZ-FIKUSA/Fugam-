using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] GameObject settings;
    [SerializeField] GameObject menu;

    public void LoadScene(int Index)
    {
        SceneManager.LoadScene(Index);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1); // loads the scene 1 that is going to be the chamber
        
    }

    public void QuitGame()
    {
        Application.Quit(); //exits the game while builded
    }

    public void OpenSettings()
    {
        settings.SetActive(true);// set it as true
        menu.SetActive(false); // disable menu

    }

    public void CloseSettings()
    {
        settings.SetActive(false);// disable settings
        menu.SetActive(true); // enable menu
    }
    
}
