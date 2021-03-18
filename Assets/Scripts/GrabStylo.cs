using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GrabStylo : MonoBehaviour
{
    private GameObject styloGrab = null;
    private Vector3 deltaPos, deltaRot2;
    private Quaternion deltaRot, rotation;
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
            //styloGrab.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + deltaRot2);
            //styloGrab.transform.eulerAngles = currentEulerAngles;
            styloGrab.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + deltaRot2.x, this.transform.rotation.eulerAngles.y + deltaRot2.y, this.transform.rotation.eulerAngles.z + deltaRot2.z) * Quaternion.Euler(1, -90, 1);
            //modifie la rotation du stylo

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
                    if (lastGrab + 0.05f < Time.realtimeSinceStartup)
                    {
                        styloGrab = other.gameObject;
                        styloGrab.GetComponent<PhotonView>().RequestOwnership();
                        deltaPos = new Vector3(styloGrab.transform.position.x - this.transform.position.x,
                                                styloGrab.transform.position.y - this.transform.position.y,
                                                styloGrab.transform.position.z - this.transform.position.z);
                        /*deltaRot = new Vector3(this.transform.rotation.eulerAngles.x - styloGrab.transform.rotation.eulerAngles.x,
                                                this.transform.rotation.eulerAngles.y - styloGrab.transform.rotation.eulerAngles.y,
                                                this.transform.rotation.eulerAngles.z - styloGrab.transform.rotation.eulerAngles.z);*/
                        //deltaRot = this.transform.rotation * Quaternion.Inverse(Quaternion.Inverse(styloGrab.transform.rotation));
                        //deltaRot = this.transform.rotation; 
                        //deltaRot = Quaternion.LookRotation(deltaPos);
                        deltaRot = Quaternion.identity;
                        rotation = new Quaternion(deltaRot.x, deltaRot.y, deltaRot.z, deltaRot.w);

                        deltaRot2.x = deltaRot.z;
                        deltaRot2.y = deltaRot.y;
                        deltaRot2.z = deltaRot.x;

                        //deltaRot2 = new Vector3(deltaRot.eulerAngles.x, deltaRot.eulerAngles.y, deltaRot.eulerAngles.x);
                        styloGrab.GetComponent<PhotonView>().RPC("Grab", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId);
                    }
                }
            }
        }
    }

}