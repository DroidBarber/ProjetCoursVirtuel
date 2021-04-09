using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlacesController : MonoBehaviourPunCallbacks
{
    public Transform playerTransform;
    public Transform leftA; //rangées gauche, gameobject avant gauche
    public Transform leftB; //rangées gauche, gameobject arriere droit
    public Transform rightA; //rangées droite, gameobject avant gauche
    public Transform rightB; //rangées droite, gameobject arriere droit

    public int nbRangeesGauche = 9;
    public int nbChaisesGauche = 7;
    public int nbRangeesDroite = 8;
    public int nbChaisesDroite = 6;
    private float offsetLeftZ;
    private float offsetLeftX;
    private float offsetRightZ;
    private float offsetRightX;
    private List<String> isAvailable;
    public Log_UI log_ui;
    public Vector3 offsetDuplicateDiapo;
    

    void Start()
    {
        offsetLeftZ = (leftB.position.z - leftA.position.z)/(nbChaisesGauche-1); //distance entre deux chaises lateralement
        offsetLeftX = (leftB.position.x - leftA.position.x)/(nbRangeesGauche-1); //distance entre deux rangees
        offsetRightZ = (rightB.position.z - rightA.position.z)/(nbChaisesDroite-1); //distance entre deux chaises lateralement
        offsetRightX = (rightB.position.x - rightA.position.x)/(nbRangeesDroite-1); //distance entre deux rangees
        isAvailable = new List<String>();
        for (int i = 0; i < nbRangeesGauche * nbChaisesGauche + nbRangeesDroite * nbChaisesDroite; i++)
        {
            isAvailable.Add("");
        }
        log_ui.ForceClear();
        var leftAPosition = leftA.position;
        var planePosition = playerTransform.gameObject.GetComponent<TeleportPlace>().duplicationDiapo.transform.position;
        offsetDuplicateDiapo = new Vector3(planePosition.x - leftAPosition.x, planePosition.y - leftAPosition.y,
            planePosition.z - leftAPosition.z);
    }
    
    public Vector3 getPlaceTransform(int indexPlace) //index commence à 0
    {
        if(indexPlace >= nbRangeesGauche * nbChaisesGauche + nbRangeesDroite * nbChaisesDroite)
        {
            Debug.LogError("IndexPlace dépasse le nombre total de places !");
            return new Vector3(0, 0, 0);
        }
        else if (indexPlace >= nbRangeesGauche * nbChaisesGauche)
        {
            indexPlace = indexPlace - nbRangeesGauche * nbChaisesGauche;
            int numRangee = indexPlace/nbChaisesDroite;
            int numChaise = indexPlace - numRangee * nbChaisesDroite;
            Vector3 place = new Vector3(offsetRightX * numRangee + rightA.position.x, rightA.position.y, offsetRightZ * numChaise + rightA.position.z);
            log_ui.AjoutLog("Vecteur TP theorique : " + place.x +" "+place.y+" "+place.z);
            return place;
        } 
        else
        {
            int numRangee = indexPlace/nbChaisesGauche;
            int numChaise = indexPlace - numRangee * nbChaisesGauche;
            Vector3 place = new Vector3(offsetLeftX * numRangee + leftA.position.x, leftA.position.y, offsetLeftZ * numChaise + leftA.position.z);
            log_ui.AjoutLog("Vecteur TP theorique : " + place.x +" "+place.y+" "+place.z);
            return place;
        }
    }

    [PunRPC]
    public void reserverPlace(int indexPlace, String playerID)
    {
        if (PhotonNetwork.IsMasterClient && isAvailable[indexPlace].Equals(""))
        {
            PhotonView.Get(this).RPC("occuperPlace", RpcTarget.All, indexPlace, playerID);
        }
    }

    [PunRPC]
    public void occuperPlace(int indexPlace, String playerID)
    {
        libererPlace(playerID);
        isAvailable[indexPlace] = playerID;
        if (playerID.Equals(PhotonNetwork.NetworkingClient.UserId))
        {
            Vector3 placePosition = getPlaceTransform(indexPlace);
            playerTransform.gameObject.SetActive(false);
            playerTransform.gameObject.GetComponent<OVRPlayerController>().GravityModifier = 0;
            playerTransform.position = placePosition;
            playerTransform.gameObject.SetActive(true);
        }
    }
    [PunRPC]
    public void libererPlaceRPC(String playerID)
    {
        libererPlace(playerID);
    }
    public void libererPlace(String playerID)
    {
        for (int i = 0; i < isAvailable.Count; i++)
        {
            if (isAvailable[i] == playerID)
            {
                isAvailable[i] = "";
            }
        }
        playerTransform.gameObject.SetActive(false);
        playerTransform.position = new Vector3(0, 1, 0);
        playerTransform.gameObject.GetComponent<OVRPlayerController>().GravityModifier = 1;
        playerTransform.gameObject.SetActive(true);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        libererPlace(otherPlayer.UserId);
    }
}
