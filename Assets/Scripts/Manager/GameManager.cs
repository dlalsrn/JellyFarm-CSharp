using UnityEngine;
using System.Collections;
using UnityEngine.Jobs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("# Jelly Prefab")]
    [SerializeField]
    private Jelly jellyPrefab;
    
    [Header("# Game Info")]
    [SerializeField]
    private int jelatin;
    [SerializeField]
    private int gold;
    [SerializeField]
    private bool isSell; // 판매 가능 여부
    public bool IsSell => isSell;
    private bool isLive = true;
    public bool IsLive => isLive; // 특정 Panel이 활성화되어 있는 상태인지
    [SerializeField]
    private float sfxVolume;
    public float SfxVolume => sfxVolume;
    [SerializeField]
    private float bgmVolume;
    public float BgmVolume => bgmVolume;
    [SerializeField]
    private GameObject clearMedal;
    private bool isClear; // 게임 클리어 여부


    [Header("# Return Position")]
    [SerializeField]
    private Vector3[] returnPos;
    private Vector2 topLeft = new Vector3(-5.3f, 0.9f); // Jelly가 넘어가면 안 되는 좌표 경계값
    private Vector2 bottomRight = new Vector3(5.3f, -2f); // Jelly가 넘어가면 안 되는 좌표 경계값

    [Header("# Jelly Info List")]
    [SerializeField]
    private int[] jellyGoldList; // Jelly의 구매, 판매 비용
    public int[] JellyGoldList => jellyGoldList;
    [SerializeField]
    private int[] jellyJelatinList; // Jelly의 해금 비용
    public int[] JellyJelatinList => jellyJelatinList;
    [SerializeField]
    private string[] jellyNameList;
    public string[] JellyNameList => jellyNameList;
    [SerializeField]
    private Sprite[] jellySpriteList;
    public Sprite[] JellySpriteList => jellySpriteList;
    [SerializeField]
    private bool[] jellyUnlockList;
    public bool[] JellyUnlockList => jellyUnlockList;
    [SerializeField]
    private List<GameObject> jellyList = new List<GameObject>(); // 현재 게임 상에 존재하는 Jelly Object List

    [Header("# Plant Info List")]
    [SerializeField]
    private int[] numGoldList;
    public int[] NumGoldList => numGoldList;
    [SerializeField]
    private int[] clickGoldList;
    public int[] ClickGoldList => clickGoldList;
    [SerializeField]
    private int numLevel;
    public int NumLevel => numLevel;
    [SerializeField]
    private int clickLevel;
    public int ClickLevel => clickLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Init();
    }

    private void Start()
    {
        StartCoroutine(AutoSaveData());
        StartCoroutine(GetAutoJelatin());

        if (isClear)
        {
            GameClear();
        }
        else
        {
            NoticeManager.Instance.PlayNotice(NoticeType.Start);
        }
    }

    IEnumerator AutoSaveData()
    {
        yield return null;

        while (true)
        {
            yield return new WaitForSeconds(3f);

            PlayerPrefs.SetInt("Jelatin", jelatin);
            PlayerPrefs.SetInt("Gold", gold);
            PlayerPrefs.SetInt("NumLevel", numLevel);
            PlayerPrefs.SetInt("ClickLevel", clickLevel);
            PlayerPrefs.SetFloat("BgmVolume", bgmVolume);
            PlayerPrefs.SetFloat("SfxVolume", sfxVolume);

            for (int index = 0; index < jellyUnlockList.Length; index++)
            {
                PlayerPrefs.SetInt($"Jelly{index}", jellyUnlockList[index] ? 1 : 0);
            }

            SavedJelly();
        }
    }

    IEnumerator GetAutoJelatin()
    {
        yield return null;

        while (true)
        {
            yield return new WaitForSeconds(3f);

            foreach (GameObject jellyObj in jellyList)
            {
                Jelly jelly = jellyObj.GetComponent<Jelly>();
                AddJelatin((jelly.Id + 1) * jelly.Level);
            }

            UIManager.Instance.UpdateJelatinText(jelatin);
        }
    }

    private void Init()
    {
        jelatin = PlayerPrefs.GetInt("Jelatin", 100);
        gold = PlayerPrefs.GetInt("Gold", 200);
        numLevel = PlayerPrefs.GetInt("NumLevel", 1);
        clickLevel = PlayerPrefs.GetInt("ClickLevel", 1);
        bgmVolume = PlayerPrefs.GetFloat("BgmVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.5f);
        isClear = PlayerPrefs.GetInt("Clear") == 1 ? true : false;

        for (int index = 0; index < jellyUnlockList.Length; index++)
        {
            jellyUnlockList[index] = PlayerPrefs.GetInt($"Jelly{index}") == 1 ? true : false;
        }

        LoadedJelly();

        UIManager.Instance.UpdateJelatinText(jelatin);
        UIManager.Instance.UpdateGoldText(gold);
    }

    public void AddJelatin(int jelatin)
    {
        this.jelatin += jelatin;
        this.jelatin = Mathf.Min(this.jelatin, 999999999);
        UIManager.Instance.UpdateJelatinText(this.jelatin);
    }

    public void AddGold(int gold)
    {
        this.gold += gold;
        this.gold = Mathf.Min(this.gold, 999999999);
        UIManager.Instance.UpdateGoldText(this.gold);
    }

    public bool CheckBorder(Vector3 jellyPos)
    {
        bool isBorder = false;

        if (jellyPos.x < topLeft.x || jellyPos.x > bottomRight.x)
        {
            isBorder = true;
        }
        else if (jellyPos.y > topLeft.y || jellyPos.y < bottomRight.y)
        {
            isBorder = true;
        }

        return isBorder;
    }

    public Vector3 SetPosition()
    {
        int index = Random.Range(0, returnPos.Length);
        return returnPos[index];
    }

    public void SetIsSell(bool isSell)
    {
        this.isSell = isSell;
    }

    public void SetIsLive(bool isLive)
    {
        this.isLive = isLive;
    }

    public void UnlockJelly(int index)
    {
        if (jellyJelatinList[index] <= jelatin)
        {
            jelatin -= jellyJelatinList[index];
            jellyUnlockList[index] = true;
            UIManager.Instance.UpdateJelatinText(jelatin);
            AudioManager.Instance.PlaySfxAudio(Sfx.Unlock);

            if (CheckUnlockJelly())
            {
                PlayerPrefs.SetInt("Clear", 1);
                GameClear();
            }
        }
        else
        {
            AudioManager.Instance.PlaySfxAudio(Sfx.Fail);
            NoticeManager.Instance.PlayNotice(NoticeType.NotJelatin);
        }
    }

    public void BuyJelly(int index)
    {
        if (jellyGoldList[index] <= gold)
        {
            if (jellyList.Count < numLevel * 2)
            {
                gold -= jellyGoldList[index];
                UIManager.Instance.UpdateGoldText(gold);
                MakeJelly(index, 1, 0f);
                AudioManager.Instance.PlaySfxAudio(Sfx.Buy);
            }
            else
            {
                NoticeManager.Instance.PlayNotice(NoticeType.NotNum);
            }
        }
        else
        {
            AudioManager.Instance.PlaySfxAudio(Sfx.Fail);
            NoticeManager.Instance.PlayNotice(NoticeType.NotGold);
        }
    }

    public void IncreaseNumLevel()
    {
        if (numLevel < 5 && gold >= numGoldList[numLevel - 1])
        {
            gold -= numGoldList[numLevel - 1];
            UIManager.Instance.UpdateGoldText(gold);
            numLevel++;
            AudioManager.Instance.PlaySfxAudio(Sfx.Buy);
        }
        else
        {
            AudioManager.Instance.PlaySfxAudio(Sfx.Fail);
            NoticeManager.Instance.PlayNotice(NoticeType.NotGold);
        }
    }

    public void IncreaseClickLevel()
    {
        if (clickLevel < 5 && gold >= clickGoldList[clickLevel - 1])
        {
            gold -= clickGoldList[clickLevel - 1];
            UIManager.Instance.UpdateGoldText(gold);
            clickLevel++;
            AudioManager.Instance.PlaySfxAudio(Sfx.Buy);
        }
        else
        {
            AudioManager.Instance.PlaySfxAudio(Sfx.Fail);
            NoticeManager.Instance.PlayNotice(NoticeType.NotGold);
        }
    }

    private void MakeJelly(int id, int level, float exp)
    {
        Vector3 SpawnPos = SetPosition();
        Jelly jelly = Instantiate<Jelly>(jellyPrefab, SpawnPos, Quaternion.identity);

        Sprite sprite = jellySpriteList[id];

        jelly.SetJellyInfo(sprite, id, level, exp);

        AddJelly(jelly.gameObject);
    }

    public void AddJelly(GameObject jelly)
    {
        jellyList.Add(jelly);
    }

    public void RemoveJelly(GameObject jelly)
    {
        jellyList.Remove(jelly);
    }

    public void ChangeBgmVolume(float vol)
    {
        bgmVolume = vol;
        AudioManager.Instance.ChangeBgmVolume(bgmVolume);
    }

    public void ChangeSfxVolume(float vol)
    {
        sfxVolume = vol;
        AudioManager.Instance.ChangeSfxVolume(sfxVolume);
    }

    // JellyList에 있는 Jelly Object의 id, level, exp를 JellyData Class로 만들어 저장
    private void SavedJelly()
    {
        List <JellyData> saveList = new List<JellyData>();

        foreach (GameObject jellyObj in jellyList)
        {
            Jelly jelly = jellyObj.GetComponent<Jelly>();
            if (jelly != null)
            {
                saveList.Add(new JellyData(jelly.Id, jelly.Level, jelly.Exp));
            }
        }

        JellyDataList dataWrapper = new JellyDataList { jellyDataList = saveList };
        string json = JsonUtility.ToJson(dataWrapper);
        PlayerPrefs.SetString("SavedJelly", json);
        PlayerPrefs.Save();
    }

    // id, level, exp를 기반으로 Setpoistion으로 갖고온 Position에 Jelly 생성
    private void LoadedJelly()
    {
        string json = PlayerPrefs.GetString("SavedJelly", "");

        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        JellyDataList dataWrapper = JsonUtility.FromJson<JellyDataList>(json);

        foreach (JellyData data in dataWrapper.jellyDataList)
        {
            MakeJelly(data.Id, data.Level, data.Exp);
        }
    }

    // 모든 Jelly가 Unlock 됐는지 Check
    private bool CheckUnlockJelly()
    {
        bool result = true;

        foreach (bool unlock in jellyUnlockList)
        {
            result &= unlock;
        }

        return result;
    }

    private void GameClear()
    {
        clearMedal.SetActive(true);
        NoticeManager.Instance.PlayNotice(NoticeType.Clear);
        AudioManager.Instance.PlaySfxAudio(Sfx.Clear);
    }
}
