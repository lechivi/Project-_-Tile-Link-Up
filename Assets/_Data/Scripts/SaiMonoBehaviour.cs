using UnityEngine;

public class SaiMonoBehaviour : MonoBehaviour
{
    [ContextMenu("Load Component")]
    private void LoadComponentContextMenu()
    {
        this.LoadComponent();
    }

    protected virtual void Awake()
    {
        this.LoadComponent();
    }

    protected virtual void LoadComponent()
    {
        //For override
    }
}
