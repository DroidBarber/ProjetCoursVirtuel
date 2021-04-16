using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNameToJoin : MonoBehaviour
{
    public string roomName;
    public string avatarName;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
