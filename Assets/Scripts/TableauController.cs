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
    private List<Vector2> lstPoint = new List<Vector2>();

    // Start is called before the first frame update
    void Awake()
    {
        texture = new Texture2D(2000, 1200);
        this.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
    }

    // Update is called once per frame
    void Update()
    {

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
        lstPoint.Add(new Vector2((int)pos.x, (int)pos.y));

        // pour faire de l'épaisseur
        for (int x = -tailleEcriture; x < tailleEcriture; x++)
        {
            for (int y = -tailleEcriture; y < tailleEcriture; y++)
            {
                texture.SetPixel((int)pos.x + x, (int)pos.y + y, c);
            }
        }
        texture.Apply();
    }

    public void WriteEnd()
    {
        Debug.LogError("WriteEnd " + lstPoint.Count);
        if (lstPoint.Count >= 4)
        {
            for (int i = 0; i < lstPoint.Count - 3; i++)
            {
                for (float u = 0; u <= 1; u += 0.01f)
                {
                    Vector2 newPos;
                    newPos.x = lstPoint[i].x * Mathf.Pow(1 - u, 3) + 3 * lstPoint[i + 1].x * u * Mathf.Pow(1 - u, 2) + 3 * lstPoint[i + 2].x * u * u * (1 - u) + lstPoint[i + 3].x * u * u * u;
                    newPos.y = lstPoint[i].y * Mathf.Pow(1 - u, 3) + 3 * lstPoint[i + 1].y * u * Mathf.Pow(1 - u, 2) + 3 * lstPoint[i + 2].y * u * u * (1 - u) + lstPoint[i + 3].y * u * u * u;

                    //newPos.x = lstPoint[i].x * Mathf.Pow(1 - u, 2) + 2 * lstPoint[i + 1].x * u * (1 - u) + lstPoint[i + 2].x * Mathf.Pow(u, 2);
                    //newPos.y = lstPoint[i].y * Mathf.Pow(1 - u, 2) + 2 * lstPoint[i + 1].y * u * (1 - u) + lstPoint[i + 2].y * Mathf.Pow(u, 2);

                    for (int x = -tailleEcriture; x < tailleEcriture; x++)
                    {
                        for (int y = -tailleEcriture; y < tailleEcriture; y++)
                        {
                            //if (Mathf.Sqrt(Mathf.Pow(newPos.x - x, 2) + Mathf.Pow(newPos.y - y, 2)) < tailleEcriture)
                            if(Vector2.Distance(newPos, new Vector2(newPos.x + x, newPos.y + y)) < tailleEcriture)
                            {
                                texture.SetPixel((int)newPos.x + x, (int)newPos.y + y, Color.black);
                            }
                            //else
                                //Debug.LogError("Valeur de la racine"+Mathf.Sqrt(Mathf.Pow(newPos.x - x, 2) + Mathf.Pow(newPos.y - y, 2)));
                        }
                    }
                }


            }
            texture.Apply();
            lstPoint.Clear();
        }
        else
        {
            lstPoint.Clear();
        }
        /*if (lstPoint.Count >= 2)
        {
            for (int i = 0; i < lstPoint.Count-1; i++)
            {
                Vector2 newPos = lstPoint[i];
                for (int u = 0; u < Vector2.Distance(lstPoint[i], lstPoint[i+1]); u++)
                {
                    newPos = lstPoint[i];
                    for (int x = -tailleEcriture; x < tailleEcriture; x++)
                    {
                        for (int y = -tailleEcriture; y < tailleEcriture; y++)
                        {
                            texture.SetPixel((int)(newPos.x + x + u/ Vector2.Distance(lstPoint[i], lstPoint[i + 1]) * (lstPoint[i + 1].x - lstPoint[i].x)), (int)(newPos.y + y + u/ Vector2.Distance(lstPoint[i], lstPoint[i + 1]) * (lstPoint[i + 1].y - lstPoint[i].y)), Color.black);
                        }
                    }
                }
            }
            texture.Apply();
            lstPoint.Clear();
        }
        else
        {
            lstPoint.Clear();
        }*/
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
        //Debug.LogWarning("RequestSyncTableau effectué");
        //Debug.LogError("id_player==nul =" + id_player == null);
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerListOthers)
        {
            //Debug.LogWarning("player:" + player.UserId);
            if (player.UserId.Equals(id_player))
            {
                //Texture2D t = new Texture2D(2000, 1200);
                /*Graphics.CopyTexture(texture, t);
                t.Compress(true);*/
                byte[] byteTexture = texture.EncodeToJPG();
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

public class BezierExample : MonoBehaviour
{
    public Vector3 startPoint = new Vector3(-0.0f, 0.0f, 0.0f);
    public Vector3 endPoint = new Vector3(-2.0f, 2.0f, 0.0f);
    public Vector3 startTangent = Vector3.zero;
    public Vector3 endTangent = Vector3.zero;
}
