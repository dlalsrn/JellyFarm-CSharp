using UnityEngine;

public class OptionButton : MonoBehaviour
{
    [SerializeField]
    private GameObject optionPanel;

    [SerializeField]
    private bool isClicked = false;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isClicked && GameManager.Instance.IsLive)
            {
                Show();
                AudioManager.Instance.PlaySfxAudio(Sfx.PauseIn);
            }
            else if (isClicked)
            {
                Hide();
                AudioManager.Instance.PlaySfxAudio(Sfx.PauseOut);
            }
        }
    }

    public void Show()
    {
        optionPanel.SetActive(true);
        Time.timeScale = 0f;
        isClicked = true;
        GameManager.Instance.SetIsLive(false);
    }

    public void Hide()
    {
        if (isClicked)
        {
            optionPanel.SetActive(false);
            Time.timeScale = 1f;
            isClicked = false;
            GameManager.Instance.SetIsLive(true);
        }
    }

    public void OnResumeButtonClicked()
    {
        Hide();
        AudioManager.Instance.PlaySfxAudio(Sfx.Button);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
        AudioManager.Instance.PlaySfxAudio(Sfx.Button);
    }
}
