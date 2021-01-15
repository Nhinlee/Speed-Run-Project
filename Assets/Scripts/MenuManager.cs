using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Editor Field
    [SerializeField]
    private GameObject panelMenu;

    [SerializeField]
    private GameObject panelGameSlot;


    // Event Handler
    public void LoadMapScene()
    {
        GameLoader.Instance.LoadMapScene();
    }

    public void ReturnToPanelMenu()
    {
        panelMenu.SetActive(true);
        panelGameSlot.SetActive(false);
    }

    public void NavigateToPanelGameSlot()
    {
        panelMenu.SetActive(false);
        panelGameSlot.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
