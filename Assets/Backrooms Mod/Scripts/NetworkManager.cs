using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.Network;
using UnityEngine.SceneManagement;

public class NetworkManager : ModNetworkBehaviour
{
    private byte GENERATOR_SEED;
    private byte LOAD_SCENE;

    public GameObject pipeRoomPrefab;
    public PipeDreamGenManager pipeDreamGenManager;

    [SerializeField] private GenerationManager generator;

    protected override void ModRegisterRPCs(ModNetworkObject modNetworkObject)
    {
        base.ModRegisterRPCs(modNetworkObject);

        GENERATOR_SEED = modNetworkObject.RegisterRPC(ClientSetSeed);
        LOAD_SCENE = modNetworkObject.RegisterRPC(ClientLoadScene);

        ServerGenSeed();
    }

    public void ServerGenSeed()
    {
        if (modNetworkObject == null || !modNetworkObject.IsServer()) return;
        if (GameSaves.GetSave(GameSaves.currentSave).seed == 0)
        {
            int seed = Random.Range(0, 3276718);
            modNetworkObject.SendRPC(GENERATOR_SEED, ModRPCRecievers.All, seed);
        } else
        {
            modNetworkObject.SendRPC(GENERATOR_SEED, ModRPCRecievers.All, GameSaves.GetSave(GameSaves.currentSave).seed);
        }
    }

    void ClientSetSeed(ModNetworkReader reader, ModRPCInfo info)
    {
        if (generator == null) return;
        generator.seed = reader.ReadInt32();
        generator.GenerateWorld();

        if (modNetworkObject.IsServer())
        {
            GameSave save = GameSaves.GetSave(GameSaves.currentSave);
            save.seed = generator.seed;
            GameSaves.SaveGame(save, save.level, save.name, save.seed);
        }
    }

    public void ServerLoadScene(int level)
    {
        if (modNetworkObject == null || !modNetworkObject.IsServer()) return;
        string sceneName = "Level " + level;
        GameSaves.SaveGame(GameSaves.GetSave(GameSaves.currentSave), level, GameSaves.GetSave(GameSaves.currentSave).name, GameSaves.GetSave(GameSaves.currentSave).seed);
        modNetworkObject.SendRPC(LOAD_SCENE, ModRPCRecievers.All, sceneName);
    }
    
    void ClientLoadScene(ModNetworkReader reader, ModRPCInfo info)
    {
        SceneManager.LoadScene(reader.ReadString());
    }
}
