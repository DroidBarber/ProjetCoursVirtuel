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


/// <summary>
/// Ce script permet de gérer le diaporama (téléchargement, et gestion pour passer les diapositives)
/// </summary>

[RequireComponent(typeof(Renderer))] // Oblige que l'objet qui possède ce script à posséder un Renderer, et s'il n'en a pas, en crée un
[RequireComponent(typeof(PhotonView))]
public class SlideController : MonoBehaviourPunCallbacks
{

    public List<Texture2D> diapo = new List<Texture2D>(); // la liste des images du diaporama
    public bool isAutoChangeSlide = false;
    public bool isOnlyMasterClientCanChangeDiapo = false; // est-ce que que seul le masterClient peux changer le diaporama?
    public float speedAutoChangeSlide = 5.0f;
    private Material material;
    private Renderer rendererObj;
    private int id_diapo_active = 0;
    //private float timer = 0.0f; //initialise le timer à zéro
    private bool isAllDownload = false; // est ce que toutes les diapositives sont téléchargé?
    private bool isNeedChangeTexture = false; // est-ce qu'il faut changer de diapositive affiché?
    public Log_UI logObj;
    public TextMeshProUGUI textLogSurDiapo;
    public RectTransform BarreChargementFull;
    public RectTransform barreChargement;
    private float xBarreChargement;
    private Texture2D textureShow; // la texture de l'image fu diaporama affiché en ce moment

    private void Awake()
    {
        textureShow = new Texture2D(1, 1);
        rendererObj = this.GetComponent<Renderer>(); // Récuperation du renderer
        rendererObj.enabled = true; // Par défaut pas d'affichage d'image
        material = rendererObj.sharedMaterial; // Récuperation du material
        material.SetTexture("_MainTex", textureShow); 

        StartCoroutine(GetDiapo()); // lance un "thread" en parallèle chargé de télécharger les diapositives

        if (!logObj)
            Debug.LogError("logObj non assigné");
        if (!textLogSurDiapo)
            Debug.LogError("logObj non assigné");
        if(!BarreChargementFull)
            Debug.LogError("logObj non assigné");
        if (!barreChargement)
            Debug.LogError("logObj non assigné");

        barreChargement.anchorMax = new Vector2(0.1f, 0.2f);

        xBarreChargement = 0.1f;
    }

    /// <summary>
    /// Losque l'on rejoint une room, si l'on n'est pas le MasterClient, alors on demande au MasterClient quel diapositive est affiché
    /// </summary>
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


    void Update()
    {
        if (isAllDownload) // si tout est téléchargé
        {
            if (isNeedChangeTexture)
            {
                textureShow.Resize(diapo[id_diapo_active].width, diapo[id_diapo_active].height);
                textureShow.LoadImage(diapo[id_diapo_active].EncodeToJPG());
                //Graphics.CopyTexture(diapo[id_diapo_active], textureShow);
                //material.SetTexture("_MainTex", diapo[id_diapo_active]);
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

    /// <summary>
    /// Fonction appelé en réseau afin de tenirà jour chez tout le monde la diapositive affiché
    /// </summary>
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
    
    /// <summary>
    /// Fonction qui télécharge les diapositives à partir d'une URL
    /// </summary>
    private IEnumerator GetDiapo()
    {
        /// https://pastebin.com/raw/8e3DsJ2V est l'URL qui ne mène qu'à un fichier type .txt qui a une URL par ligne
        /* cet URL mène donc juste à un fichier contenant une URL par ligne:
         *  https://media.gettyimages.com/vectors/presentation-title-slide-design-template-with-retro-midcentury-vector-id1220485840?s=2048x2048
            https://media.gettyimages.com/vectors/abstract-retro-midcentury-geometric-graphics-vector-id1211608021?s=2048x2048
            https://media.gettyimages.com/vectors/presentation-title-slide-design-template-with-retro-midcentury-vector-id1264569265?s=2048x2048
            https://media.gettyimages.com/vectors/neon-color-geometric-round-rectangle-on-metal-stripe-pattern-portal-vector-id1207741638?s=2048x2048
         */
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
            float longueurDiapo = 0.8f;
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
                            //material.SetTexture("_MainTex", diapo[0]);
                            textureShow.Resize(diapo[0].width, diapo[0].height);
                            textureShow.LoadImage(diapo[0].EncodeToJPG());

                            barreChargement.gameObject.SetActive(false);
                            BarreChargementFull.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
