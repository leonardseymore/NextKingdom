using UnityEngine;

public class DontDestroy : MonoBehaviour {

    static DontDestroy instance;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        
    }
}
