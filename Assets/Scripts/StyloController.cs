using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


/// <summary>
/// Ce script permet de gérer un stylo (est-ce qu'il est pris ou non notamment)
/// </summary>
public class StyloController : MonoBehaviourPunCallbacks
{
    private string id_player_owner="";

    /// <summary>
    /// Tant qu'on est pas connecté à une room, on désactive son renderer, pour faire comme s'il n'est pas là
    /// on peux pas désactivé le gameobject, sinon on ne pourra plus le réactivé que avec un autre script sur un autre gameobject, ce qui n'est pas pratique
    /// </summary>
    private void Awake()
    {
        this.gameObject.GetComponent<Renderer>().enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Informe que ce stylo a été pris/grab
    /// </summary>
    /// <param name="id_player">L'identifiant du joueur qui a pris le stylo</param>
    [PunRPC]
    public void Grab(string id_player)
    {
        id_player_owner = id_player;
        this.gameObject.GetComponent<Collider>().isTrigger = true;
        Destroy(this.gameObject.GetComponent<Rigidbody>()); // sinon le stylo pris tomberais des "mains" à cause de la gravité

        Log_UI log_ui = GameObject.Find("Log_UI").GetComponent<Log_UI>();
        log_ui.AjoutLog("grab by " + id_player, 15);
    }
    /// <summary>
    /// Informe que ce stylo a été laché, il reprend donc la gravité afin de tomber sur une table par exemple
    /// </summary>
    [PunRPC]
    public void GrabEnd()
    {
        id_player_owner = "";
        this.gameObject.GetComponent<Collider>().isTrigger = false;
        if (!this.gameObject.GetComponent<Rigidbody>())
        {
            this.gameObject.AddComponent<Rigidbody>();
        }

        Log_UI log_ui = GameObject.Find("Log_UI").GetComponent<Log_UI>();
        log_ui.AjoutLog("grabend", 15);

    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            // demande afin de savoir si le stylo est pris ou non
            this.GetComponent<PhotonView>().RPC("RequireSync", RpcTarget.MasterClient, PhotonNetwork.NetworkingClient.UserId);
        }
        this.gameObject.GetComponent<Renderer>().enabled = true;
    }

    /// <summary>
    /// Renvoie au joueur qui l'a demandé l'information sur l'état (grab ou pas) du stylo
    /// </summary>
    /// <param name="id_player"></param>
    [PunRPC]
    public void RequireSync(string id_player)
    {
        if(!this.isGrab())
        {
            //Debug.LogError("RequireSync grabend pour " + id_player);
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (player.UserId.Equals(id_player))
                {
                    this.GetComponent<PhotonView>().RPC("GrabEnd", player);
                    //Debug.LogError("RequireSync grabend send pour " + id_player);
                    return;
                }
            }
        }
        if (this.isGrab())
        {
            Debug.LogError("RequireSync grab pour " + id_player);
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (player.UserId.Equals(id_player))
                {
                    this.GetComponent<PhotonView>().RPC("Grab", player, id_player_owner);
                    Debug.LogError("RequireSync grab send pour " + id_player);
                    return;
                }
            }
        }
        
    }

    /// <summary>
    /// Si un joueur quitte la room, il faut que les stylo qu'il a grab soit laché
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Debug.LogWarning("OnPlayerLeftRoom, is master=" + PhotonNetwork.IsMasterClient + " id same=" + otherPlayer.UserId.Equals(id_player_owner));
        if (otherPlayer.UserId.Equals(id_player_owner))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                this.GetComponent<PhotonView>().RPC("GrabEnd", RpcTarget.All);
                Debug.LogError("GrabEnd en RCP all");
            }
        }
    }
     

    public bool isGrab()
    {
        return id_player_owner != "";
    }

    public string get_id_player_owner()
    {
        return id_player_owner;
    }
}
