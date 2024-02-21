using System.Collections;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    private const string PlayerTag = "Player";

    public Vector3 CameraTargetPosition { 
        get => new(player.transform.position.x, transform.position.y, transform.position.z); }

    private GameObject player = null;
    private InputController inputController = null;

    private IEnumerator followPlayerCoroutine = null;
 
    void Start()
    {
        player = GameObject.FindWithTag(PlayerTag);
        inputController = FindObjectOfType<InputController>();

        inputController.StartMoveEvent.AddListener(StartFollowing);
        inputController.StopMoveEvent.AddListener(StopFollowing);

        transform.position = CameraTargetPosition;
    }

    private void OnDisable()
    {
        inputController.StartMoveEvent.RemoveListener(StartFollowing);
        inputController.StopMoveEvent.RemoveListener(StopFollowing);
    }

    private void StartFollowing()
    {
        followPlayerCoroutine = FollowPlayer();
        StartCoroutine(followPlayerCoroutine);
    }

    private void StopFollowing()
    {
        if (followPlayerCoroutine != null) StopCoroutine(followPlayerCoroutine);
    }

    private IEnumerator FollowPlayer()
    {
        while(true)
        {
            transform.position = CameraTargetPosition;
            yield return null;
        }
    }
}
