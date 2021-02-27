using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Network : MonoBehaviour
{
    public GameObject cubePrefab;
    public Log_UI logObj;
    public GameObject playerPrefab;
    public GameObject playerOVR;
    // Start is called before the first frame update
    void Awake()
    {
        if (!logObj)
            Debug.LogError("logObj non assigné");
        if (!cubePrefab)
            Debug.LogError("cubePrefab non assigné");
        if (!playerPrefab)
            Debug.LogError("playerPrefab non assigné");
        if (!playerOVR)
            Debug.LogError("playerOVR non assigné");
    }

    // Update is called once per frame
    void Update()
    {
        logObj.AjoutLog(PhotonNetwork.NetworkingClient.State.ToString(),1);
        if (PhotonNetwork.NetworkingClient.InRoom)
        {
            PhotonNetwork.Instantiate(cubePrefab.name, cubePrefab.transform.position, Quaternion.identity);
            logObj.AjoutLog(PhotonNetwork.NetworkingClient.CurrentRoom.Name, 1000);
            GameObject p =  PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, Quaternion.identity);
            p.GetComponent<SyncPosPlayer>().player = playerOVR;
            Destroy(this);
        }
    }
}
