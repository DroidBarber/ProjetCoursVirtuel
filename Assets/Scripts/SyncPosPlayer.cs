using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Ce script permet de "recopier" la position et la rotation du joueur et de ses controllers, afin de les synchroniser avec
/// un modèle simplifier qui lui sera instancié et synchronisé en multijoueur
/// </summary>
public class SyncPosPlayer : MonoBehaviour
{
    private GameObject player;
    private GameObject leftHandAnchor, rightHandAnchor;
    private GameObject m_leftHandAnchor, m_rightHandAnchor;
    private bool isSetup = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (!this.gameObject.GetComponent<PhotonView>().IsMine) // si ce n'est pas mon gameobject, alors je ne dois rien faire
            Destroy(this);
        else
        {
            if (!this.GetComponent<Renderer>()) // nécessaire selon les avatars (notamment entre celui de la capsule et les "vrai" avatar)
            {
                for (int i = 0; i < this.gameObject.transform.childCount; i++)
                {
                    var currentChild = this.gameObject.transform.GetChild(i);
                    if (currentChild.CompareTag("Avatar"))
                    {
                        currentChild.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                this.GetComponent<Renderer>().enabled = false;
            }
        }

    }

    /// <summary>
    /// Mise à jour en continue des données, afin qu'elle soit synchronisé ensuite par le photon transform view
    /// </summary>
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

    /// <summary>
    /// Permet de rechercher et associer les différents gameobjects
    /// </summary>
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

    /// <summary>
    /// Permet de faire une recherche réccursive dans une arboressence de Gameobject
    /// </summary>
    /// <returns></returns>
    private GameObject SearchInChild(string name, GameObject parent)
    {
        if (parent.name == name)
            return parent.gameObject;
        foreach (Transform child in parent.transform)
        {
            GameObject temp = SearchInChild(name, child.gameObject);
            if (temp)
                return temp;
        }
        return null;
    }
}