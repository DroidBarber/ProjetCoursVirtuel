using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListeBouton: MonoBehaviour
{
    [SerializeField]
    private Text myText;

    [SerializeField]
    private ListeBoutonController boutonControl;

    private string myTextString;

    public void SetText(string textString)
    {
        myTextString = textString;
        myText.text = textString;

    }

    public void OnClick()
    {
        boutonControl.ButtonClicked(myTextString);

    }
  
}
