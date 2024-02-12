using UnityEngine;

public class WalkableObject : MonoBehaviour
{
	public virtual bool IsPlatform { get; }
	public virtual bool IsGround { get; }
}
