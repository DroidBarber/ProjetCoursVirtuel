using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Ce script est utilisé sur un gameobject qui suit le controller droit
/// Le gameObject qui possède ce script doit avoir un collider avec isTrigger d'activé
/// Ce script permet de faire les actions nécessaires afin de prendre et de lacher un stylo, ainsi que de faire 
/// suivre la position/rotation du stylo vis à vis de notre controller
/// Nous ne pouvons pas tous simplement faire en sorte que le stylo deviennent enfant de ce gameObject sans quoi cela
/// casserait la synchronisation réseau par Photon de la position/rotation du stylo, ainsi nous faisont ainsi
/// Néanmoins un système comme la synchronisation réseau des position/rotation des avatars serait plus efficace, mais n'a pas été réalisé
/// car nous n'y avons pas penser initialement.
/// </summary>
public class GrabStylo : MonoBehaviour
{
    private GameObject styloGrab = null; // Le stylo qui a été grab
    private Vector3 deltaPos, deltaRot2;
    private Quaternion deltaRot;
    /// <summary>
    /// deltaPos, deltaRot2 et deltaRot permette d'avoir la position et une rotation relative à la main/manette 
    /// lorsqu'un stylo viens d'être pris, afin de garder ce delta
    /// Néanmoins, pour la rotation, cela n'est pas simple, et il peux y avoir des bugs, notamment si à 
    /// l'origine le stylo n'est pas dans une certaine orientation
    /// </summary>
    private float lastGrab;
    // lastGrab est le temps auquel on a laché en dernier un stylo, cela est utile car ayant la même
    // touche pour prendre et lacher le stylo, sans cela on lachait et reprennait le stylo immédiatement

    // Start is called before the first frame update
    void Start()
    {
        lastGrab = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (styloGrab) // si on a pris un stylo
        {
            // on met à jour sa position relative au controller
            styloGrab.transform.position = this.transform.position + deltaPos;
            styloGrab.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + deltaRot2.x, this.transform.rotation.eulerAngles.y + deltaRot2.y, this.transform.rotation.eulerAngles.z + deltaRot2.z) * Quaternion.Euler(1, -90, 1);

            // si on appuie sur le bouton "a" du controller, ou du clavier, alors on va lacher le stylo
            if (OVRInput.GetUp(OVRInput.RawButton.A) || Input.GetKeyUp(KeyCode.A))
            {
                // on informe tout le monde que le stylo a été laché, et on effectue certaines
                // actions comme modifier la gravité, car sinon le resterais en l'air, ayant désactivé la gravité
                // lorsque l'on a pris le stylo
                styloGrab.GetComponent<PhotonView>().RPC("GrabEnd", RpcTarget.All);
                styloGrab.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.CurrentRoom.MasterClientId);
                styloGrab = null;
                lastGrab = Time.realtimeSinceStartup;
            }
        }
    }

    /// <summary>
    /// Cette fonction est appelé lorsqu'il y a un Collider qui reste en collision avec l'objet qui possède ce script
    /// et qui doit aussi posseder un Collider avec "isTrigger" d'activé
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (!styloGrab) // si on n'a pas déjà pris un stylo
        {
            if (other.tag.Equals("Stylo"))
            {
                if (OVRInput.GetUp(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.A)) // si on appuie sur le bouton "a" du controller, ou du clavier
                {
                    // temps afin de ne pas lacher et reprendre immédiatement le stylo, cf explication de lastGrab au dessus
                    if (lastGrab + 0.05f < Time.realtimeSinceStartup) 
                    {
                        // ici on prend prend le stylo, et initialise l'ensemble des delta

                        styloGrab = other.gameObject;

                        deltaPos = new Vector3(styloGrab.transform.position.x - this.transform.position.x,
                                                styloGrab.transform.position.y - this.transform.position.y,
                                                styloGrab.transform.position.z - this.transform.position.z);
                        deltaRot = Quaternion.identity;

                        deltaRot2.x = deltaRot.z;
                        deltaRot2.y = deltaRot.y;
                        deltaRot2.z = deltaRot.x;

                        styloGrab.GetComponent<PhotonView>().RPC("Grab", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId);
                        // Ce RCP permet d'informer à tous que le stylo a été pris, et de plus il effectue certaines
                        // actions comme modifier la gravité, car sinon le stylo suivrait la position, mais "tomberais" du fait de la gravité
                    }
                }
            }
        }
    }

}