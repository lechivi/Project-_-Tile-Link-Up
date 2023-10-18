using UnityEngine;

public class SaiMonoBehaviour : MonoBehaviour
{
    [ContextMenu("Load Component")]
    private void LoadComponentContextMenu()
    {
        this.LoadCompoent();
    }

    protected virtual void Awake()
    {
        this.LoadCompoent();
    }

    protected virtual void LoadCompoent()
    {
        //For override
    }
}
