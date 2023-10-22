using UnityEngine;

public class MenuSceneCtrl : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlayBGM(AUDIO.BGM_EMOTION);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.HideAllPanel();
            UIManager.Instance.MainMenuPanel.Show(null);
        }
    }
}
