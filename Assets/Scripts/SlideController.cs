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
    private Material material;
    private Renderer rendererObj;
    private int id_diapo_active = 0;

    private void Awake()
    {
        if (this.GetComponent<Renderer>()) // Test si le Component existe sur l'objet courant
        {
            rendererObj = this.GetComponent<Renderer>(); // Récuperation du renderer
            rendererObj.enabled = false; // Par défaut pas d'affichage d'image
            material = rendererObj.material; // Récuperation du material
            material.SetTexture("_MainTex", null); // Pas de texture(=image) par défaut
            StartCoroutine(GetDiapo());
        }
        else
        {
            Debug.LogError("Renderer manquant pour le " + this.name);
        }
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
            if (diapo.Count != 0)
            {
                material.SetTexture("_MainTex", diapo[id_diapo_active]);
            }
            else
            {
                material.SetTexture("_MainTex", null);
            }
        }
        else if (Input.GetKeyUp(KeyCode.O)) //diapo suivante
        {
            id_diapo_active = (id_diapo_active + 1) % diapo.Count;
            material.SetTexture("_MainTex", diapo[id_diapo_active]);
        }
        else if (Input.GetKeyUp(KeyCode.I)) //diapo précédente
        {
            id_diapo_active = (id_diapo_active - 1) >= 0 ? id_diapo_active - 1 : diapo.Count - 1;
            material.SetTexture("_MainTex", diapo[id_diapo_active]);
        }
    }

    private IEnumerator GetDiapo()
    {

        UnityWebRequest www = UnityWebRequest.Get("https://pastebin.com/raw/8e3DsJ2V");
        yield return www.SendWebRequest(); // envoie de la requète à l'URL

        if (www.isNetworkError || www.isHttpError) // Si un problème de connexion
        {
            Debug.Log(www.error);
        }
        else
        {
            // Affichage dans la console des données reçues
            //Debug.Log(www.downloadHandler.text);

            // On passe d'un string contenant l'ensemble des URL, à une liste de string contenenat une URL chacun
            string lst_image = www.downloadHandler.text;
            List<string> lines = new List<string>(
                lst_image.Split(new string[] { "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries));

            // Pour chaque URL d'image, on la télécharge et la range dans la variable diapo
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
}
