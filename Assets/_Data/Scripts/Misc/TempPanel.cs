using UnityEngine;
using UnityEngine.SceneManagement;

public class TempPanel : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReplayGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
