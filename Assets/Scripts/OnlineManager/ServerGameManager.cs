using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EternalFlame 
{
    public class ServerGameManager : MonoBehaviour,INetworkRunnerCallbacks
    {
    private NetworkRunner _runner;

    public async void StartGame()
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        _runner.AddCallbacks(this);

        GameMode mode = GameMode.AutoHostOrClient; // Chế độ tự động host hoặc client
       

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex+1);
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "RoomTest", // Đặt tên phòng để các client join cùng phòng này
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    // Callback khi người chơi tham gia vào game
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }

    // Callback khi người chơi rời khỏi game
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player left: {player}");
    }

    // Callback để gửi dữ liệu đầu vào từ client
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // Dùng để gửi các lệnh đầu vào (Input) đến server
    }

    // Callback khi thiếu đầu vào từ người chơi
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.LogWarning($"Missing input from player: {player}");
    }

    // Callback khi Runner bị tắt
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        // In ra lý do tắt NetworkRunner
    Debug.Log($"NetworkRunner has shut down. Reason: {shutdownReason}");

    // Dọn dẹp tài nguyên
    CleanupResources();

 
    }
    private void CleanupResources(){
    // Ví dụ dọn dẹp các đối tượng liên quan đến mạng
    if (_runner != null)
    {
        Destroy(_runner.gameObject); // Hủy NetworkRunner nếu cần
    }

    // Xóa các tài nguyên khác (nếu có)
    // ...
}
    // Callback khi kết nối thành công đến server
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to server");
    }

    // Callback khi mất kết nối với server
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.LogWarning($"Disconnected from server: {reason}");
    }

    // Callback khi có yêu cầu kết nối từ một client khác
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log($"Connection request from {request.RemoteAddress}");
    }

    // Callback khi kết nối thất bại
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.LogError($"Connect failed to {remoteAddress}: {reason}");
    }

    // Callback khi nhận tin nhắn mô phỏng (user-defined message)
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("Simulation message received");
    }

    // Callback khi danh sách session được cập nhật
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
      
    }


    // Callback khi nhận được phản hồi xác thực từ server
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("Custom authentication response received");
    }

    // Callback khi xảy ra quá trình chuyển đổi host
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
       
    }

    // Callback khi load xong một scene
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("Scene load done");
    }

    // Callback khi bắt đầu load một scene
    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("Scene load started");
    }

    // Callback khi một đối tượng rời khỏi AOI (Area of Interest)
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log($"Object {obj.name} exited AOI for player {player}");
    }

    // Callback khi một đối tượng vào trong AOI (Area of Interest)
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log($"Object {obj.name} entered AOI for player {player}");
    }

    // Callback khi nhận dữ liệu đáng tin cậy từ một player
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        Debug.Log($"Reliable data received from {player}");
    }

    // Callback khi có tiến trình gửi dữ liệu đáng tin cậy
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        Debug.Log($"Reliable data progress from {player}: {progress * 100}%");
    }
    }
}
