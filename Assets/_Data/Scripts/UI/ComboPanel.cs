using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboPanel : BaseUIElement
{
    [Header("COMBO PANEL")]
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;

    public Slider Slider { get => this.slider; set => this.slider = value; }
    public TMP_Text Text { get => this.text; set => this.text = value; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.slider == null)
            this.slider = GetComponentInChildren<Slider>();

        if (this.text == null)
            this.text = GetComponentInChildren<TMP_Text>();
    }
}
