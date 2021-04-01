using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SyncPosPlayer : MonoBehaviour
{
    private GameObject player;
    private GameObject leftHandAnchor, rightHandAnchor;
    private GameObject m_leftHandAnchor, m_rightHandAnchor;
    private bool isSetup = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (!this.gameObject.GetComponent<PhotonView>().IsMine)
            Destroy(this);
        else
            this.GetComponent<Renderer>().enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSetup)
        {
            this.transform.position = player.transform.position;
            this.transform.rotation = player.transform.rotation;
            m_leftHandAnchor.transform.position = leftHandAnchor.transform.position;
            m_leftHandAnchor.transform.rotation = leftHandAnchor.transform.rotation;
            m_rightHandAnchor.transform.position = rightHandAnchor.transform.position;
            m_rightHandAnchor.transform.rotation = rightHandAnchor.transform.rotation;
        }
    }
    public void setup(GameObject player)
    {
        this.player = player;
        leftHandAnchor = SearchInChild("LeftHandAnchor", player);
        rightHandAnchor = SearchInChild("RightHandAnchor", player);
        m_leftHandAnchor = SearchInChild("LeftHandAnchor", this.gameObject);
        m_rightHandAnchor = SearchInChild("RightHandAnchor", this.gameObject);

        foreach (Transform child in m_leftHandAnchor.transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in m_rightHandAnchor.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (!leftHandAnchor || !rightHandAnchor || !m_leftHandAnchor || !m_rightHandAnchor)
        {
            Debug.LogError("Erreur dans la recherche de SearchInChild");
        }
        else
            isSetup = true;
    }

    private GameObject SearchInChild(string name, GameObject parent)
    {
        if (parent.name == name)
            return parent.gameObject;
        foreach (Transform child in parent.transform)
        {
            GameObject temp =  SearchInChild(name, child.gameObject);
            if (temp)
                return temp;
        }
        return null;
    }
}
