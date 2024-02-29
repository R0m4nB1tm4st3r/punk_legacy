using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int Level01SceneId = 1;
    private const string UIMainTag = "UI";

    [field: SerializeField]
    public float MasterVolume { get; set; } = 1f;

    private MenuProvider menuProvider = null;

    void Start()
    {
        menuProvider = GameObject.FindGameObjectWithTag(UIMainTag).GetComponent<MenuProvider>();
    }

    public void StartGame() => UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Level01SceneId);

    public void GoToSettings()
    {
        menuProvider.MainMenu.SetActive(false);
        menuProvider.SettingsMenu.SetActive(true);
    }

    public void GoBackToMainMenu()
    {
        menuProvider.MainMenu.SetActive(true);
        menuProvider.SettingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
