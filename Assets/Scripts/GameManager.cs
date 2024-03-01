using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const int GameEntrySceneId = 0;
    private const int Level01SceneId = 1;
    private const float MasterVolumeDefault = 1.0f;
    private const string UIMainTag = "UI";
    private const string PlayerPrefsMasterVolumeKey = "MasterVolume";

    [field: SerializeField]
    public float MasterVolume { get; set; } = 1f;
    [field: SerializeField]
    public AudioClip GameEntryMusic { get; set; } = null;
    [field: SerializeField]
    public AudioClip Level01Music { get; set; } = null;

    public UnityEvent<float> ChangeVolumeEvent = null;

    private MenuProvider menuProvider = null;
    private InputController inputController = null;
    private AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ChangeVolumeEvent = new UnityEvent<float>();
    }

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
        audioSource.volume = MasterVolume;

        if(SceneManager.GetActiveScene().buildIndex == GameEntrySceneId)
            audioSource.clip = GameEntryMusic;
        else
            audioSource.clip = Level01Music;

        audioSource.Play();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(PlayerPrefsMasterVolumeKey, MasterVolume);
        ChangeVolumeEvent.RemoveAllListeners();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(Level01SceneId);
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

    public void ChangeMasterVolume(float newVolume)
    {
        MasterVolume = newVolume;
        audioSource.volume = MasterVolume;
        ChangeVolumeEvent.Invoke(newVolume);
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
