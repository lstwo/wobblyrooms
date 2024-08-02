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
                GetComponent<NavMeshAgent>().SetDestination(player.GetPlayerPosition());
                timer = 0;
            }

            visual.LookAt(player.GetPlayerPosition());
            visual.Rotate(Vector3.up, visualsYRotOffset);
            visual.position = transform.position;
            visual.Translate(Vector3.up * visualsYPosOffset);
        }
    }
}