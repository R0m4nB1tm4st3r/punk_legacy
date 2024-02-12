using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    static T instance = null;
    public static T Instance 
    { 
        get
        {
            if (instance == null)
            {
				GameObject newGameObject = new()
				{
					name = typeof(T).Name
				};
				instance = newGameObject.AddComponent<T>();
            }
            return instance;
        }
    }

    void Awake()
    {
        Debug.Log("awaking singleton");
        if (Instance != null && Instance != this as T)
        {
            Debug.Log("destroyed duplicate singleton");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("instantiated singleton");
            instance = this as T;
        }
    }
}
