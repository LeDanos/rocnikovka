using TMPro;
using UnityEngine;

public class ClearButton : MonoBehaviour, IInteractable{
    public TextMeshPro codeText;
    public void Interact(){
        GameObject.Find("Confirm Button").GetComponent<ConfirmButton>().codeC[0]=0;
        GameObject.Find("Confirm Button").GetComponent<ConfirmButton>().codeC[1]=0;
        GameObject.Find("Confirm Button").GetComponent<ConfirmButton>().codeC[2]=0;
        GameObject.Find("Confirm Button").GetComponent<ConfirmButton>().codeC[3]=0;
        GameObject.Find("Confirm Button").GetComponent<ConfirmButton>().changing=0;
        codeText.text="____";
    }
}