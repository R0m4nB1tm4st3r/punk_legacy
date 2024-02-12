using UnityEngine;

public class Platform : WalkableObject
{
    public override bool IsPlatform { get; } = true;
    public override bool IsGround { get; } = false;
}
