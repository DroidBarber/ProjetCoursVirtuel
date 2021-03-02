using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.DrawRay(this.transform.position, this.transform.forward*10);
        
        RaycastHit hit;
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
        }
        
    }
}
