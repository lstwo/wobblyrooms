using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Wobblyrooms.MainMenu;
using ModWobblyLife;

namespace Wobblyrooms
{
    public class AgentMonster : BaseMonster
    {
        [HideInInspector]
        public ModPlayerCharacter player;

        [Tooltip("IMPORTANT: Set to -90 for Level 6 and 3 Monster")]
        public float visualsYRotOffset;

        [Tooltip("IMPORTANT: Set to -1 for Level 6 Monster and to 0 for Level 3 Monster")]
        public float visualsYPosOffset;

        [Tooltip("Whether X and Z rotation should be locked")]
        public bool lockXZRotation = true;

        float timer = 0;

        private void Start()
        {
            var players = GameMode.Instance.playerControllers;
            var playerIndex = Random.Range(0, players.Count - 1);
            player = players[playerIndex].GetPlayerCharacter();
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer > 5)
            {
                GetComponentElseChildren<NavMeshAgent>().SetDestination(player.GetPlayerPosition());
                timer = 0;
            }

            visual.LookAt(player.GetPlayerPosition());
            visual.Rotate(Vector3.up, visualsYRotOffset);
            visual.position = transform.position;
            visual.Translate(Vector3.up * visualsYPosOffset);

            if(lockXZRotation) visual.rotation = Quaternion.Euler(new Vector3(0, visual.rotation.y, 0));       
        }

        T GetComponentElseChildren<T>()
        {
            if(TryGetComponent<T>(out var component))
            {
                return component;
            } else
            {
                return GetComponentInChildren<T>();
            }
        }
    }
}