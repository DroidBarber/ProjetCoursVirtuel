using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ce script permet de déplacer le joueur dans les version PC
/// Pour ce faire, il suffit d'utiliser les flèches directionnelle du clavier, et la souris
/// </summary>
public class MovePlayerPC : MonoBehaviour
{
    public float speedMove = 1.0f;
    public float speedRotate = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        pos *= Time.deltaTime * speedMove;
        this.transform.Translate(pos, Space.Self);

        Vector3 rot = new Vector3(0, Input.GetAxis("Mouse X"), 0);
        rot *= Time.deltaTime * speedRotate;
        this.transform.Rotate(rot, Space.Self);
    }
}
