using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int GameEntrySceneId = 0;
    private const int Level01SceneId = 1;
    private const float MasterVolumeDefault = 1.0f;
    private const string UIMainTag = "UI";
    private const string PlayerPrefsMasterVolumeKey = "MasterVolume";

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

    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsMasterVolumeKey))
            PlayerPrefs.SetFloat(PlayerPrefsMasterVolumeKey, MasterVolumeDefault);

        MasterVolume = PlayerPrefs.GetFloat(PlayerPrefsMasterVolumeKey, MasterVolumeDefault);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(PlayerPrefsMasterVolumeKey, MasterVolume);
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Level01SceneId);
    }

    public void GoToSettings()
    {
        menuProvider.TopLevelMenu.SetActive(false);
        menuProvider.SettingsMenu.SetActive(true);
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Level01SceneId);
    }

    public void GoBackToTopLevelMenu()
    {
        menuProvider.TopLevelMenu.SetActive(true);
        menuProvider.SettingsMenu.SetActive(false);
    }

    public void GoBackToMainScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameEntrySceneId);
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
