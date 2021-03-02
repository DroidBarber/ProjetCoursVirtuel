using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GrabStylo : MonoBehaviour
{
    private GameObject styloGrab = null;
    private Vector3 deltaPos, deltaRot;
    private float lastGrab;
    // Start is called before the first frame update
    void Start()
    {
        lastGrab = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (styloGrab)
        {
            styloGrab.transform.position = this.transform.position + deltaPos;
            styloGrab.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + deltaRot);
            if (OVRInput.GetUp(OVRInput.RawButton.A) || Input.GetKeyUp(KeyCode.A))
            {
                styloGrab.GetComponent<PhotonView>().RPC("GrabEnd", RpcTarget.All);
                styloGrab.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.CurrentRoom.MasterClientId);
                styloGrab = null;
                lastGrab = Time.realtimeSinceStartup;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!styloGrab)
        {
            if (other.tag.Equals("Stylo"))
            {
                if (OVRInput.GetUp(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.A))
                {
                    if (lastGrab + 0.05f <  Time.realtimeSinceStartup)
                    {
                        styloGrab = other.gameObject;
                        styloGrab.GetComponent<PhotonView>().RequestOwnership();
                        deltaPos = new Vector3(styloGrab.transform.position.x - this.transform.position.x,
                                                styloGrab.transform.position.y - this.transform.position.y,
                                                styloGrab.transform.position.z - this.transform.position.z);
                        deltaRot = new Vector3(this.transform.rotation.eulerAngles.x - styloGrab.transform.rotation.eulerAngles.x,
                                                this.transform.rotation.eulerAngles.y - styloGrab.transform.rotation.eulerAngles.y,
                                                this.transform.rotation.eulerAngles.z - styloGrab.transform.rotation.eulerAngles.z);
                        styloGrab.GetComponent<PhotonView>().RPC("Grab", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId);
                    }
                }
            }
        }
    }

}
