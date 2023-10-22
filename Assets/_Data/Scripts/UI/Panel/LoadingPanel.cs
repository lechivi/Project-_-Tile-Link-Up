using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingPanel : BaseUIElement
{
    [Header("LOADING PANEL")]
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TMP_Text completeText;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.loadingSlider == null)
            this.loadingSlider = GetComponentInChildren<Slider>();

        if (this.completeText == null)
            this.completeText = GetComponentInChildren<TMP_Text>();
    }

    public IEnumerator LoadScene(int sceneIndex)
    {
        this.completeText.enabled = false;

        this.Show(null);
        this.loadingSlider.value = 0;

        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            float percent = asyncOperation.progress;
            this.loadingSlider.value = (int)(percent * this.loadingSlider.maxValue);

            if (percent >= 0.9f)
            {
                this.loadingSlider.value = this.loadingSlider.maxValue;
                this.completeText.enabled = true;
                if (Input.anyKeyDown)
                {
                    asyncOperation.allowSceneActivation = true;

                    yield return null;
                    this.Hide();
                }
            }
            yield return null;

        }
    }
}
