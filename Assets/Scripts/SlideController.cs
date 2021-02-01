using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;

public class SlideController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Texture2D> diapo = new List<Texture2D>();
    private Material mat;
    private Renderer rendererObj;
    private int id_diapo_active = 0;

    private void Awake()
    {
        if (this.GetComponent<Renderer>())
        {
            rendererObj = this.GetComponent<Renderer>(); //récuperation du renderer
            rendererObj.enabled = false; // par défaut pas d'affichage d'image
            mat = rendererObj.material; // récuperation du material
            mat.SetTexture("_MainTex", null); // Pas de texture(=image) par défaut
        }
        else
        {
            Debug.LogError("Missing Renderer");
        }

        StartCoroutine(Check());

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))//appuyer sur M pour activer ou desactiver le diapo
        {
            rendererObj.enabled = !rendererObj.enabled;
            mat.SetTexture("_MainTex", diapo[id_diapo_active]);
            Debug.Log(id_diapo_active);
        }
        else if (Input.GetKeyUp(KeyCode.O)) //diapo 1
        {
            id_diapo_active = (id_diapo_active + 1) % diapo.Count;
            Debug.Log(id_diapo_active);
            mat.SetTexture("_MainTex", diapo[id_diapo_active]);
        }
        else if (Input.GetKeyUp(KeyCode.I)) //diapo 2
        {
            id_diapo_active = (id_diapo_active - 1) >= 0 ? id_diapo_active - 1 : diapo.Count - 1;
            Debug.Log(id_diapo_active);
            mat.SetTexture("_MainTex", diapo[id_diapo_active]);
            mat.SetTexture("_MainTex", diapo[id_diapo_active]);
        }
    }

    private IEnumerator Check()
    {

        UnityWebRequest www = UnityWebRequest.Get("https://pastebin.com/raw/8e3DsJ2V");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

        string lst_image = www.downloadHandler.text;
        List<string> lines = new List<string>(
            lst_image.Split(new string[] { "\r", "\n" },
            StringSplitOptions.RemoveEmptyEntries));

        foreach (string url in lines)
        {
            www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                diapo.Add(((DownloadHandlerTexture)www.downloadHandler).texture);
            }
        }
    }
}
