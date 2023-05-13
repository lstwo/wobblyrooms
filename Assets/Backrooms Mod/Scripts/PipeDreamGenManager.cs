using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;

public class PipeDreamGenManager : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject roomObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(roomObj, spawnPos.position, roomObj.transform.rotation);
            gameObject.SetActive(false);
        }
    }
}
