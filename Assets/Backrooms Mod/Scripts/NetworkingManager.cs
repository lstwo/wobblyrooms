using ModWobblyLife.Network;
using Wobblyrooms.MainMenu;

namespace Wobblyrooms
{
    public class NetworkingManager : ModNetworkBehaviour
    {
        public static int num;
        public static bool temp;

        private byte RPC_LOAD_SCENE;
        private byte RPC_GEN_SEED;

        protected override void ModRegisterRPCs(ModNetworkObject modNetworkObject)
        {
            base.ModRegisterRPCs(modNetworkObject);

            RPC_LOAD_SCENE = modNetworkObject.RegisterRPC(ClientLoadScene);
            RPC_GEN_SEED = modNetworkObject.RegisterRPC(ClientSetSeed);
        }

        /// <summary>
        /// Loads a Scene by the scene name
        /// </summary>
        /// <param name="sceneName"></param>
        public void ServerLoadScene(string sceneName)
        {
            if (modNetworkObject == null) return;

            modNetworkObject.SendRPC(RPC_LOAD_SCENE, ModRPCRecievers.Others, sceneName);
            LoadScene(sceneName);
        }

        private void ClientLoadScene(ModNetworkReader reader, ModRPCInfo info)
        {
            LoadScene(reader.ReadString());
        }

        private void LoadScene(string sceneName)
        {
            ModScenes.Load(sceneName);
        }

        /// <summary>
        /// Sets the seed for all clients and / or generates a new one
        /// </summary>
        /// <param name="overrideSave"></param>
        public void ServerGenSeed(bool overrideSave)
        {
            if (modNetworkObject == null || !modNetworkObject.IsServer()) return;

            int seed;
            if (overrideSave) seed = UnityEngine.Random.Range(0, int.MaxValue);
            else seed = GameSaves.GetSave(GameSaves.currentSave).seed;

            modNetworkObject.SendRPC(RPC_GEN_SEED, ModRPCRecievers.All, seed);
        }

        void ClientSetSeed(ModNetworkReader reader, ModRPCInfo info)
        {
            UnityEngine.Random.InitState(reader.ReadInt32());

            if (modNetworkObject != null && modNetworkObject.IsServer() && GameSaves.currentSave != 0)
            {
                GameSave save = GameSaves.GetSave(GameSaves.currentSave);
                save.seed = reader.ReadInt32();
                GameSaves.SaveGame(save, save.level, save.name, save.seed, save.hardcore);
            }
        }


        public static void SetNum(int i)
        {
            num = i;
        }
    }
}
