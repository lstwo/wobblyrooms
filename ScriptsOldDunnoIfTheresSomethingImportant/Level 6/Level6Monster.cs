﻿using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Level6Monster : MonoBehaviour
{
    public Transform player;
    public Transform visual;

    float timer = 0;

    void Update()
    {
        player = Gamemode.instance.playerTransform;
        timer += Time.deltaTime;
        if (timer > 5)
        {
            GetComponent<NavMeshAgent>().SetDestination(player.position);
            timer = 0;
        }
        visual.Rotate(Vector3.up, -90);
        visual.LookAt(player.position);
        visual.position = transform.position;
        visual.Translate(Vector3.up * -1f);
    }

    public GameObject jumpscare;

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

        if (other.tag == "Player")
        {
            if (Settings.jumpscares)
                jumpscare.SetActive(true);
            StartCoroutine(ResetJumpscare());
        }
    }

    IEnumerator ResetJumpscare()
    {
        yield return new WaitForSeconds(0.3175f);
        jumpscare.SetActive(false);
        transform.position = new Vector3(Random.Range(0, 150), 2.9f, Random.Range(0, 150));
    }
}