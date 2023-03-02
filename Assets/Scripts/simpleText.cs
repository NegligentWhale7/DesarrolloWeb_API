using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class simpleText : MonoBehaviour
{
    public ContenedorPersonas contenedorPersonas;

    TMP_Text texto;
  void Awake(){
    texto=gameObject.GetComponent<TMP_Text>();
  } 
    
  public void printText() {
    string personasText ="";
    foreach(Persona p in contenedorPersonas.personas){
        personasText += "- " + p.nombre+"<br> ";
    }
    texto.text=personasText;
  }
    public void ClearText()
    {
        string personasText = "";
        texto.text=personasText;
    }
    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
