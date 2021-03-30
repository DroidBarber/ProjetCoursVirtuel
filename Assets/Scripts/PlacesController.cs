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
            Debug.Log("numRangee : "+ numRangee); 
            int numChaise = indexPlace - numRangee * nbChaisesDroite;
            Debug.Log("numChaise : "+ numChaise); 
            Vector3 place = new Vector3(offsetRightX * numRangee + rightA.position.x, rightA.position.y, offsetRightZ * numChaise + rightA.position.z);
            Debug.Log("Position index : " + indexPlace + " | Pos : " + place);
            return place;
        } 
        else
        {
            int numRangee = indexPlace/nbChaisesGauche;
            int numChaise = indexPlace - numRangee * nbChaisesGauche;
            Vector3 place = new Vector3(offsetLeftX * numRangee + leftA.position.x, leftA.position.y, offsetLeftZ * numChaise + leftA.position.z);
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
            playerTransform.position = placePosition;
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
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        libererPlace(otherPlayer.UserId);
    }
}
