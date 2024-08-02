using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Wobblyrooms.MainMenu;

namespace Wobblyrooms
{
    public class Level6Monster : BaseMonster
    {
        [HideInInspector]
        public Transform player;

        // IMPORTANT: Set to -90 for Level 6 Monster
        public float visualsYRotOffset;

        // IMPORTANT: Set to -1 for Level 6 Monster
        public float visualsYPosOffset;

        float timer = 0;

        void Update()
        {
            var players = GameMode.Instance.playerControllers;
            var playerIndex = Random.Range(0, players.Count - 1);
            player = players[playerIndex].GetPlayerTransform();

            timer += Time.deltaTime;
            if (timer > 5)
            {
                GetComponent<NavMeshAgent>().SetDestination(player.position);
                timer = 0;
            }

            visual.Rotate(Vector3.up, visualsYRotOffset);
            visual.LookAt(player.position);
            visual.position = transform.position;
            visual.Translate(Vector3.up * visualsYPosOffset);
        }
    }
}