using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MeleeController : MonoBehaviour
{
	private const int MaxComboCount = 3;

	[field: SerializeField]
	public AudioClip PunchClip {  get; set; } = null;

	public bool HasPunchAnimationEnded { get; set; } = false;
	public int ComboCount { get; private set; } = 0;
	public UnityEvent<int> UpdateComboCounterEvent { get; private set; } = null;

	private InputController inputController = null;
	private DamageController damageController = null;
	private HitBoxController[] meleeHitBoxes = null;
	private GameManager gameManager = null;
	private AudioSource audioSource = null;

	IEnumerator punchCoroutine = null;

	private void Awake()
	{
		UpdateComboCounterEvent = new UnityEvent<int>();
	}

	void Start()
    {
		inputController = FindObjectOfType<InputController>();
		damageController = GetComponent<DamageController>();
		audioSource = GetComponent<AudioSource>();

		EnableMeleeControls();
		damageController.DieEvent.AddListener(DisableMeleeControls);

		meleeHitBoxes = new HitBoxController[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			meleeHitBoxes[i] = transform.GetChild(i).gameObject.GetComponent<HitBoxController>();
		}
	}

    private void OnEnable()
    {
        gameManager = FindObjectOfType<GameManager>();
		gameManager.ChangeVolumeEvent.AddListener((float newVolume) => audioSource.volume = newVolume);
    }

    private void OnDisable()
	{
		DisableMeleeControls();
		damageController.DieEvent.RemoveListener(DisableMeleeControls);
	}

	void EnableMeleeControls()
	{
		inputController.PunchEvent.AddListener(StartOrEndMelee);
	}

	void DisableMeleeControls(bool shouldDisable = true)
	{
		if (shouldDisable)
		{
			inputController.PunchEvent.RemoveListener(StartOrEndMelee);
		}
	}

	void StartOrEndMelee(bool hasInput)
	{
		if (hasInput)
		{
			punchCoroutine = Punch();
			StartCoroutine(punchCoroutine);
		}
		else
		{
			if (punchCoroutine != null) StopCoroutine(punchCoroutine);
			ComboCount = 0;
		}
	}

	public void TriggerHit(int hitboxIndex)
	{
		meleeHitBoxes[hitboxIndex].Attack();
	}

	public void PlayPunchSound()
	{
		audioSource.PlayOneShot(PunchClip);
	}

	public void IndicatePunchAnimationEnd()
	{
		HasPunchAnimationEnded = true;
	}

	IEnumerator Punch()
	{
		while (true)
		{
			HasPunchAnimationEnded = false;
			ComboCount = (ComboCount % MaxComboCount) + 1;
			UpdateComboCounterEvent.Invoke(ComboCount);
			yield return new WaitUntil(() => HasPunchAnimationEnded);
		}
	}
}
