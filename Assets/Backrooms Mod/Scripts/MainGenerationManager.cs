using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModWobblyLife;
using ModWobblyLife.Network;
using UnityEngine.AI;
using Wobblyrooms.MainMenu;

namespace Wobblyrooms
{
    public class MainGenerationManager : BaseGenerationManager
    {
        public int seed = 42;

        [Header("Config")]

        [Tooltip("Amount of Rooms")]
        public int mapSize = 16;

        [Tooltip("size of every room (room size x = 7 AND room size z = 7)")]
        public float roomSize = 7;

        [Tooltip("parent of the rooms")]
        public Transform worldGrid;

        [Tooltip("chance of empty room spawning")]
        public int mapEmptyness;

        [Tooltip("chance of light spawning")]
        public int mapBrightness;

        [Tooltip("Amount of Rooms")]
        public bool bUseRandomRot = true;


        [Header("Rare Rooms Settings")]

        [Tooltip("whether to allow for rare rooms to spawn")]
        public bool enableRareRooms;

        [Tooltip("how rare they are (higher = rarer)")]
        public int rareness = 25;

        [Tooltip("how many to spawn in one area")]
        public int rareAmount = 10;


        [Header("Rooms")]

        [Tooltip("prefabs of the rooms")]
        public List<GameObject> roomTypes;

        [Tooltip("empty room (generated more often)")]
        public GameObject emptyRoom;

        [Tooltip("rare variant of rooms")]
        public List<GameObject> rareRooms;


        private int mapSizeSqr; // amount of rooms in one row
        private Vector3 currentPos; // position of currently generating room

        private float currentPosX, currentPosZ; // position of the currently generating room
        private int currentPosTracker; // current room in the row (4 = 4th room in that row)

        public override IEnumerator AwakeGen()
        {
            yield break;
        }

        public override IEnumerator StartGen()
        {
            mapSizeSqr = (int)Mathf.Sqrt(mapSize); // get the square root of the mapSize (amount of rooms in one row)

            yield return null;

            if (!ModNetworkManager.Instance.IsServer()) yield break;

            for (int i = 0; i < mapEmptyness; i++)
                roomTypes.Add(emptyRoom); // adds empty rooms to the roomtypes array

            int _rareAmount = rareAmount;
            for (int i = 0; i < mapSize; i++) // generate each room
            {
                if (currentPosTracker == mapSizeSqr) // go to new room if row is 'full'
                {
                    currentPosX = 0; // resets the current x position
                    currentPosZ += roomSize; // moves z position to new row

                    currentPosTracker = 0; // reset currentPosTracker for new row
                }
                currentPos = new Vector3(currentPosX, 0, currentPosZ); // set the position for currently generating room

                // whether to spawn the rare rooms or the normal rooms
                if (enableRareRooms)
                {
                    if (Random.Range(0, rareness) == rareness-1 || _rareAmount != rareAmount)
                    {
                        float rot;
                        int room = Random.Range(0, rareRooms.Count);

                        if (bUseRandomRot)
                            rot = Random.Range(0, 4) * 90;
                        else
                            rot = rareRooms[room].transform.rotation.y;

                        ModNetworkManager.Instance.InstantiateNetworkPrefab(rareRooms[room], (go) => go.transform.SetParent(worldGrid), currentPos,
                            Quaternion.Euler(0, rot, 0), null, true); // generating the rare room for rareAmount times

                        _rareAmount--; // counting down to indicate that one has been generated

                        if (_rareAmount == 0)
                            _rareAmount = rareAmount; // reset rareAmount once all of them have been generated
                    }
                    else
                    {
                        int rand = Random.Range(0, roomTypes.Count);
                        float rot;

                        if (bUseRandomRot)
                            rot = Random.Range(0, 4) * 90;
                        else
                            rot = roomTypes[rand].transform.rotation.y;

                        if (rot == 0.7071068f) rot = 90; else if (rot == 1) rot = 180; else if (rot == -0.7071068f) rot = -90f;

                        ModNetworkManager.Instance.InstantiateNetworkPrefab(roomTypes[rand], (go) => go.transform.SetParent(worldGrid), currentPos, Quaternion.Euler(0, rot, 0), null, true); // create the room
                    }
                }
                else
                {
                    int rand = Random.Range(0, roomTypes.Count);
                    int rot = Random.Range(0, 4) * 90;
                    ModNetworkManager.Instance.InstantiateNetworkPrefab(roomTypes[rand], (go) => go.transform.SetParent(worldGrid), currentPos, Quaternion.Euler(0, rot, 0), null, true); // create the room
                }

                currentPosTracker++; // set the tracker to the next room
                currentPosX += roomSize; // move to current x position to next to the last room
            }

            if (surface == null && agents.Count < 1) yield break;

            surface.agentTypeID = agents[0].agentTypeID;
            surface.BuildNavMesh();

            foreach (NavMeshAgent agent in agents)
            {
                agent.gameObject.SetActive(true);
            }
        }

        public override IEnumerator UpdateGen(List<ModPlayerController> controllers)
        {
            yield break;
        }

        public override void EndGen()
        {
        }
    }
}