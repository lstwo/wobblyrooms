using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.Network;

public class NetworkManager : ModNetworkBehaviour
{
    private byte GENERATOR_SEED;
    private byte SPAWN_PIPEROOMS;

    public GameObject pipeRoomPrefab;
    public PipeDreamGenManager pipeDreamGenManager;

    [SerializeField] private GenerationManager generator;

    protected override void ModRegisterRPCs(ModNetworkObject modNetworkObject)
    {
        base.ModRegisterRPCs(modNetworkObject);

        GENERATOR_SEED = modNetworkObject.RegisterRPC(ClientSetSeed);
        SPAWN_PIPEROOMS = modNetworkObject.RegisterRPC(ClientSpawnPipeRooms);

        ServerGenSeed();
    }

    public void ServerGenSeed()
    {
        if(GameSaves.GetSave(GameSaves.currentSave).seed == 0)
        {
            if (modNetworkObject == null || !modNetworkObject.IsServer()) return;
            int seed = Random.Range(0, 3276718);
            modNetworkObject.SendRPC(GENERATOR_SEED, ModRPCRecievers.All, seed);
        } else
        {
            if (modNetworkObject == null || !modNetworkObject.IsServer()) return;
            modNetworkObject.SendRPC(GENERATOR_SEED, ModRPCRecievers.All, GameSaves.GetSave(GameSaves.currentSave));
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

    public void ServerSpawnPipeRooms(PipeDreamGenManager pdgm)
    {

    }

    void ClientSpawnPipeRooms(ModNetworkReader reader, ModRPCInfo info)
    {

    }
}
