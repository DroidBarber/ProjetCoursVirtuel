﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))] // Oblige que l'objet qui possède ce script à posséder un Text, et s'il n'en a pas, en crée un
public class Log_UI : MonoBehaviour
{
    private List<string> listeLog = new List<string>();
    private List<float> listeChrono = new List<float>();
    private Text textObj;
    public float tempsaff = 15;
    public GameObject imageBackgroundLog;
    private bool isActive = true;

    // Start is called before the first frame update
    void Awake()
    {
        textObj = this.gameObject.GetComponent<Text>();
        textObj.text = "";

        if (!imageBackgroundLog)
        {
            Debug.LogError("imageBackgroundLog non assigné");
        }
        imageBackgroundLog.SetActive(false); // Par défaut, non affiché
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Text>().text = "";

        // Supprimer les logs qui sont "périmé" pour leur temps d'affichage
        for (int i = listeLog.Count - 1; i >= 0; i--)
        {
            listeChrono[i] -= Time.deltaTime;
            if (listeChrono[i] <= 0)
            {
                listeLog.RemoveAt(i);
                listeChrono.RemoveAt(i);
            }
        }

        if (isActive)
        {

            for (int i = 0; i < listeLog.Count; i++)
            {
                textObj.text += listeLog[i];
                if (i < listeLog.Count - 1) // si ce n'est pas le dernier log, on retourne à la ligne
                {
                    textObj.text += "\n";
                }
            }
        }
        

        if (textObj.text == "") // si aucun log, on désactive l'image background
            imageBackgroundLog.SetActive(false);
        else
            imageBackgroundLog.SetActive(true);

    }
    public void AjoutLog(string log)
    {
        listeLog.Add(log);
        listeChrono.Add(tempsaff);
    }
    public void AjoutLog(string log, float time)
    {
        listeLog.Add(log);
        listeChrono.Add(time);
    }


    public void changeisActive()
    {
        this.isActive = !this.isActive;
    }

}