using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableauController : MonoBehaviour
{
    private Texture2D texture;
    public GameObject coinHautGauche, coinHautDroite, coinBasGauche, coinBasDroite;

    // Start is called before the first frame update
    void Awake()
    {
        texture = new Texture2D(300, 300);
        this.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Write(Vector3 position, Color c)
    {
        Debug.Log("Position:" + position);

        // z et y
        Vector2 pos = new Vector2();
        pos.x = ((coinHautGauche.transform.position.z - coinHautDroite.transform.position.z) - (position.z - coinHautDroite.transform.position.z)) / (coinHautGauche.transform.position.z - coinHautDroite.transform.position.z);
        pos.y = ((position.y - coinBasGauche.transform.position.y) - (coinHautGauche.transform.position.y - coinBasGauche.transform.position.y)) / (coinHautGauche.transform.position.y - coinBasGauche.transform.position.y);
        pos.x *= texture.width;
        pos.y *= texture.height;
        Debug.Log((int)pos.x + "   " + (int)pos.y);
        texture.SetPixel((int)pos.x, (int)pos.y, c);
        texture.Apply();
    }
}
