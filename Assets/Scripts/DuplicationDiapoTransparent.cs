using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ce script ne sert juste à régler la transparence des duplications (diapo et tableau)
/// Nous ne pouvons pas tout simplement régler la transparence dans le material, car cela 
/// modifierais aussi celle des "vrai" tableau et diapo, ce qui n'est pas le but initial
/// C'est pourquoi il faut modifier l'alpha uniquement lorsque l'application est lancé, car
/// ainsi, cette modification (alpha) n'impacte que l'objet auquel ce script est affecté, et 
/// pas tout les objets qui ont ce même material
/// </summary>
public class DuplicationDiapoTransparent : MonoBehaviour
{
    [Range(0,1)]
    public float alpha = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        Color c = this.gameObject.GetComponent<Renderer>().materials[0].color;
        c.a = alpha;
        this.gameObject.GetComponent<Renderer>().materials[0].color = c;
    }

    // Update is called once per frame
    void Update()
    {
        // juste pour le test, à enlever
        Color c = this.gameObject.GetComponent<Renderer>().materials[0].color;
        c.a = alpha;
        this.gameObject.GetComponent<Renderer>().materials[0].color = c;
    }
}
