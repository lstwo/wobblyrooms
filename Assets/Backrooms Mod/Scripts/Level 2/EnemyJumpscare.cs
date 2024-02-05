using ModWobblyLife;
using ModWobblyLife.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyJumpscare : ModNetworkBehaviour
{
    bool activated = false;

    public GameObject jumpscare;

    private byte RPC_JUMPSCARE;

    protected override void ModRegisterRPCs(ModNetworkObject modNetworkObject)
    {
        base.ModRegisterRPCs(modNetworkObject);

        RPC_JUMPSCARE = modNetworkObject.RegisterRPC(ClientJumpscare);
    }

    private void Start()
    {
        for (int i = 0; i < SceneManager.GetActiveScene().GetRootGameObjects().Length; i++)
        {
            if (SceneManager.GetActiveScene().GetRootGameObjects()[i].name == "JumpscareCanvas")
            {
                jumpscare = SceneManager.GetActiveScene().GetRootGameObjects()[i].transform.Find("Panel").gameObject; 
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Gamemode.instance.jumpscares || modNetworkObject == null || !modNetworkObject.IsServer()) return;
        if(other.tag == "Player" && !activated)
        {
            activated = true;
            modNetworkObject.SendRPC(RPC_JUMPSCARE, other.GetComponentInParent<ModPlayerCharacter>().modNetworkObject.GetOwner());
        }
    }

    void ClientJumpscare(ModNetworkReader reader, ModRPCInfo info)
    {
        jumpscare.SetActive(true);
        StartCoroutine(ResetJumpscare());
    }

    IEnumerator ResetJumpscare()
    {
        yield return new WaitForSeconds(0.3175f);
        Destroy(gameObject);
        jumpscare.SetActive(false);
    }
}
