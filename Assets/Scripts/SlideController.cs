using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Texture2D> diapo = new List<Texture2D>();
    private Material material;
    private Renderer renderer;

    private void Awake()
    {
        if (this.GetComponent<Renderer>()) 
        {
            renderer = this.GetComponent<Renderer>(); //recuperation du renderer
            renderer.enabled = false;
            material = renderer.material; //recuperation du material
            material.SetTexture("_MainTex",null); //initialisation à null du tableau
        }
        else
        {
            Debug.LogError("Missing Renderer");
        }
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))//appuyer sur M pour activer ou desactiver le diapo
        {
            renderer.enabled = !renderer.enabled;
        }
        else if (Input.GetKeyUp(KeyCode.O)) //diapo 1
        {
            material.SetTexture("_MainTex",diapo[0]);
        } else if (Input.GetKeyUp(KeyCode.P)) //diapo 2
        {
            material.SetTexture("_MainTex",diapo[1]);
        }
    }
}
