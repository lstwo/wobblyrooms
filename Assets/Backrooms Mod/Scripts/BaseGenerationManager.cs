using ModWobblyLife;
using ModWobblyLife.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wobblyrooms
{
    public abstract class BaseGenerationManager : ModNetworkBehaviour
    {
        /// <summary>
        /// called on awake for early initialization
        /// </summary>
        public abstract IEnumerator AwakeGen();

        /// <summary>
        /// called on start after game mode initialization for pre-generation
        /// </summary>
        public abstract IEnumerator StartGen();

        /// <summary>
        /// called on update for dynamic generation
        /// </summary>
        public abstract IEnumerator UpdateGen(List<ModPlayerController> controllers);

        /// <summary>
        /// called before new level gets loaded
        /// </summary>
        public abstract void EndGen();
    }
}
