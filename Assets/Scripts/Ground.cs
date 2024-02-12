using UnityEngine;

public class Ground : WalkableObject
{
	public override bool IsPlatform { get; } = false;
	public override bool IsGround { get; } = true;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
