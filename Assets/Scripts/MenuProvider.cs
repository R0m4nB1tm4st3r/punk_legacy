using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuProvider : MonoBehaviour
{
    private const int GameEntrySceneId = 0;

    private int ActiveSceneIndex { get => SceneManager.GetActiveScene().buildIndex; }
    private int TopLevelMenuChildIndex {
        get => ActiveSceneIndex == GameEntrySceneId ? 1 : 0; }
    private int SettingsMenuChildIndex {
        get => ActiveSceneIndex == GameEntrySceneId ? 2 : 1; }

    public GameObject TopLevelMenu { get => transform.GetChild(TopLevelMenuChildIndex).gameObject; }
    public GameObject SettingsMenu { get => transform.GetChild(SettingsMenuChildIndex).gameObject; }
}
