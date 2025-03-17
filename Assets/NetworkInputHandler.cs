using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInputHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //For Pc
        networkInputData.horizontalInput = Input.GetAxisRaw("Horizontal");
        networkInputData.isReversing = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        //For Mobile
        HandleMobileInput(ref networkInputData);

        input.Set(networkInputData);

    }

    private void HandleMobileInput(ref NetworkInputData networkInputData)
    {
        bool leftPressed = false;
        bool rightPressed = false;

        int activeTouches = 0;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                activeTouches++;

                if (touch.position.x < Screen.width / 2)
                {
                    leftPressed = true;
                }
                else if (touch.position.x >= Screen.width / 2)
                {
                    rightPressed = true;
                }
            }
        }
    

        if (activeTouches > 0)
        {
            if (leftPressed && !rightPressed)
                networkInputData.horizontalInput = -1f;
            else if (rightPressed && !leftPressed)
               networkInputData.horizontalInput = 1f;
            else
                networkInputData.horizontalInput = 0f;

            networkInputData.isReversing = leftPressed && rightPressed;
        }
    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject networkObject, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject networkObject, PlayerRef player) { }



}
