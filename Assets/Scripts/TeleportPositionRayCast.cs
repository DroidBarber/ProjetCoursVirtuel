using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script fonctionnel mais qui ne marche pas complettement
/// Il permet de se téléporter à une position dans la salle en la pointant, néanmoins en le faisant, 
/// cela nous téléporte à une position opposé à l'endroit où nous avons "pointé" avec le controller
/// </summary>
/// 
public class TeleportPositionRayCast : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        if (!player)
        {
            Debug.LogError("player non assigné");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(this.transform.position, this.transform.forward*10);
        
     /*   RaycastHit hit;
        Ray ray = new Ray(this.transform.position, this.transform.forward);

        int layer_mask = LayerMask.GetMask("Ground");
        if (OVRInput.Get(OVRInput.RawButton.B) || Input.GetKey(KeyCode.B))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask))
            {
                // afficher le raycast
            }
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.B) || Input.GetKeyUp(KeyCode.B))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask))
            {
                player.transform.position = new Vector3(hit.point.x, player.transform.position.y, hit.point.z);
                //Il faudrait prendre en compte la différence de hauteur au niveau de l'estrade!!!
            }
        }*/
        
    }
}
