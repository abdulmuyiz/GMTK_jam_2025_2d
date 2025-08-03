using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSpript : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Backshots");
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
