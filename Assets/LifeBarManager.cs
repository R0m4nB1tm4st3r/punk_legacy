using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarManager : MonoBehaviour
{
    private const float HealthBarFillDefault = 1f;

    [field: SerializeField]
    public DamagableObject TargetDamagable { get; set; }

    private Slider lifeBar = null;
    private IEnumerator subscribeCoroutine = null;

    private void OnEnable()
    {
        lifeBar = GetComponent<Slider>();
        lifeBar.value = HealthBarFillDefault;

        subscribeCoroutine = SubscribeToDmgReceiveEvent();
        StartCoroutine(subscribeCoroutine);
    }

    private void UpdateHealthBar()
    {
        lifeBar.value = TargetDamagable.CurrentHp / TargetDamagable.MaxHp;
    }

    private IEnumerator SubscribeToDmgReceiveEvent()
    {
        while (TargetDamagable.ReceiveDmgEvent == null)
        {
            yield return null;
        }

        TargetDamagable.ReceiveDmgEvent.AddListener(UpdateHealthBar);
    }
}
