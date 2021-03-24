using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlacesController : MonoBehaviour
{
    public Transform leftA; //rangées gauche, gameobject avant gauche
    public Transform leftB; //rangées gauche, gameobject arriere droit
    public Transform rightA; //rangées droite, gameobject avant gauche
    public Transform rightB; //rangées droite, gameobject arriere droit

    private int nbRangeesGauche = 9;
    private int nbChaisesGauche = 7;
    private int nbRangeesDroite = 8;
    private int nbChaisesDroite = 6;
    private float offsetLeftZ;
    private float offsetLeftX;
    private float offsetRightZ;
    private float offsetRightX;

    void Start()
    {
        offsetLeftZ = (leftB.position.z - leftA.position.z)/(nbChaisesGauche-1); //distance entre deux chaises lateralement
        offsetLeftX = (leftB.position.x - leftA.position.x)/(nbRangeesGauche-1); //distance entre deux rangees
        offsetRightZ = (rightB.position.z - rightA.position.z)/(nbChaisesDroite-1); //distance entre deux chaises lateralement
        offsetRightX = (rightB.position.x - rightA.position.x)/(nbRangeesDroite-1); //distance entre deux rangees
    }
    
    public Vector3 getPlaceTransform(int indexPlace) //index commence à 0
    {
        if(indexPlace >= nbRangeesGauche * nbChaisesGauche + nbRangeesDroite * nbChaisesDroite)
        {
            Debug.LogError("IndexPlace dépasse le nombre total de places !");
            return new Vector3(0, 0, 0);
        }
        else if (indexPlace >= nbRangeesGauche * nbChaisesGauche)
        {
            indexPlace = indexPlace - nbRangeesGauche * nbChaisesGauche;
            int numRangee = indexPlace/nbChaisesDroite;
            Debug.Log("numRangee : "+ numRangee); 
            int numChaise = indexPlace - numRangee * nbChaisesDroite;
            Debug.Log("numChaise : "+ numChaise); 
            Vector3 place = new Vector3(offsetRightX * numRangee + rightA.position.x, rightA.position.y, offsetRightZ * numChaise + rightA.position.z);
            Debug.Log("Position index : " + indexPlace + " | Pos : " + place);
            return place;
        } 
        else
        {
            int numRangee = indexPlace/nbChaisesGauche;
            int numChaise = indexPlace - numRangee * nbChaisesGauche;
            Vector3 place = new Vector3(offsetLeftX * numRangee + leftA.position.x, leftA.position.y, offsetLeftZ * numChaise + leftA.position.z);
            return place;
        }
    }
}
