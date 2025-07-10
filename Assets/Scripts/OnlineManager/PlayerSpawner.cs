using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;

namespace EternalFlame
{
    public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private NetworkPrefabRef playerPrefab;
        [SerializeField] InputSystem_Actions inputActions;
        private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();


        void Start()
        {
            inputActions = new InputSystem_Actions();
            inputActions.Enable();
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var inputData = new NetworkInputData
            {
                movement = inputActions.Player.Move.ReadValue<Vector2>(),
                jump = inputActions.Player.Jump.triggered,
             
            };
            input.Set(inputData);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer) // chỉ server/host spawn
            {
                Debug.Log($"Player {player} joined. Spawning character...");
                Vector2 spawnPosition = new Vector2((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 10);
                var playerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

                Debug.Log($"Spawned for PlayerRef: {player} - InputAuthority: {playerObject.InputAuthority}");
            }
            else
            {
                Debug.Log($"Player {player} joined but not spawning character as client.");
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (spawnedCharacters.TryGetValue(player, out NetworkObject playerObject))
            {
                runner.Despawn(playerObject);
                spawnedCharacters.Remove(player);
            }
        }

        // Các callback khác giữ nguyên như cũ...
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            // Xử lý khi mất kết nối với server (nếu cần)
        }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            Debug.LogError($"Connect failed to {remoteAddress}: {reason}");
        }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {

        }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }


    }
}