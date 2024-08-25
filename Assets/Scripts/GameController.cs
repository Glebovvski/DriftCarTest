using Core;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private Button joinGame;
    [SerializeField] private Button hostGame;

    [Inject] private PlayerData _playerData;
    
    private void Awake()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        SceneManager.sceneLoaded += TryStopHost;
        hostGame.onClick.AddListener(Host);
        joinGame.onClick.AddListener(Join);
    }

    private void Join()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.BitConverter.GetBytes((int)_playerData.CarSettings.ControlType);
        NetworkManager.Singleton.StartClient();
    }

    private void TryStopHost(Scene scene, LoadSceneMode arg1)
    {
        if (scene.buildIndex == 0 && NetworkManager.Singleton.IsHost) //is main menu scene
            NetworkManager.Singleton.Shutdown();
    }

    private void Host()
    {
        Debug.LogError(_playerData.CarSettings.ControlType.ToString());
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.BitConverter.GetBytes((int)_playerData.CarSettings.ControlType);
        NetworkManager.Singleton.StartHost();
    }
}