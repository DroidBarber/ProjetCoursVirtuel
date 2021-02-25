using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SlideController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Texture2D> diapo = new List<Texture2D>();
    private Material material;
    private Renderer rendererObj;
    private int id_diapo_active = 0;
    private float timer = 0.0f; //initialise le timer à zéro
    private GameObject t;

    private void Awake()
    {
        Debug.Log(this.name);
        if (this.GetComponent<Renderer>()) // Test si le Component existe sur l'objet courant
        {
            rendererObj = this.GetComponent<Renderer>(); // Récuperation du renderer
            rendererObj.enabled = true; // Par défaut pas d'affichage d'image
            material = rendererObj.material; // Récuperation du material
            material.SetTexture("_MainTex", null); // Pas de texture(=image) par défaut
            StartCoroutine(GetDiapo());
        }
        else
        {
            Debug.LogError("Renderer manquant pour le " + this.name);
        }
        t = GameObject.Find("Log_UI");
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))//appuyer sur P pour activer ou desactiver le diapo
        {
            rendererObj.enabled = !rendererObj.enabled; //change l'état du diapo (activé ou non)
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
    private void FixedUpdate()
    {
        if (!rendererObj.enabled || diapo.Count <= 0) return; //execute la fonction uniquement si le tableau est activé
        //et si le diapo est chargé
        timer += Time.deltaTime;
        if (!(timer > 1.0f)) return;
       // Debug.Log("Id_diapo_active : " + id_diapo_active);
        id_diapo_active = (id_diapo_active+1)%diapo.Count; //index diapo suivante
        material.SetTexture("_MainTex", diapo[id_diapo_active]); //charge la diapo suivante
        timer -= 1.0f;
        t.GetComponent<Log_UI>().AjoutLog("Id_diapo_active : " + id_diapo_active);// affichage des logs dans le canvas

    }
}
