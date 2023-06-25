using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.Network;

public class EntitySpawner : MonoBehaviour
{
    public ModNetworkBehaviour entity;

    public bool pipeDreams;

    private void Update()
    {
        if(pipeDreams) PipeDreams();
    }

    float counter;

    void PipeDreams()
    {
        Debug.Log("count");
        counter += Time.deltaTime;
        if (counter >= Random.Range(15, 121))
        {
            Debug.Log("counter");
            if (Random.Range(0, 2) == 0)
                ModNetworkManager.Instance.InstantiateNetworkPrefab(entity.gameObject, null, Gamemode.instance.playerTransform.position - new Vector3(10, 0, 0), 
                    entity.transform.rotation, null, true);
            else
                ModNetworkManager.Instance.InstantiateNetworkPrefab(entity.gameObject, null, Gamemode.instance.playerTransform.position + new Vector3(10, 0, 0), 
                    new Quaternion(entity.transform.rotation.x, entity.transform.rotation.y + 180, entity.transform.rotation.z, entity.transform.rotation.w), null, true);
            counter = 0;
        }
    }
}
