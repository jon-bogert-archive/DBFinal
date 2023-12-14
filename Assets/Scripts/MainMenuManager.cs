using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string gameScene;
    [SerializeField] string shopScene;
    [SerializeField] string startScene;
    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void LoadShop()
    {
        SceneManager.LoadScene(shopScene);
    }

    public void LogOut()
    {
        SceneManager.LoadScene(startScene);
    }
}
