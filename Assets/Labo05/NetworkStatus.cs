using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkStatus : MonoBehaviour
{
    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        UnityEngine.Debug.Log($"Client {clientId} connected");
    }

    private void OnClientDisconnected(ulong clientId)
    {
        UnityEngine.Debug.Log($"Client {clientId} disconnected");
    }
}
