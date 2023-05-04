using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.Network;

public class NetworkManager : ModNetworkBehaviour
{
    private byte GENERATOR_SEED;

    [SerializeField] private GenerationManager generator;

    protected override void ModRegisterRPCs(ModNetworkObject modNetworkObject)
    {
        base.ModRegisterRPCs(modNetworkObject);

        GENERATOR_SEED = modNetworkObject.RegisterRPC(ClientSetSeed);
        Debug.Log("RPC");

        ServerGenSeed();
    }

    public void ServerGenSeed()
    {
        if (modNetworkObject == null || !modNetworkObject.IsServer()) return;
        int seed = Random.Range(0, 3276718);
        modNetworkObject.SendRPC(GENERATOR_SEED, ModRPCRecievers.All, seed);
        Debug.Log("ServerSeed");
    }

    void ClientSetSeed(ModNetworkReader reader, ModRPCInfo info)
    {
        generator.seed = reader.ReadInt32();
        generator.GenerateWorld();
        Debug.Log("ClientSeed");
    }
}
