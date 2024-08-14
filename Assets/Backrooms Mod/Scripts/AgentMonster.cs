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
        public ModPlayerController player;

        [Tooltip("IMPORTANT: Set to -90 for Level 6 and 3 Monster")]
        public float visualsYRotOffset;

        [Tooltip("IMPORTANT: Set to -1 for Level 6 Monster and to 0 for Level 3 Monster")]
        public float visualsYPosOffset;

        [Tooltip("Whether X and Z rotation should be locked")]
        public bool lockXZRotation = true;

        float timer = 0;

        protected void Start()
        {
            SelectPlayerOnStart();
        }

        protected void Update()
        {
            HandleAgent();
            SetRotation();
            SetPosition();
        }

        /// <summary>
        /// Used to select a random player on start. Override to disable or change functionality.
        /// </summary>
        protected void SelectPlayerOnStart()
        {
            var players = GameMode.Instance.playerControllers;
            var playerIndex = Random.Range(0, players.Count - 1);
            player = players[playerIndex];
        }

        /// <summary>
        /// Sets the agents target position.
        /// Gets called every frame!
        /// </summary>
        protected void HandleAgent()
        {
            timer += Time.deltaTime;
            if (timer > 5)
            {
                var agent = this.GetComponentElseChildren<NavMeshAgent>();

                if (agent != null)
                {
                    agent.SetDestination(player.GetPlayerCharacter().GetPlayerPosition());
                }

                timer = 0;
            }
        }

        /// <summary>
        /// Sets the position of the visuals to the agent. Override for complex behaviour.
        /// Gets called every frame!
        /// </summary>
        protected void SetPosition()
        {
            visual.position = transform.position;
            visual.Translate(Vector3.up * visualsYPosOffset);
        }

        /// <summary>
        /// Sets the rotation to look at the player. Override for complex behaviour.
        /// Gets called every frame!
        /// </summary>
        protected void SetRotation()
        {
            visual.LookAt(player.GetPlayerCharacter().GetPlayerPosition());
            visual.Rotate(Vector3.up, visualsYRotOffset);

            if(lockXZRotation) visual.rotation = Quaternion.Euler(new Vector3(0, visual.rotation.y, 0));
        }
    }
}