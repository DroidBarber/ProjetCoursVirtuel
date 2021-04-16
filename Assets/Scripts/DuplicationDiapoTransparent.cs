using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
