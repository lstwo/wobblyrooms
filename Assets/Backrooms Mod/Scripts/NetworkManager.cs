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
        if (modNetworkObject == null || !modNetworkObject.IsServer()) return;
        int seed = Random.Range(0, 3276718);
        modNetworkObject.SendRPC(GENERATOR_SEED, ModRPCRecievers.All, seed);
    }

    void ClientSetSeed(ModNetworkReader reader, ModRPCInfo info)
    {
        if (generator == null) return;
        generator.seed = reader.ReadInt32();
        generator.GenerateWorld();
    }

    public void ServerSpawnPipeRooms(PipeDreamGenManager pdgm)
    {

    }

    void ClientSpawnPipeRooms(ModNetworkReader reader, ModRPCInfo info)
    {

    }
}
