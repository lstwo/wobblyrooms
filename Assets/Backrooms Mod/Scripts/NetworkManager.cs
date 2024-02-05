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
    public Level2GenManager pipeDreamGenManager;

    public static NetworkManager instance;

    [SerializeField] private GenerationManager generator;

    private void Awake()
    {
        instance = this;
    }

    protected override void ModNetworkStart(ModNetworkObject modNetworkObject)
    {
        base.ModNetworkStart(modNetworkObject);
        instance.ServerGenSeed();
    }

    protected override void ModRegisterRPCs(ModNetworkObject modNetworkObject)
    {
        base.ModRegisterRPCs(modNetworkObject);

        GENERATOR_SEED = modNetworkObject.RegisterRPC(ClientSetSeed);
        LOAD_SCENE = modNetworkObject.RegisterRPC(ClientLoadScene);
    }

    public void ServerGenSeed()
    {
        if (modNetworkObject == null || !modNetworkObject.IsServer()) return;
        Debug.Log("logg");
        int seed = Random.Range(0, int.MaxValue);
        modNetworkObject.SendRPC(GENERATOR_SEED, ModRPCRecievers.All, seed);
    }

    void ClientSetSeed(ModNetworkReader reader, ModRPCInfo info)
    {
        if (generator != null)
            generator.seed = reader.ReadInt32();

        Random.InitState(reader.ReadInt32());

        if (modNetworkObject != null && modNetworkObject.IsServer())
        {
            GameSave save = GameSaves.GetSave(GameSaves.currentSave);
            save.seed = Random.seed;
            GameSaves.SaveGame(save, save.level, save.name, save.seed);
        }


        Debug.Log("loggg");

        if (generator != null)
        {
            generator.GenerateWorld();
            Debug.Log("logggg");
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
        ModScenes.Load(reader.ReadString());
    }
}
