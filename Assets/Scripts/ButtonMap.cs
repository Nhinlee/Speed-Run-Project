using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMap : MonoBehaviour
{
    // Editor Fields
    [SerializeField]
    private int level;

    // Event Handler
    public void LoadLevel()
    {
        Debug.Log("heellllsdafsafdoasdfos");
        GameLoader.Instance.LoadLevelScene(this.level);
    }
}
