using UnityEngine;

public class SellButton : MonoBehaviour
{
    public void OnButtonClicked()
    {
        AudioManager.Instance.PlaySfxAudio(Sfx.Button);
        NoticeManager.Instance.PlayNotice(NoticeType.Sell);
    }
}
