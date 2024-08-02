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
        public NavMeshSurface surface;
        public List<NavMeshAgent> agents = new List<NavMeshAgent>();
        public GameObject moncher;

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
                    if (Random.Range(0, 178) == 1)
                    {
                        ModNetworkManager.Instance.InstantiateNetworkPrefab(moncher, (go) => agents.Add(go.transform.Find("level_3_moncher (1)").GetComponent<NavMeshAgent>()),
                            currentPos, moncher.transform.rotation, null, true);
                    }
                }

                currentPosTracker++; // set the tracker to the next room
                currentPosX += roomSize; // move to current x position to next to the last room
            }
        }
    }
}
