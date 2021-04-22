using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TableauController : MonoBehaviourPunCallbacks
{
    private Texture2D texture;
    public GameObject coinHautGauche, coinHautDroite, coinBasGauche, coinBasDroite;
    private int tailleEcriture = 6, pointCalculx, pointCalculy;
    private List<Vector2> lstPoint = new List<Vector2>();
    private bool needApplyTexture = false;

    // Start is called before the first frame update
    void Awake()
    {
        texture = new Texture2D(2000, 1200);
        this.gameObject.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", texture);
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (OVRInput.GetUp(OVRInput.RawButton.B) || Input.GetKeyUp(KeyCode.B))
            {
                this.GetComponent<PhotonView>().RPC("ClearTableau", RpcTarget.All);

            }
        }
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
        //texture.SetPixel((int)pos.x, (int)pos.y, c);
        pos.x = (int)pos.x;
        pos.y = (int)pos.y;
        if (lstPoint.Count >= 1 && Vector2.Distance(lstPoint[lstPoint.Count - 1], pos) > tailleEcriture)
            lstPoint.Add(new Vector2((int)pos.x, (int)pos.y));
        if (lstPoint.Count == 0)
            lstPoint.Add(new Vector2((int)pos.x, (int)pos.y));

        // pour faire de l'épaisseur
        /*for (int x = -tailleEcriture; x < tailleEcriture; x++)
        {
            for (int y = -tailleEcriture; y < tailleEcriture; y++)
            {
                texture.SetPixel((int)pos.x + x, (int)pos.y + y, c);
            }
        }*/
        if (lstPoint.Count >= 4)
        //Debug.Log(lstPoint.Count);
        {
            for (int i = 0; i < lstPoint.Count - 3; i += 3) //changer valeurs boucle ?
            {
                float u;
                for (int t = 0; t <= 50; t ++)
                {
                    u = t / 50.0f;
                    Vector2 newPos;
                    //pointcalcul x et y, modif listPoint[i+1] à partir du deuxième patch, i > 0
                    if (i == 0)
                    {
                        pointCalculx = (int)lstPoint[i + 1].x;
                        pointCalculy = (int)lstPoint[i + 1].y;

                    }
                    else
                    {
                        //calcul des coefficients de colinéarité pour le raccordement de la courbe
                        pointCalculx = (int)(2 * lstPoint[i].x - lstPoint[i - 1].x);
                        pointCalculy = (int)(2 * lstPoint[i].y - lstPoint[i - 1].y);

                    }

                    newPos.x = lstPoint[i].x * Mathf.Pow(1 - u, 3) + 3 * pointCalculx * u * Mathf.Pow(1 - u, 2) + 3 * lstPoint[i + 2].x * u * u * (1 - u) + lstPoint[i + 3].x * u * u * u;
                    newPos.y = lstPoint[i].y * Mathf.Pow(1 - u, 3) + 3 * pointCalculy * u * Mathf.Pow(1 - u, 2) + 3 * lstPoint[i + 2].y * u * u * (1 - u) + lstPoint[i + 3].y * u * u * u;

                    for (int x = -tailleEcriture; x < tailleEcriture; x++)
                    {
                        for (int y = -tailleEcriture; y < tailleEcriture; y++)
                        {
                            if (Vector2.Distance(newPos, new Vector2(newPos.x + x, newPos.y + y)) < tailleEcriture)
                            {
                                if (texture.GetPixel((int)newPos.x + x, (int)newPos.y + y) != c)
                                    texture.SetPixel((int)newPos.x + x, (int)newPos.y + y, c);
                            }

                        }
                    }
                }


            }
            needApplyTexture = true;
            lstPoint.RemoveAt(0);
            lstPoint.RemoveAt(0);
            lstPoint.RemoveAt(0);


        }
    }
    public void FixedUpdate()
    {
        if (needApplyTexture)
        {
            texture.Apply();
            needApplyTexture = false;
        }
    }

    public void WriteEnd()
    {
        this.GetComponent<PhotonView>().RPC("WriteEndRPC", RpcTarget.Others);
        lstPoint.Clear();
    }

    [PunRPC]
    public void WriteEndRPC()
    {
        lstPoint.Clear();
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            this.GetComponent<PhotonView>().RPC("RequestSyncTableau", RpcTarget.MasterClient, PhotonNetwork.NetworkingClient.UserId);
            //Debug.LogError("RequestSyncTableau demandés");

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
        //Debug.LogError("Update all effectué avec taille=" + t.Length);
        /*Destroy(texture);
        texture = null;
        texture = new Texture2D(2000, 1200);*/
        texture.LoadImage(t);
        //this.gameObject.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", texture);
    }

    [PunRPC]
    public void UpdateWrite(int posX, int posY, Vector3 color)
    {
        Vector2 pos = new Vector2(posX, posY);
        Color c = new Color(color.x, color.y, color.z, 1);
        c.a = 1;// dans le doute
                //Debug.Log("Position:" + position);
        /*GameObject.Find("Log_UI").GetComponent<Log_UI>().ForceClear();
        GameObject.Find("Log_UI").GetComponent<Log_UI>().AjoutLog(position.ToString() + " color" + c.ToString());*/

        // z et y


        //Debug.Log((int)pos.x + "   " + (int)pos.y);
        //texture.SetPixel((int)pos.x, (int)pos.y, c);
        pos.x = (int)pos.x;
        pos.y = (int)pos.y;
        lstPoint.Add(new Vector2((int)pos.x, (int)pos.y));

        // pour faire de l'épaisseur
        /*for (int x = -tailleEcriture; x < tailleEcriture; x++)
        {
            for (int y = -tailleEcriture; y < tailleEcriture; y++)
            {
                texture.SetPixel((int)pos.x + x, (int)pos.y + y, c);
            }
        }*/
        if (lstPoint.Count >= 4)
        //Debug.Log(lstPoint.Count);
        {
            for (int i = 0; i < lstPoint.Count - 3; i += 3) //changer valeurs boucle ?
            {
                for (float u = 0; u <= 1; u += 0.008f)
                {
                    Vector2 newPos;
                    //pointcalcul x et y, modif listPoint[i+1] à partir du deuxième patch, i > 0
                    if (i == 0)
                    {
                        pointCalculx = (int)lstPoint[i + 1].x;
                        pointCalculy = (int)lstPoint[i + 1].y;

                    }
                    else
                    {
                        //calcul des coefficients de colinéarité pour le raccordement de la courbe
                        pointCalculx = (int)(2 * lstPoint[i].x - lstPoint[i - 1].x);
                        pointCalculy = (int)(2 * lstPoint[i].y - lstPoint[i - 1].y);

                    }

                    newPos.x = lstPoint[i].x * Mathf.Pow(1 - u, 3) + 3 * pointCalculx * u * Mathf.Pow(1 - u, 2) + 3 * lstPoint[i + 2].x * u * u * (1 - u) + lstPoint[i + 3].x * u * u * u;
                    newPos.y = lstPoint[i].y * Mathf.Pow(1 - u, 3) + 3 * pointCalculy * u * Mathf.Pow(1 - u, 2) + 3 * lstPoint[i + 2].y * u * u * (1 - u) + lstPoint[i + 3].y * u * u * u;

                    for (int x = -tailleEcriture; x < tailleEcriture; x++)
                    {
                        for (int y = -tailleEcriture; y < tailleEcriture; y++)
                        {
                            if (Vector2.Distance(newPos, new Vector2(newPos.x + x, newPos.y + y)) < tailleEcriture)
                            {
                                texture.SetPixel((int)newPos.x + x, (int)newPos.y + y, c);
                            }

                        }
                    }
                }


            }
            needApplyTexture = true;
            lstPoint.RemoveAt(0);
            lstPoint.RemoveAt(0);
            lstPoint.RemoveAt(0);
        }
    }

    [PunRPC]
    public void ClearTableau()
    {
        texture.Resize(1, 1);
        texture.SetPixel(1, 1, Color.white);
        texture.Apply();
        texture.Resize(2000, 1200);



    }
}