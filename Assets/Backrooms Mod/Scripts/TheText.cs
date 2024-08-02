using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wobblyrooms
{
    public class TheText : MonoBehaviour
    {
        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return new WaitForSeconds(10.0f);
            GetComponent<TextMeshProUGUI>().enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                GameMode.Instance.networkingManager.ServerGenSeed(true);
                GameMode.Instance.LoadLevel(int.Parse(SceneManager.GetActiveScene().name.Split(' ')[1].Trim()));
            }
        }
    }
}
