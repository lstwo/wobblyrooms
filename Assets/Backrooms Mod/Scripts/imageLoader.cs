using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class imageLoader : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DownloadImage("https://i.imgur.com/xfH4pGp.png"));
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            GetComponent<RawImage>().texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }
}
