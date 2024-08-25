using System.Collections;
using Car;
using Core;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace NetworkTools
{
    public class ConnectionApprovalComponent : MonoBehaviour
    {
        public NetworkPrefabsList prefabs;

        private CarManager carManager;
        [Inject] private PlayerData playerData;

        private void Start()
        {
            if (NetworkManager.Singleton.ConnectionApprovalCallback == null)
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
            } while (!carManager);

            ApproveRequest(request, response);
        }

        private void ApproveRequest(NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            var clientId = request.ClientNetworkId;

            response.CreatePlayerObject = false;
            response.Position = carManager.GetPosition((int)clientId);
            var controlType = System.BitConverter.ToInt32(request.Payload);
            response.PlayerPrefabHash = prefabs.PrefabList[0].SourceHashToOverride;
            carManager.SpawnClientCar(clientId, playerData, (ControlType)controlType);

            playerData.SetCar(carManager);
            response.Approved = true;
            response.Pending = false;
        }
    }
}