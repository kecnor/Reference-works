using TMPro;
using UnityEngine;

public class WriteLog : MonoBehaviour
{
    //Writes the messege on the gameObject's text panel
    public void WriteNewLog(string message)
    {
        GetComponent<TextMeshProUGUI>().text += $"\n{message}";
    }
}