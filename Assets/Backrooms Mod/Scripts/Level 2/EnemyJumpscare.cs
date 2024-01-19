using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyJumpscare : MonoBehaviour
{
    public GameObject jumpscare;

    private void Start()
    {
        for (int i = 0; i < SceneManager.GetActiveScene().GetRootGameObjects().Length; i++)
        {
            if (SceneManager.GetActiveScene().GetRootGameObjects()[i].name == "JumpscareCanvas")
            {
                jumpscare = SceneManager.GetActiveScene().GetRootGameObjects()[i].transform.Find("Panel").gameObject; 
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Gamemode.instance.jumpscares) return;
        if(other.tag == "Player")
        {
            jumpscare.SetActive(true);
            StartCoroutine(ResetJumpscare());
        }
    }

    IEnumerator ResetJumpscare()
    {
        yield return new WaitForSeconds(0.3175f);
        jumpscare.SetActive(false);
        Destroy(gameObject);
    }
}
