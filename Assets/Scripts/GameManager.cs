using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int GameEntrySceneId = 0;
    private const int Level01SceneId = 1;
    private const string UIMainTag = "UI";

    [field: SerializeField]
    public float MasterVolume { get; set; } = 1f;

    private MenuProvider menuProvider = null;
    private InputController inputController = null;

    void Start()
    {
        menuProvider = Resources.FindObjectsOfTypeAll<MenuProvider>()[0];
        inputController = FindObjectOfType<InputController>();

        if (inputController != null)
        {
            inputController.PauseEvent.AddListener(ShowInGameMenu);
            inputController.UnPauseEvent.AddListener(HideInGameMenu);
        }
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Level01SceneId);
    }

    public void GoToSettings()
    {
        menuProvider.TopLevelMenu.SetActive(false);
        menuProvider.SettingsMenu.SetActive(true);
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Level01SceneId);
    }

    public void GoBackToTopLevelMenu()
    {
        menuProvider.TopLevelMenu.SetActive(true);
        menuProvider.SettingsMenu.SetActive(false);
    }

    public void GoBackToMainScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(GameEntrySceneId);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ShowInGameMenu()
    {
        menuProvider.gameObject.SetActive(true);
    }

    public void HideInGameMenu()
    {
        menuProvider.gameObject.SetActive(false);
    }
}
