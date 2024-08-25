using System.Collections;
using System.Collections.Generic;
using Car;
using Core;
using GameTools;
using Popup;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ConnectionApprovalComponent : MonoBehaviour // NetworkBehaviour
{
    public NetworkPrefabsList prefabs;

    private CarManager carManager;
    private EndGamePopup endGamePopup;
    [Inject] private PlayerData playerData;

    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback +=
            HandleJoinRequests;
    }

    private void HandleJoinRequests(NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {
        response.Pending = true;
        StartCoroutine(InitDependenciesRoutine(request, response));
    }

    IEnumerator InitDependenciesRoutine(NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex > 0);
        do
        {
            carManager = FindObjectOfType<CarManager>();
            endGamePopup = FindObjectOfType<EndGamePopup>();
        } while (!carManager && !endGamePopup);

        ApproveRequest(request, response);
    }

    private void ApproveRequest(NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {
        var clientId = request.ClientNetworkId;

        Debug.LogError("GET CLIENT ID " + clientId);

        // Additional connection data defined by user code
        response.CreatePlayerObject = false;
        response.Position = carManager.GetPosition((int)clientId);
        var controlType = System.BitConverter.ToInt32(request.Payload);
        response.PlayerPrefabHash = prefabs.PrefabList[0].SourceHashToOverride;
        var car = carManager.SpawnClientCar(clientId,playerData,(ControlType)controlType);
        // car.SetControlType((ControlType)controlType);

        // car.SetIsControllable(true);
        // car.SetPlayerData(playerData);
        endGamePopup.SetCar();
        // GameTimer.Instance.SetCar();
        playerData.SetCar(carManager);
        response.Approved = true;
        response.Pending = false;
    }
}