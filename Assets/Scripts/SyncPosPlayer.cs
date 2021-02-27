using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SyncPosPlayer : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        if (!this.gameObject.GetComponent<PhotonView>().IsMine)
        {
            Destroy(this);
        }
        else
        {
            this.GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position;
    }
}
