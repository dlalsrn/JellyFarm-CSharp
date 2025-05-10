using UnityEngine;

public class Jelly : MonoBehaviour
{
    private Animator animator;

    private IState currentState;

    private Vector2 moveSpeed;
    public Vector2 MoveSpeed => moveSpeed;

    private float pickTime; // Drag 하고 있는 시간

    [Header("# Jelly Info")]
    [SerializeField]
    private int id;
    public int Id => id;
    [SerializeField]
    private int level;
    public int Level => level;
    [SerializeField]
    private float exp;
    public float Exp => exp;

    [Header("# Jelly Animation Controller")]
    [SerializeField]
    private RuntimeAnimatorController[] runtimeAnicon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        GetExpByTime();
        currentState?.Update();
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void SetMoveSpeed(Vector2 dirVec)
    {
        moveSpeed = dirVec;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.IsLive)
        {
            pickTime = 0f;
            ChangeState(new TouchState(this));
        }
    }

    private void OnMouseUp()
    {
        // 판매 가능, 판매 Button에 Drop 했을 경우
        if (GameManager.Instance.IsSell)
        {
            //int gold = GameManager.Instance.GetJellyGold(id) * level; 
            int gold = GameManager.Instance.JellyGoldList[id] * level; // Jelly의 판매 가격 = 구매 가격 x 현재 Level
            GameManager.Instance.AddGold(gold);
            GameManager.Instance.RemoveJelly(gameObject);
            Destroy(gameObject);
            AudioManager.Instance.PlaySfxAudio(Sfx.Sell);
        }
        // 판매 불가, 판매 Button이 아닌 곳에 Drop 했을 경우
        else
        {
            ChangeState(new IdleState(this));
        }
    }

    private void OnMouseDrag()
    {
        pickTime += Time.deltaTime;
        if (pickTime >= 0.2f)
        {
            ChangeState(new PickState(this));
        }    
    }

    // 자동으로 얻는 경험치
    private void GetExpByTime()
    {
        if (level != 3)
        {
            exp += Time.deltaTime;
            if (exp >= level * 50)
            {
                animator.runtimeAnimatorController = runtimeAnicon[level];
                level++;
                AudioManager.Instance.PlaySfxAudio(Sfx.Grow);
            }
        }

    }

    // 클릭에 의해 얻는 경험치
    public void GetExpByClicked()
    {
        if (level != 3)
        {
            exp += 1f;
        }
    }

    public void SetJellyInfo(Sprite sprite, int id, int level, float exp)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        animator.runtimeAnimatorController = runtimeAnicon[level - 1];
        name = $"Jelly {id}";

        this.id = id;
        this.level = level;
        this.exp = exp;
    }
}
