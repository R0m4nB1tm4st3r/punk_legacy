using UnityEngine;

public class MenuProvider : MonoBehaviour
{
    private const int MainMenuChildIndex = 1;
    private const int SettingsMenuChildIndex = 2;

    public GameObject MainMenu { get => transform.GetChild(MainMenuChildIndex).gameObject; }
    public GameObject SettingsMenu { get => transform.GetChild(SettingsMenuChildIndex).gameObject; }
}
