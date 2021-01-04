using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    int HP = 1;
    void Start()
    {
        
    }
    public void ChangeHP(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        //Destroy(gameObject);
        Application.LoadLevel(Application.loadedLevel);
    }
}
