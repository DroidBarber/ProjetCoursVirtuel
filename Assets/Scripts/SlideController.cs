using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Renderer))] // Oblige que l'objet qui possède ce script à posséder un Renderer, et s'il n'en a pas, en crée un
public class SlideController : MonoBehaviour
{
    public List<Texture2D> diapo = new List<Texture2D>();
    public bool isAutoChangeSlide = false;
    public float speedAutoChangeSlide = 5.0f;
    private Material material;
    private Renderer rendererObj;
    private int id_diapo_active = 0;
    private float timer = 0.0f; //initialise le timer à zéro
    private bool isAllDownload = false;
    public Log_UI logObj;
    public TextMeshProUGUI textLogSurDiapo;

    private void Awake()
    {
        rendererObj = this.GetComponent<Renderer>(); // Récuperation du renderer
        rendererObj.enabled = true; // Par défaut pas d'affichage d'image
        material = rendererObj.material; // Récuperation du material
        material.SetTexture("_MainTex", null); // Pas de texture(=image) par défaut
        StartCoroutine(GetDiapo());

        if (!logObj)
            Debug.LogError("logObj non assigné");

        if (!textLogSurDiapo)
            Debug.LogError("logObj non assigné");

    }

    void Start()
    {

    }

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

            //afficher un message ici pour les soucis de réseau
            textLogSurDiapo.text = "Vous avez un problème de réseau internet";
        }
        else
        {
            // Affichage dans la console des données reçues
            //Debug.Log(www.downloadHandler.text);

            // On récupère la liste des URL des images
            string lst_image = www.downloadHandler.text;

            // On passe d'un string contenant l'ensemble des URL, à une liste de string contenenant une URL chacune
            List<string> linesURL = new List<string>(
                lst_image.Split(new string[] { "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries));

            int nbDiapo = linesURL.Count;

            // Pour chaque URL d'image, on la télécharge et la range dans la variable diapo
            foreach (string url in linesURL)
            {
                www = UnityWebRequestTexture.GetTexture(url);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    //message pb internet
                    textLogSurDiapo.text = "Vous avez un problème de réseau internet";
                }
                else
                {
                    Texture tex = ((DownloadHandlerTexture)www.downloadHandler).texture; // Le (DownloadHandlerTexture) est un cast !
                    if (tex == null)
                    {
                        Debug.Log("Erreur lors du téléchargement de l'image: " + url);
                        //ERREUR le DL
                        textLogSurDiapo.text = "Erreur lors du téléchargement de l'image:\n" + url;
                    }
                    else
                    {
                        diapo.Add((Texture2D)tex);
                        //la diapo ajoute l'image du lien à sa diapo
                        //ajouter l'avancement du téléchargement ici
                        if(diapo.Count != linesURL.Count) // si on a pas encore télécharger toutes les images
                        {
                            textLogSurDiapo.text = "Téléchargement de la diapositive " + diapo.Count + "/" + linesURL.Count;

                            // lag artificiel lor du téléchargement des images pour voir l'affichage
                            yield return new WaitForSeconds(2);

                        }
                        else
                        {
                            textLogSurDiapo.text = "";
                            isAllDownload = true;
                        }
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
        //execute la fonction uniquement si le tableau est activé et si le diapo est chargé
        if (!rendererObj.enabled || diapo.Count <= 0) return;

        if (isAutoChangeSlide && isAllDownload)
        {
            timer += Time.fixedDeltaTime;
            if (timer < speedAutoChangeSlide) return;
            id_diapo_active = (id_diapo_active + 1) % diapo.Count; //index diapo suivante
            material.SetTexture("_MainTex", diapo[id_diapo_active]); //charge la diapo suivante
            timer -= speedAutoChangeSlide;
            //logObj.AjoutLog("Id_diapo_active : " + id_diapo_active, speedAutoChangeSlide);// affichage des logs dans le canvas
        }
    }
}
