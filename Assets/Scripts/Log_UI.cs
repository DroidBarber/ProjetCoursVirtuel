using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log_UI : MonoBehaviour
{
    private List<string> listeLog = new List<string>();
    private List<float> listeChrono = new List<float>();
    public float tempsaff = 15;

    // Start is called before the first frame update
    void Awake()
    {
        this.gameObject.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Text>().text = "\n";
        for (int i = 0; i < listeLog.Count; i++)
        {
            listeChrono[i] -= Time.deltaTime;
            if(listeChrono[i] <= 0)
            {
                listeLog.RemoveAt(i);
                listeChrono.RemoveAt(i);
                i--;
            }
            else
            {
                this.gameObject.GetComponent<Text>().text += listeLog[i];
            }
        }

    }
    public void AjoutLog(string log)
    {
        listeLog.Add(log);
        listeChrono.Add(tempsaff);
    }
}