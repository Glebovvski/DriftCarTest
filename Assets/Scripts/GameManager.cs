using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager
{
    public void Play()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("Racing", LoadSceneMode.Single);
    }

    public void Exit()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
