using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TouchSpikeAndSaw : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float TimeWait;
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collider other)
    {
        if (other.gameObject.CompareTag("touchToDie"))
        {
            Die();
        }
    }
    private void Die()
    {
        StartCoroutine(WaitForTime());
        LoadCurrentScence();
    }
    void LoadCurrentScence()
    {
        var currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
    }
    
    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(TimeWait);
    }
}
