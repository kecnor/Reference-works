using Unity.Netcode;
using UnityEngine;

public class JoinToLobby : MonoBehaviour
{
    #region Variables
    //Scriptable Object
    [SerializeField] private ToggleVisibility toggleVisibility;

    //Scene
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject multiplayerHostMenu;
    [SerializeField] private GameObject multiplayerClientMenu; 
    [SerializeField] private GameObject waitingForMultiplayer;

    // Prefab
    [SerializeField] private GameObject player;
    #endregion

    //How should a Host behave with a multiplayer lobby
    public void StartHost()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            toggleVisibility.ChangeObjects(mainMenu, multiplayerHostMenu);
            Debug.Log("Joined to the lobby as the host");
        }
        else
        {
            Debug.Log("There is already a host");
        }
    }

    public void StopHost()
    {
        toggleVisibility.ChangeObjects(multiplayerHostMenu, mainMenu);
        NetworkManager.Singleton.Shutdown();
        Debug.Log("Closed the lobby");
    }

    //How should a Client behave with a multiplayer lobby

    public void StartClient()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnected;
        toggleVisibility.ChangeObjects(mainMenu, multiplayerClientMenu);
        toggleVisibility.DisappearObject(player);
        NetworkManager.Singleton.StartClient();
        toggleVisibility.AppearObject(waitingForMultiplayer);
    }

    public void StopClient()
    {
        toggleVisibility.ChangeObjects(multiplayerClientMenu, mainMenu);
        NetworkManager.Singleton.Shutdown();
        Debug.Log("Disconnected from the lobby");
    }

    private void OnConnected(ulong clientId)
    {
        toggleVisibility.DisappearObject(waitingForMultiplayer);
        toggleVisibility.AppearObject(player);
        Debug.Log("Joined to the lobby as a Client");
    }

    private void OnDisconnected(ulong clientId)
    {
        if (multiplayerClientMenu.activeSelf)
        {
            toggleVisibility.ChangeObjects(multiplayerClientMenu, mainMenu);
            toggleVisibility.AppearObject(waitingForMultiplayer);
            Debug.Log("The is no longer a host");
        }

        NetworkManager.Singleton.OnClientConnectedCallback -= OnConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnDisconnected;
    }
}