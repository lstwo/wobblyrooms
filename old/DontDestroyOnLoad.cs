using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    public string inScene;
    public GameObject _gameObject;

    void Awake()
    {
        DontDestroyOnLoad(_gameObject);
        DontDestroyOnLoad(this);
        if(inScene != null || !inScene.Equals(""))
        {
            _gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == inScene)
        {
            _gameObject.SetActive(true);
        } else
        {
            _gameObject.SetActive(false);
        }
    }
}
