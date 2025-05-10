using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LeftButton : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private LeftButton otherButton;
    [SerializeField]
    private Sprite activeImage;
    [SerializeField]
    private Sprite InActiveImage;
    
    [SerializeField]
    private bool isClicked = false; // 이미 활성화되어 있는지

    private void LateUpdate()
    {
        if (isClicked && Input.GetKeyDown(KeyCode.Escape))
        {
            Invoke("Hide", 0.1f);
        }    
    }

    public void OnButtonClicked()
    {
        if (isClicked)
        {
            isClicked = false;
            Hide();
            AudioManager.Instance.PlaySfxAudio(Sfx.Button);
        }
        else
        {
            isClicked = true;
            Show();
            AudioManager.Instance.PlaySfxAudio(Sfx.Button);
        }
    }

    public void Show()
    {
        // Panel을 띄울 상황이 아니리면, 다른 Panel이 활성화되어 있다면 해당 Panel Hide
        if (!GameManager.Instance.IsLive)
        {
            otherButton.Hide();
        }

        isClicked = true;
        GameManager.Instance.SetIsLive(false);
        GetComponent<Image>().sprite = activeImage;
        panel.GetComponent<Animator>().SetTrigger("doShow");
    }

    public void Hide()
    {
        isClicked = false;
        GameManager.Instance.SetIsLive(true);
        GetComponent<Image>().sprite = InActiveImage;
        panel.GetComponent<Animator>().SetTrigger("doHide");
    }
}
