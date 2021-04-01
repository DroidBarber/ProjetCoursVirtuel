using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TableauController : MonoBehaviourPunCallbacks
{
    private Texture2D texture;
    public GameObject coinHautGauche, coinHautDroite, coinBasGauche, coinBasDroite;
    private int tailleEcriture = 6;

    // Start is called before the first frame update
    void Awake()
    {
        texture = new Texture2D(2000, 1200);
        this.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject log = GameObject.Find("Log_UI");
        log.GetComponent<Log_UI>().ForceClear();
        log.GetComponent<Log_UI>().AjoutLog(PhotonNetwork.InRoom.ToString());
    }

    public void Write(Vector3 position, Color c)
    {
        c.a = 1;// dans le doute
        //Debug.Log("Position:" + position);
        /*GameObject.Find("Log_UI").GetComponent<Log_UI>().ForceClear();
        GameObject.Find("Log_UI").GetComponent<Log_UI>().AjoutLog(position.ToString() + " color" + c.ToString());*/

        // z et y
        Vector2 pos = new Vector2();
        pos.x = ((coinHautGauche.transform.position.z - coinHautDroite.transform.position.z) - (position.z - coinHautDroite.transform.position.z)) / (coinHautGauche.transform.position.z - coinHautDroite.transform.position.z);
        pos.y = ((position.y - coinBasGauche.transform.position.y) - (coinHautGauche.transform.position.y - coinBasGauche.transform.position.y)) / (coinHautGauche.transform.position.y - coinBasGauche.transform.position.y);
        pos.x *= texture.width;
        pos.y *= texture.height;

        this.GetComponent<PhotonView>().RPC("UpdateWrite", RpcTarget.Others, (int)pos.x, (int)pos.y, new Vector3(c.r, c.g, c.b));

        //Debug.Log((int)pos.x + "   " + (int)pos.y);
        texture.SetPixel((int)pos.x, (int)pos.y, c);

        // pour faire de l'épaisseur
        for (int x = -tailleEcriture; x < tailleEcriture; x++)
        {
            for (int y = -tailleEcriture; y < tailleEcriture; y++)
            {
                texture.SetPixel((int)pos.x+x, (int)pos.y+y, c);
            }
        }
        texture.Apply();
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            this.GetComponent<PhotonView>().RPC("RequestSyncTableau", RpcTarget.MasterClient, PhotonNetwork.NetworkingClient.UserId);
            Debug.LogError("RequestSyncTableau demandés");

        }
    }
    [PunRPC]
    public void RequestSyncTableau(string id_player)
    {
        Debug.LogWarning("RequestSyncTableau effectué");
        Debug.LogError("id_player==nul =" + id_player == null);
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerListOthers)
        {
            Debug.LogWarning("player:" + player.UserId);
            if (player.UserId.Equals(id_player))
            {
                //Texture2D t = new Texture2D(2000, 1200);
                /*Graphics.CopyTexture(texture, t);
                t.Compress(true);*/
                byte[] byteTexture =  texture.EncodeToJPG();
                this.GetComponent<PhotonView>().RPC("UpdateAll", player, byteTexture);

                return;
            }
        }
    }

    [PunRPC]
    public void UpdateAll(byte[] t)
    {
        Debug.LogError("Update all effectué avec taille=" + t.Length);
        Destroy(texture);
        texture = null;
        texture = new Texture2D(2000, 1200);
        texture.LoadImage(t);
        this.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
    }

    [PunRPC]
    public void UpdateWrite(int posX, int posY, Vector3 color)
    {
        Color c = new Color(color.x, color.y, color.z, 1);
        texture.SetPixel(posX, posY, c);
        for (int x = -tailleEcriture; x < tailleEcriture; x++)
        {
            for (int y = -tailleEcriture; y < tailleEcriture; y++)
            {
                texture.SetPixel((int)posX + x, (int)posY + y, c);
            }
        }
        texture.Apply();
    }
}

