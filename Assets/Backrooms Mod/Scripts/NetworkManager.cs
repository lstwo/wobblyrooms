using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.Network;
using UnityEngine.SceneManagement;
using System;
using static Cinemachine.DocumentationSortingAttribute;

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
        instance.ServerGenSeed(false);
    }

    protected override void ModRegisterRPCs(ModNetworkObject modNetworkObject)
    {
        base.ModRegisterRPCs(modNetworkObject);

        GENERATOR_SEED = modNetworkObject.RegisterRPC(ClientSetSeed);
        LOAD_SCENE = modNetworkObject.RegisterRPC(ClientLoadScene);
    }

    public void ServerGenSeed(bool _override)
    {
        if (modNetworkObject == null || !modNetworkObject.IsServer()) return;

        int seed;
        if (_override) seed = UnityEngine.Random.Range(0, int.MaxValue);
        else seed = GameSaves.GetSave(GameSaves.currentSave).seed;

        modNetworkObject.SendRPC(GENERATOR_SEED, ModRPCRecievers.All, seed);
    }

    void ClientSetSeed(ModNetworkReader reader, ModRPCInfo info)
    {
        if (generator != null)
            generator.seed = reader.ReadInt32();

        UnityEngine.Random.InitState(reader.ReadInt32());

        if (modNetworkObject != null && modNetworkObject.IsServer())
        {
            GameSave save = GameSaves.GetSave(GameSaves.currentSave);
            save.seed = reader.ReadInt32();
            GameSaves.SaveGame(save, save.level, save.name, save.seed);
        }

        if (generator != null)
        {
            StartCoroutine(generator.GenerateWorld());
        }
    }

    public void ServerLoadScene(int level)
    {
        Debug.Log(modNetworkObject.IsServer());
        if (modNetworkObject == null) return;
        string sceneName = "Level " + level;
        GameSaves.SaveGame(GameSaves.GetSave(GameSaves.currentSave), level, GameSaves.GetSave(GameSaves.currentSave).name, GameSaves.GetSave(GameSaves.currentSave).seed);
        modNetworkObject.SendRPC(LOAD_SCENE, ModRPCRecievers.All, sceneName);
    }
    
    void ClientLoadScene(ModNetworkReader reader, ModRPCInfo info)
    {
        string sceneName = reader.ReadString();

        if (sceneName.StartsWith("Level") && GameSaves.currentSave != 4)
            Achievements.CompleteAchievement(Achievements.MappedToLevel[int.Parse(sceneName.Split(' ')[1].Trim())].id);

        try 
        {
            ModScenes.Load(sceneName);
        } 
        catch(NullReferenceException e)
        {
            Debug.Log(sceneName);
            Debug.Log(e.StackTrace);
            SceneManager.LoadScene(sceneName);
        }
    }
}
