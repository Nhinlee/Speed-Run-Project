using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPlace : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 resetPoint = new Vector2(0f, 0f);
    [SerializeField] private float timeDelay = 2f;
    void Awake()
    {
        int numberObject = FindObjectsOfType<ResetPlace>().Length;
        if(numberObject > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayerDie()
    {
        StartCoroutine(TimeDelay());
        int currentScence = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScence);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = resetPoint;
    }
    IEnumerator TimeDelay()
    {
        yield return new WaitForSecondsRealtime(timeDelay);
    }

    public void SetResetPlace(Vector2 newPlace)
    {
        resetPoint = newPlace;
    }
}
