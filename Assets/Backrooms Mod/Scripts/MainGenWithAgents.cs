using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using ModWobblyLife.Network;
using Wobblyrooms.MainMenu;

namespace Wobblyrooms
{
    public class MainGenWithAgents : MainGenerationManager
    {
        [Header("Agent Settings")]

        [Tooltip("The surface component of your nav mesh ground")]
        public NavMeshSurface surface;

        [Tooltip("The currently spawned in agents")]
        public List<NavMeshAgent> agents = new List<NavMeshAgent>();

        [Tooltip("The monster prefab to spawn")]
        public GameObject moncher;

        [Tooltip("The chance for the moster to spawn for each tile (higher number = less chance)")]
        public int monsterChance = 178;

        private int mapSizeSqr; // amount of rooms in one row
        private Vector3 currentPos; // position of currently generating room

        private float currentPosX, currentPosZ; // position of the currently generating room
        private int currentPosTracker; // current room in the row (4 = 4th room in that row)

        public override IEnumerator StartGen()
        {
            yield return base.StartGen();

            for (int i = 0; i < mapSize; i++)
            {
                mapSizeSqr = (int)Mathf.Sqrt(mapSize); // get the square root of the mapSize (amount of rooms in one row)

                if (currentPosTracker == mapSizeSqr) // go to new room if row is 'full'
                {
                    currentPosX = 0; // resets the current x position
                    currentPosZ += roomSize; // moves z position to new row

                    currentPosTracker = 0; // reset currentPosTracker for new row
                }
                currentPos = new Vector3(currentPosX, 0, currentPosZ); // set the position for currently generating room

                if (surface != null && Settings.entities)
                {
                    if (Random.Range(0, monsterChance) == 1)
                    {
                        ModNetworkManager.Instance.InstantiateNetworkPrefab(moncher, (go) => agents.Add(go.transform.GetComponentInChildren<NavMeshAgent>(true)),
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
