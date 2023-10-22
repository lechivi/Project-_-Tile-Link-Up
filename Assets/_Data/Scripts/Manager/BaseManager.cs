using UnityEngine;

public class BaseManager<T> : SaiMonoBehaviour where T : BaseManager<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindObjectOfType<T>();

                if (instance == null)
                {
                    Debug.LogError($"No {typeof(T).Name} Singleton Instance.");
                }
            }

            return instance;
        }
    }

    public static bool HasInstance
    {
        get
        {
            return (instance != null);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = (T)this;
            DontDestroyOnLoad(this);
            return true;
        }
        else if (instance == this)
        {
            return true;
        }

        Object.Destroy(this.gameObject);
        return false;
    }
}
