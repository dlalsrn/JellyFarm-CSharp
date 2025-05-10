using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("# Left Text")]
    [SerializeField]
    private TextMeshProUGUI jelatinText;

    [Header("# Right Text")]
    [SerializeField]
    private TextMeshProUGUI goldText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdateJelatinText(int jelatin)
    {
        jelatinText.SetText(string.Format("{0:n0}", jelatin));
    }

    public void UpdateGoldText(int gold)
    {
        goldText.SetText(string.Format("{0:n0}", gold));
    }
}
