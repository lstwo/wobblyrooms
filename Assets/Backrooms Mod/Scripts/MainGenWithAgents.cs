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

        private int _mapSizeSqr;
        private Vector3 _currentPos; // position of currently generating room

        private float _currentPosX, _currentPosZ; // position of the currently generating room
        private int _currentPosTracker; // current room in the row (4 = 4th room in that row)

        public override IEnumerator StartGen()
        {
            yield return base.StartGen();

            for (int i = 0; i < mapSize; i++)
            {
                _mapSizeSqr = (int)Mathf.Sqrt(mapSize); // get the square root of the mapSize (amount of rooms in one row)

                if (_currentPosTracker == _mapSizeSqr) // go to new room if row is 'full'
                {
                    _currentPosX = 0; // resets the current x position
                    _currentPosZ += roomSize; // moves z position to new row

                    _currentPosTracker = 0; // reset currentPosTracker for new row
                }
                _currentPos = new Vector3(_currentPosX, 0, _currentPosZ); // set the position for currently generating room

                if (surface != null && Settings.entities)
                {
                    if (Random.Range(0, monsterChance) == 1)
                    {
                        ModNetworkManager.Instance.InstantiateNetworkPrefab(moncher, (go) => agents.Add(go.gameObject.GetComponentElseChildren<NavMeshAgent>(true)),
                            _currentPos, moncher.transform.rotation, null, true);
                    }
                }

                _currentPosTracker++; // set the tracker to the next room
                _currentPosX += roomSize; // move to current x position to next to the last room
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
