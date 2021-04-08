using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class StyloController : MonoBehaviourPunCallbacks
{
    private string id_player_owner="";

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

    [PunRPC]
    public void Grab(string id_player)
    {
        id_player_owner = id_player;
        this.gameObject.GetComponent<Collider>().isTrigger = true;
        Destroy(this.gameObject.GetComponent<Rigidbody>());

        Log_UI log_ui = GameObject.Find("Log_UI").GetComponent<Log_UI>();
        log_ui.AjoutLog("grab by " + id_player, 15);
    }

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
            this.GetComponent<PhotonView>().RPC("RequireSync", RpcTarget.MasterClient, PhotonNetwork.NetworkingClient.UserId);
        }
        this.gameObject.GetComponent<Renderer>().enabled = true;
    }

    [PunRPC]
    public void RequireSync(string id_player)
    {
        if(!this.isGrab())
        {
            Debug.LogError("RequireSync grabend pour " + id_player);
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (player.UserId.Equals(id_player))
                {
                    this.GetComponent<PhotonView>().RPC("GrabEnd", player);
                    Debug.LogError("RequireSync grabend send pour " + id_player);
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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // problème ici
        Debug.LogWarning("OnPlayerLeftRoom, is master=" + PhotonNetwork.IsMasterClient + " id same=" + otherPlayer.UserId.Equals(id_player_owner));
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
