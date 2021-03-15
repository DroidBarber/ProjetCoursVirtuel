using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Renderer))] // Oblige que l'objet qui possède ce script à posséder un Renderer, et s'il n'en a pas, en crée un
[RequireComponent(typeof(PhotonView))]
public class SlideController : MonoBehaviourPunCallbacks
{
    public List<Texture2D> diapo = new List<Texture2D>();
    public bool isAutoChangeSlide = false;
    public bool isOnlyMasterClientCanChangeDiapo = false;
    public float speedAutoChangeSlide = 5.0f;
    private Material material;
    private Renderer rendererObj;
    private int id_diapo_active = 0;
    private float timer = 0.0f; //initialise le timer à zéro
    private bool isAllDownload = false;
    private bool isNeedChangeTexture = false;
    public Log_UI logObj;
    public TextMeshProUGUI textLogSurDiapo;
    public RectTransform BarreChargementFull;
    public RectTransform barreChargement;
    private float xBarreChargement;
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
        if(!BarreChargementFull)
            Debug.LogError("logObj non assigné");
        if (!barreChargement)
            Debug.LogError("logObj non assigné");

        barreChargement.anchorMax = new Vector2((float)0.1, (float)0.2);
        //BarreChargement.rect.width = 0;
        
        xBarreChargement = (float)0.1; //0.1f
}

    public override void OnJoinedRoom()
    {
        // Si c'est le MasterClient, c'est lui la référence, donc il va pas se demander à lui même
        if (!PhotonNetwork.IsMasterClient) 
        {
            // Demande la synchronisation de l'id_diapo_active
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RequireSyncDiapo", RpcTarget.MasterClient);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (isAllDownload)
        {
            if (isNeedChangeTexture)
            {
                material.SetTexture("_MainTex", diapo[id_diapo_active]);
                isNeedChangeTexture = false;
            }
            if (!isOnlyMasterClientCanChangeDiapo || PhotonNetwork.IsMasterClient)
            {
                if (Input.GetKeyUp(KeyCode.P))//appuyer sur P pour activer ou desactiver le diapo
                {
                    rendererObj.enabled = !rendererObj.enabled; //change l'état du diapo (activé ou non)
                }
                else if (Input.GetKeyUp(KeyCode.O) || OVRInput.GetUp(OVRInput.RawButton.X)) //diapo suivante
                {

                    DiapoNext();
                }
                else if (Input.GetKeyUp(KeyCode.I) || OVRInput.GetUp(OVRInput.RawButton.Y)) //diapo précédente
                {
                    DiapoBack();
                }
            }
        }
    }

    /// <summary>
    /// Changer pour la diapositive suivante
    /// </summary>
    public void DiapoNext()
    {
        id_diapo_active = (id_diapo_active + 1) % diapo.Count;
        
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SyncDiapo", RpcTarget.All, id_diapo_active);
    }

    /// <summary>
    /// Changer pour la diapositive précédente
    /// </summary>
    public void DiapoBack()
    {
        id_diapo_active = (id_diapo_active - 1) >= 0 ? id_diapo_active - 1 : diapo.Count - 1;
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SyncDiapo", RpcTarget.All, id_diapo_active);
    }

    [PunRPC]
    private void SyncDiapo(int id_diapo_active)
    {
        isNeedChangeTexture = true;
        this.id_diapo_active = id_diapo_active;
    }

    /// <summary>
    /// Quand un nouveau joueur arrive, il demande au Master de sync l'id_diapo_active via cette fonction
    /// </summary>
    [PunRPC]
    private void RequireSyncDiapo()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SyncDiapo", RpcTarget.All, id_diapo_active);
        }
        else
        {
            Debug.LogError("Un RCP RequireSyncDiapo a été envoyé à quelqu'un d'autre que le MasterClient");
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
            float longueurDiapo = (float)0.8;
            float avancementchargement = longueurDiapo / nbDiapo;

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

                            //Avancement de la barre de téléchargement
                            xBarreChargement += avancementchargement; 
                            barreChargement.anchorMax = new Vector2(xBarreChargement, (float)0.2);
                            //modifier le witdh

                            // lag artificiel lor du téléchargement des images pour voir l'affichage
                            yield return new WaitForSeconds(0.5f);

                        }
                        else
                        {
                            textLogSurDiapo.text = "";
                            isAllDownload = true;
                            rendererObj.enabled = true;
                            isNeedChangeTexture = true;
                            material.SetTexture("_MainTex", diapo[0]);
                            barreChargement.gameObject.SetActive(false);
                            BarreChargementFull.gameObject.SetActive(false);
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

        //if (isAutoChangeSlide && isAllDownload)
        //{
        //    timer += Time.fixedDeltaTime;
        //    if (timer < speedAutoChangeSlide) return;
        //    id_diapo_active = (id_diapo_active + 1) % diapo.Count; //index diapo suivante
        //    material.SetTexture("_maintex", diapo[id_diapo_active]); //charge la diapo suivante
        //    timer -= speedAutoChangeSlide;
        //    logObj.AjoutLog("id_diapo_active : " + id_diapo_active, speedAutoChangeSlide);// affichage des logs dans le canvas
        //}
    }
}
