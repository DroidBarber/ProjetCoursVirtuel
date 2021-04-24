using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Permet de synchroniser la couleur du 1er avatar qui n'est rien d'autre qu'une capsule
/// </summary>
public class SyncColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (this.gameObject.GetComponent<PhotonView>().IsMine)
        {
            PhotonView photonView = PhotonView.Get(this);
            Color color = Random.ColorHSV();

            photonView.RPC("SyncColorFct", RpcTarget.AllBuffered, color.r, color.g, color.b);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void SyncColorFct(float r, float g, float b)
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material = new Material(Shader.Find("Standard"));
        rend.material.color = new Color(r, g, b);
    }
}
