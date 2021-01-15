using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CanvasTutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject panelTutorial;

    // Ex: Tap/Hold
    [SerializeField]
    private TextMeshProUGUI txtInteract;

    // Ex: W/S/Space
    [SerializeField]
    private TextMeshProUGUI txtButton;

    // Ex: To Jump/To JumpSlice/To Punch
    [SerializeField]
    private TextMeshProUGUI txtActionName;

    [SerializeField]
    private float timeShowDialog;

    private Coroutine cShowDialog;

    public void Show(string strInteract, string strButton, string strActionName)
    {
        txtInteract.text = strInteract;
        txtButton.text = strButton;
        txtActionName.text = strActionName;
        //---------------------------------
        if (cShowDialog != null)
        {
            StopCoroutine(cShowDialog);
            cShowDialog = null;
            
        }
        cShowDialog = StartCoroutine(CoroutineShowDialog());
    }

    private IEnumerator CoroutineShowDialog()
    {
        panelTutorial.SetActive(true);
        yield return new WaitForSeconds(timeShowDialog);
        panelTutorial.SetActive(false);
    }
}
