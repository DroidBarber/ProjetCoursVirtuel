using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacesControllerTP : PlacesController
{
    public List<GameObject> lst_places;

    public override Vector3 getPlaceTransform(int indexPlace)
    {
        if (lst_places.Count >= indexPlace)
        {
            return lst_places[indexPlace].transform.position;
        }
        else
        {
            return new Vector3(0, 1, 0);
        }
    }


}
