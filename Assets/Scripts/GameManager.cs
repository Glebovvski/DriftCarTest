using Unity.Netcode;
using UnityEngine.SceneManagement;

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
