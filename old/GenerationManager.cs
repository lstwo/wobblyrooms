using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModWobblyLife;
using ModWobblyLife.Network;
using UnityEngine.AI;
using OldWobblyrooms.MainMenu;

namespace OldWobblyrooms
{
    public class GenerationManager : ModNetworkBehaviour
    {
        public int seed = 42;

        [Header("Config")]
        public int mapSize = 16; // amount of rooms
        public float roomSize = 7; // size of every room (room size x = 7 AND room size z = 7)
        public Transform worldGrid; // parent of the rooms
        public int mapEmptyness; // chance of empty room spawning
        public int mapBrightness; // chance of light spawning
        public bool enableRareRooms; // whether to allow for rare rooms to spawn
        public int rareness = 25; // how rare they are (higher = rarer)
        public int rareAmount = 10; // how many to spawn in one area
        public NavMeshSurface surface;
        public List<NavMeshAgent> agents = new List<NavMeshAgent>();
        public GameObject moncher;
        public bool bUseRandomRot = true;

        [Header("Rooms")]
        public List<GameObject> roomTypes; // prefab of the rooms
        public GameObject emptyRoom; // empty room (generated more often)
        public List<GameObject> lightTypes; // prefab of the lights
        public List<GameObject> rareRooms; // rare variant of rooms

        private int mapSizeSqr; // amount of rooms in one row
        private Vector3 currentPos; // position of currently generating room

        private float currentPosX, currentPosZ; // position of the currently generating room
        private int currentPosTracker; // current room in the row (4 = 4th room in that row)

        private void Start()
        {

            mapSizeSqr = (int)Mathf.Sqrt(mapSize); // get the square root of the mapSize (amount of rooms in one row)
        }

        // generating all the rooms
        public IEnumerator GenerateWorld()
        {
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

                if (surface != null && Settings.entities)
                {
                    if (Random.Range(0, 178) == 1)
                    {
                        ModNetworkManager.Instance.InstantiateNetworkPrefab(moncher, (go) => agents.Add(go.transform.Find("level_3_moncher (1)").GetComponent<NavMeshAgent>()),
                            currentPos, moncher.transform.rotation, null, true);
                    }
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
    }
}