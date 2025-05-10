using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JellyPanel : MonoBehaviour
{
    [Header("# Jelly Unlock Field")]
    [SerializeField]
    private Image jellyUnlockImage;
    [SerializeField]
    private TextMeshProUGUI jellyNameText;
    [SerializeField]
    private TextMeshProUGUI jellyGoldText;

    [Header("# Jelly Lock Field")]
    [SerializeField]
    private GameObject jellyLockPanel;
    [SerializeField]
    private Image jellyLockImage;
    [SerializeField]
    private TextMeshProUGUI jellyJelatinText;

    [Header("# Jelly Panel Info")]
    [SerializeField]
    private TextMeshProUGUI curPageText;
    private int curPage = 0;
    
    private void Start()
    {
        ChangeJellyPanel();
    }

    public void PageUp()
    {
        if (curPage < 11)
        {
            curPage++;
            curPageText.SetText(string.Format("#{0:00}", curPage + 1));
            AudioManager.Instance.PlaySfxAudio(Sfx.Button);
        }
        else
        {
            AudioManager.Instance.PlaySfxAudio(Sfx.Fail);
        }

        ChangeJellyPanel();
    }

    public void PageDown()
    {
        if (curPage > 0)
        {
            curPage--;
            curPageText.SetText(string.Format("#{0:00}", curPage + 1));
            AudioManager.Instance.PlaySfxAudio(Sfx.Button);
        }
        else
        {
            AudioManager.Instance.PlaySfxAudio(Sfx.Fail);
        }

        ChangeJellyPanel();
    }

    private void ChangeJellyPanel()
    {
        if (GameManager.Instance.JellyUnlockList[curPage])
        {
            jellyUnlockImage.sprite = GameManager.Instance.JellySpriteList[curPage];
            jellyLockImage.SetNativeSize();
            jellyNameText.SetText(GameManager.Instance.JellyNameList[curPage]);
            jellyGoldText.SetText(string.Format("{0:n0}", GameManager.Instance.JellyGoldList[curPage]));
            jellyLockPanel.SetActive(false);
        }
        else
        {
            jellyLockImage.sprite = GameManager.Instance.JellySpriteList[curPage];
            jellyLockImage.SetNativeSize();
            jellyJelatinText.SetText(string.Format("{0:n0}", GameManager.Instance.JellyJelatinList[curPage]));
            jellyLockPanel.SetActive(true);
        }
    }

    public void UnlockJelly()
    {
        GameManager.Instance.UnlockJelly(curPage);
        ChangeJellyPanel();
    }

    public void BuyJelly()
    {
        GameManager.Instance.BuyJelly(curPage);
    }
}
