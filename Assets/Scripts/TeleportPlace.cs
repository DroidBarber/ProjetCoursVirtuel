using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlace : MonoBehaviour
{
    // Start is called before the first frame update
    public PlacesController placesController;
    private int indexPlace = 0;
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.5f || Input.GetKeyUp(KeyCode.K))
        { 
            Vector3 placePosition = placesController.getPlaceTransform(indexPlace);
            this.transform.position = placePosition;
            indexPlace++;
        }
    }
}
