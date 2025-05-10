using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NoticeType
{
    Start,
    Clear,
    Sell,
    NotJelatin,
    NotGold,
    NotNum,
}

public class NoticeManager : MonoBehaviour
{
    public static NoticeManager Instance { get; private set; }

    [SerializeField]
    private GameObject noticePanel;
    [SerializeField]
    private TextMeshProUGUI noticeText;
    [SerializeField]
    private Color positiveColor;
    [SerializeField]
    private Color negativeColor;

    [SerializeField]
    private string[] noticeTexts;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayNotice(NoticeType type)
    {
        CancelInvoke("Hide");

        noticePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -3f);
        noticeText.SetText(noticeTexts[(int)type]);
        noticePanel.GetComponent<Image>().color = (type.ToString().Substring(0, 3) == "Not" ? negativeColor : positiveColor);

        Invoke("Hide", 4f);
    }

    private void Hide()
    {
        noticePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 20f);
    }
}
