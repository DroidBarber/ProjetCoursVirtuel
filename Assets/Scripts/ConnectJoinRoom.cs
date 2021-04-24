using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

/// <summary>
/// Ce script sert à la création ainsi qu'à rejoindre une room dont le nom de la room proviendra du script RoomNameToJoin
/// Ce script n'est utile que lorsque le "joueur" est celui qui crée la room, sinon il n'aura pas d'effet particulier, 
/// hormis 2 debug.
/// </summary>
public class ConnectJoinRoom : MonoBehaviourPunCallbacks
{
    [Tooltip("Si on affiche les Debug.Log() ou non")]
    public bool isShowDebugLogInUnity = true; // Si on affiche les Debug.Log()

    /// <summary>
    /// Used as PhotonNetwork.GameVersion. Il faut le changer à chaque version qui change des 
    /// fonctionnalité multijoueur afin que deux "joueurs" n'ayant pas les même version de ce logiciel ne puisse
    /// se retrouver ensemble et ainsi générer des bug car ils n'ont pas les même utilisations du réseau PhotonEngine
    /// </summary>
    public byte Version = 1;

    /// <summary>Max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.</summary>
    [Tooltip("The max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.")]
    public byte MaxPlayers = 20;

    public int playerTTL = -1; // Time To Live des objets d'un joueur déconnecté

    void Awake()
    {
        // On recherche la room à laquel on doit se connecter si nous ne somme pas dans une room, c'est à dire que
        // nous allons être MasterClient et que c'est nous qui allons créer la room
        if (!PhotonNetwork.InRoom)
        {
            GameObject g = GameObject.Find("RoomNameToJoin");
            string roomName = g.GetComponent<RoomNameToJoin>().roomName;
            CreateRoom(roomName);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (isShowDebugLogInUnity)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }
        //Mettre ici un changement de scène pour la scène de choix de room
    }

    public override void OnJoinedRoom()
    {
        if (isShowDebugLogInUnity)
        {
            Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room in region [" + PhotonNetwork.CloudRegion + "]. Game is now running.");
        }
    }

    /// <summary>
    /// Fonction appelé uniquement lorsque le "joueur" crée une salle et deviendra donc le masterClient, 
    /// car il est le premier à rejoindre cette room, vu qu'elle n'existait pas avant 
    /// </summary>
    public void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = this.MaxPlayers };
        if (playerTTL >= 0)
            roomOptions.PlayerTtl = playerTTL;
        roomOptions.PublishUserId = true;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
    }
}
