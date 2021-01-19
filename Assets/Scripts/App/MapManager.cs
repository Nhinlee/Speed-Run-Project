using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private List<Button> btnLevels;
    
    // TODO: Implement this class to handle lock button & Open button (Game save)

    public void ReturnToMenuScene()
    {
        GameLoader.Instance.LoadMenuScene();
    }
}
