
using System.Collections;
using UnityEngine;

public class DamageController : DamagableObject
{
    private const string PlayerTag = "Player";
    private const float RestartLevelDelay = 2f;

    [field: SerializeField]
    public AudioClip DieClip { get; set; } = null;

    private GameManager gameManager = null;
    private AudioSource audioSource = null;
    private IEnumerator restartLevelCoroutine = null;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();

        gameManager.ChangeVolumeEvent.AddListener((float newVolume) => audioSource.volume = newVolume);

        DieEvent.AddListener((bool isDead) => {
            if (isDead)
            {
                if (audioSource != null)audioSource.PlayOneShot(DieClip);

                if (gameObject.CompareTag(PlayerTag))
                {
                    restartLevelCoroutine = RestartLevel();
                    StartCoroutine(restartLevelCoroutine);
                }
            }
        });
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(RestartLevelDelay);
        gameManager.RestartLevel();
    }
}
