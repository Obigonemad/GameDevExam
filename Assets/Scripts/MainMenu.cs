using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
  
  
        SceneManager.LoadSceneAsync("LevelOne");
  
  
    }
    
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}