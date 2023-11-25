using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private const string level1name = "level1";
    private const string level2name = "level2";
    public void OnLevel1ButtonPressed()
    {
        SceneManager.LoadScene(level1name);
    }
    public void OnLevel2ButtonPressed()
    {
        SceneManager.LoadScene(level2name);
    }
    public void OnExitToDekstopButtonPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit(); 
    }
}
