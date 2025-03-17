using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void OnHostClicked()
    {
        NetworkManager.instance.StartGame(GameMode.Host);
    }
    public void OnJoinClicked()
    {
        NetworkManager.instance.StartGame(GameMode.Client);
    }
}