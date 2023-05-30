using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;
using ModWobblyLife.Network;

public class PipeDreamGenManager : MonoBehaviour
{
    public Transform spawnPos;
    public ModNetworkBehaviour roomObj;
    public NetworkManager networkManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ModNetworkManager.Instance.InstantiateNetworkPrefab(roomObj.gameObject, null, spawnPos.position, 
                roomObj.transform.rotation, null, true);
            gameObject.SetActive(false);
        }
    }
}
