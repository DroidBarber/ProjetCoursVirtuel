using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TableauController : MonoBehaviourPunCallbacks
{
    private Texture2D texture;
    public GameObject coinHautGauche, coinHautDroite, coinBasGauche, coinBasDroite;

    // Start is called before the first frame update
    void Awake()
    {
        texture = new Texture2D(300, 300);
        this.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Write(Vector3 position, Color c)
    {
        c.a = 1;// dans le doute
        Debug.Log("Position:" + position);
        GameObject.Find("Log_UI").GetComponent<Log_UI>().ForceClear();
        GameObject.Find("Log_UI").GetComponent<Log_UI>().AjoutLog(position.ToString() + " color" + c.ToString());

        // z et y
        Vector2 pos = new Vector2();
        pos.x = ((coinHautGauche.transform.position.z - coinHautDroite.transform.position.z) - (position.z - coinHautDroite.transform.position.z)) / (coinHautGauche.transform.position.z - coinHautDroite.transform.position.z);
        pos.y = ((position.y - coinBasGauche.transform.position.y) - (coinHautGauche.transform.position.y - coinBasGauche.transform.position.y)) / (coinHautGauche.transform.position.y - coinBasGauche.transform.position.y);
        pos.x *= texture.width;
        pos.y *= texture.height;

        this.GetComponent<PhotonView>().RPC("UpdateWrite", RpcTarget.Others, (int)pos.x, (int)pos.y, new Vector3(c.r, c.g, c.b));

        Debug.Log((int)pos.x + "   " + (int)pos.y);
        texture.SetPixel((int)pos.x, (int)pos.y, c);

        // pour faire de l'épaisseur
        /*for (int x = -40; x < 40; x++)
        {
            for (int y = -40; y < 40; y++)
            {
                texture.SetPixel((int)pos.x+x, (int)pos.y+y, c);
            }
        }*/
        texture.Apply();
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            this.GetComponent<PhotonView>().RPC("RequestSyncTableau", RpcTarget.MasterClient, PhotonNetwork.NetworkingClient.UserId);
        }
    }
    [PunRPC]
    public void RequestSyncTableau(string id_player)
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player.UserId.Equals(id_player))
            {
                this.GetComponent<PhotonView>().RPC("UpdateAll", player, texture);
                return;
            }
        }
    }

    [PunRPC]
    public void UpdateAll(Texture2D t)
    {
        texture = null;
        texture = t;
    }

    [PunRPC]
    public void UpdateWrite(int posX, int posY, Vector3 color)
    {
        Color c = new Color(color.x, color.y, color.z, 1);
        texture.SetPixel(posX, posY, c);
    }

}
