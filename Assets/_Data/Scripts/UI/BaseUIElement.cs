using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BaseUIElement : SaiMonoBehaviour
{
    [Header("BASE UI")]
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected bool isSetActiveGameObjec;

    public CanvasGroup CanvasGroup { get => this.canvasGroup; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.canvasGroup == null)
            this.canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void Show(object data)
    {
        this.gameObject.SetActive(true);

        this.SetActiveCanvasGroup(true);
    }

    public virtual void Hide()
    {
        if (this.isSetActiveGameObjec)
            this.gameObject.SetActive(false);

        this.SetActiveCanvasGroup(false);
    }

    protected void SetActiveCanvasGroup(bool isActive)
    {
        if (this.canvasGroup != null)
        {
            this.canvasGroup.alpha = isActive ? 1 : 0;
            this.canvasGroup.blocksRaycasts = isActive;
        }
    }

    public bool IsShow()
    {
        return (this.canvasGroup.alpha == 1 && !this.isSetActiveGameObjec) || (this.gameObject.activeSelf && this.isSetActiveGameObjec);
    }

    public void SetInteract(bool isInteract)
    {
        this.canvasGroup.interactable = isInteract;
    }
}
