using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.Network;
using OldWobblyrooms;

namespace Wobblyrooms.Level2
{
    public class EntitySpawner : MonoBehaviour
    {
        public ModNetworkBehaviour entity;

        float counter;

        private void Update()
        {
            Debug.Log("count");
            counter += Time.deltaTime;
            if (counter >= Random.Range(1, 5))
            {
                Debug.Log("counter");
                if (Random.Range(0, 2) == 0)
                    ModNetworkManager.Instance.InstantiateNetworkPrefab(entity.gameObject, null, Gamemode.playerTransform.position - new Vector3(10, 0, 0),
                        entity.transform.rotation, null, true);
                else
                    ModNetworkManager.Instance.InstantiateNetworkPrefab(entity.gameObject, null, Gamemode.playerTransform.position + new Vector3(10, 0, 0),
                        new Quaternion(entity.transform.rotation.x, entity.transform.rotation.y + 180, entity.transform.rotation.z, entity.transform.rotation.w), null, true);
                counter = 0;
            }
        }
    }
}