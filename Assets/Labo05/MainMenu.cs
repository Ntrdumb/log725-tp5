using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : NetworkBehaviour
{
    [SerializeField] private GameObject clientRender;
    [SerializeField] private GameObject hostRender;
    [SerializeField] private GameObject menuRender;

    //For host
    public TMP_InputField inputMessageField;
    public TMP_InputField inputPreviewField;
    public TextMeshProUGUI txtStatusField;
    private string encryptedMessage;

    //For client
    public TMP_InputField inputMessageReceivedField;
    public TextMeshProUGUI txtGameStatus;
    private StringBuilder currentClientSequence = new StringBuilder();

    //Sound
    public AudioSource beepSound; //For .
    public AudioSource boopSound; //For -

    //Game status
    private bool gameOver = false;

    private void Start()
    {

        if (IsServer)
        {
            HostGame();
        }
        else if (IsClient)
        {
            JoinGame();
        }

        if (inputMessageField != null)
        {
            inputMessageField.onValueChanged.AddListener(UpdatePreviewWithMorse);
        }
    }

    public void HostGame()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.StartHost();
            UnityEngine.Debug.Log("Host started");
            menuRender.SetActive(false);
            clientRender.SetActive(false);
            hostRender.SetActive(true);
            //UnityEngine.SceneManagement.SceneManager.LoadScene("HostScene");
        }
    }

    public void JoinGame()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.StartClient();
            UnityEngine.Debug.Log("Client started");
            menuRender.SetActive(false);
            hostRender.SetActive(false);
            clientRender.SetActive(true);
            //UnityEngine.SceneManagement.SceneManager.LoadScene("JoinScene");
        }
    }

    public void testMsgSend()
    {
        inputMessageReceivedField.text = "message";
    }

    public void SendMessageToClient()
    {
        UnityEngine.Debug.Log("Sending");
        string message = inputMessageField.text;
        encryptedMessage = ConvertToMorse(message);

        //Reset
        gameOver = false;
        currentClientSequence.Clear();
        txtStatusField.text = "";

        SendMessageToClientRpc(message);
    }

    [ClientRpc]
    void SendMessageToClientRpc(string message)
    {
        currentClientSequence.Clear();
        gameOver = false;
        txtGameStatus.text = "";
        UnityEngine.Debug.Log("Sent message");
        inputMessageReceivedField.text = $"{message}";

    }

    //Client side 
    public void SendMorseCharacter(string morseCharacter)
    {
        if (gameOver)
        {
            return;
        }

        SendMorseCharacterServerRpc(morseCharacter);
    }

    [ServerRpc(RequireOwnership = false)]
    void SendMorseCharacterServerRpc(string morseCharacter)
    {
        if (gameOver)
        {
            return;
        }

        if (morseCharacter == ".")
        {
            beepSound.Play();
        }
        else if (morseCharacter == "-")
        {
            boopSound.Play();
        }

        currentClientSequence.Append(morseCharacter);

        UpdateClientSequenceOnHost();

        if (currentClientSequence.Length >= encryptedMessage.Length)
        {
            VerifySequence();
        }
    }

    private void UpdateClientSequenceOnHost()
    {
        txtStatusField.text = $"{currentClientSequence.ToString()}";
    }

    private void Update()
    {
        if (!IsHost && !gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Period))
            {
                SendMorseCharacter(".");
            }
            else if (Input.GetKeyDown(KeyCode.Minus))
            {
                SendMorseCharacter("-");
            }
        }
        else
        {
            return;
        }
    }

    //Host side
    void VerifySequence()
    {
        if (currentClientSequence.ToString() == encryptedMessage)
        {
            SendResultToClientRpc(true);
        }
        else
        {
            SendResultToClientRpc(false);
        }
    }


    [ClientRpc]
    void SendResultToClientRpc(bool isSuccess)
    {
        if (isSuccess)
        {
            txtGameStatus.text = "Félicitations!";
            gameOver = true;
        }
        else
        {
            txtGameStatus.text = "Try again.";
            txtStatusField.text = "";
            currentClientSequence.Clear();
        }
    }

    private void UpdatePreviewWithMorse(string text)
    {
        if (inputPreviewField != null)
        {
            inputPreviewField.text = ConvertToMorse(text);
        }
    }

    private string ConvertToMorse(string text)
    {
        var morseDictionary = new Dictionary<char, string>
        {
            { 'A', ".-" }, { 'B', "-..." }, { 'C', "-.-." }, { 'D', "-.." },
            { 'E', "." }, { 'F', "..-." }, { 'G', "--." }, { 'H', "...." },
            { 'I', ".." }, { 'J', ".---" }, { 'K', "-.-" }, { 'L', ".-.." },
            { 'M', "--" }, { 'N', "-." }, { 'O', "---" }, { 'P', ".--." },
            { 'Q', "--.-" }, { 'R', ".-." }, { 'S', "..." }, { 'T', "-" },
            { 'U', "..-" }, { 'V', "...-" }, { 'W', ".--" }, { 'X', "-..-" },
            { 'Y', "-.--" }, { 'Z', "--.." },
            { '0', "-----" }, { '1', ".----" }, { '2', "..---" }, { '3', "...--" },
            { '4', "....-" }, { '5', "....." }, { '6', "-...." }, { '7', "--..." },
            { '8', "---.." }, { '9', "----." },
            { ' ', "/" }
        };

        string morseCode = "";
        foreach (char c in text.ToUpper())
        {
            if (morseDictionary.ContainsKey(c))
            {
                morseCode += morseDictionary[c];
            }
        }

        return morseCode.Trim();
    }

}
