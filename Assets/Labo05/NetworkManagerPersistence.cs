using UnityEngine;
using Unity.Netcode;

public class NetworkManagerPersistence : MonoBehaviour
{
    private void Awake()
{
    if (NetworkManager.Singleton != this)
    {
        Destroy(gameObject);
        return;
    }

    DontDestroyOnLoad(gameObject); 
}
}
