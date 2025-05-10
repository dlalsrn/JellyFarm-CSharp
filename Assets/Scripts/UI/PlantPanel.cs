using TMPro;
using UnityEngine;

public class PlantPanel : MonoBehaviour
{
    [Header("# Num Plant")]
    [SerializeField]
    private TextMeshProUGUI numSubText;
    [SerializeField]
    private TextMeshProUGUI numGoldText;
    [SerializeField]
    private GameObject numButton;

    [Header("# Click Plant")]
    [SerializeField]
    private TextMeshProUGUI clickSubText;
    [SerializeField]
    private TextMeshProUGUI clickGoldText;
    [SerializeField]
    private GameObject clickButton;

    private void Start()
    {
        UpdateNumPlant();
        UpdateClickPlant();
    }

    public void UpgradeNumLevel()
    {
        if (GameManager.Instance.NumLevel < 5)
        {
            GameManager.Instance.IncreaseNumLevel();
            UpdateNumPlant();
        }
    }

    private void UpdateNumPlant()
    {
        int curLevel = GameManager.Instance.NumLevel;

        if (curLevel == 5)
        {
            numButton.SetActive(false);
        }

        numSubText.SetText($"젤리 수용량 {2 * curLevel}");
        numGoldText.SetText(string.Format("{0:n0}", GameManager.Instance.NumGoldList[curLevel - 1]));
    }

    public void UpgradeClickLevel()
    {
        if (GameManager.Instance.ClickLevel < 5)
        {
            GameManager.Instance.IncreaseClickLevel();
            UpdateClickPlant();
        }
    }

    private void UpdateClickPlant()
    {
        int curLevel = GameManager.Instance.ClickLevel;

        if (curLevel == 5)
        {
            clickButton.SetActive(false);
        }

        clickSubText.SetText($"젤리 생산량 x {curLevel}");
        clickGoldText.SetText(string.Format("{0:n0}", GameManager.Instance.ClickGoldList[curLevel - 1]));
    }
}
