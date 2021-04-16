using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class MicroOculus : MonoBehaviour
{
    Recorder r;
    // Start is called before the first frame update
    void Awake()
    {
        r = this.gameObject.GetComponent<Recorder>();
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }

        if (UnityEngine.Microphone.devices.Length >=3)
        {
            r.UnityMicrophoneDevice = UnityEngine.Microphone.devices[2];
        }
        else if (UnityEngine.Microphone.devices.Length >= 1)
        {
            r.UnityMicrophoneDevice = UnityEngine.Microphone.devices[0];
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        //r.UnityMicrophoneDevice = UnityEngine.Microphone.devices[2];

    }
    
}
