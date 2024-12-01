using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Testing : MonoBehaviour
{
    public void Logging()
    {
        // Find inputMessage
        GameObject inputMessage = GameObject.Find("inputMessage");
        if (inputMessage != null)
        {
            TMP_InputField tmpInputField = inputMessage.GetComponent<TMP_InputField>();
            if (tmpInputField != null)
            {
                UnityEngine.Debug.Log("Input Message Text (TextMeshPro): " + tmpInputField.text);
            }
        }

        // Find inputPreview
        GameObject inputPreview = GameObject.Find("inputPreview");
        if (inputPreview != null)
        {
            TMP_InputField tmpInputFieldPreview = inputPreview.GetComponent<TMP_InputField>();
            if (tmpInputFieldPreview != null)
            {
                UnityEngine.Debug.Log("Input Preview Text (TextMeshPro): " + tmpInputFieldPreview.text);
            }
        }

        // Find txtStatus
        GameObject txtStatus = GameObject.Find("txtStatus");
        if (txtStatus != null)
        {
            TextMeshProUGUI tmpTextStatus = txtStatus.GetComponent<TextMeshProUGUI>();
            if (tmpTextStatus != null)
            {
                UnityEngine.Debug.Log("Text Status Content (TextMeshPro): " + tmpTextStatus.text);
            }
        }
    }
}
