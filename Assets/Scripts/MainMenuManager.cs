using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string gameScene;
    [SerializeField] string shopScene;
    [SerializeField] string accountScene;
    [SerializeField] string startScene;
    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void LoadShop()
    {
        SceneManager.LoadScene(shopScene);
    }

    public void AccountInfo()
    {
        SceneManager.LoadScene(accountScene);
    }

    public void LogOut()
    {
        SceneManager.LoadScene(startScene);
    }
}
