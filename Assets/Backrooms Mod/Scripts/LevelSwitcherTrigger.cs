using ModWobblyLife;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSwitcherTrigger : MonoBehaviour
{
    public int level;
    public NetworkManager networkManager;

    private void OnTriggerEnter(Collider other)
    {
        NetworkManager.instance.ServerLoadScene(level);
    }
}
