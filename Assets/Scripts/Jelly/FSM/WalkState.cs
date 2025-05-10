using UnityEngine;

public class WalkState : IState
{
    private Jelly jelly;
    private float duration;
    private Vector3 direction;

    public WalkState(Jelly jelly)
    {
        this.jelly = jelly;
    }

    public void Enter()
    {
        duration = Random.Range(1.5f, 2.5f);
        direction = new Vector3(jelly.MoveSpeed.x, jelly.MoveSpeed.y, jelly.MoveSpeed.y);
        jelly.GetComponent<Animator>().SetBool("isWalk", true);
    }

    public void Update()
    {
        duration -= Time.deltaTime;

        // Jelly Translate
        jelly.GetComponent<SpriteRenderer>().flipX = direction.x < 0f;
        jelly.transform.Translate(direction * Time.deltaTime);

        // 경계 좌표 Check, true면 Jelly가 경계를 넘어가려고 한다는 의미
        // true일 경우 맵 내부의 임의의 좌표로 이동 시킴
        if (GameManager.Instance.CheckBorder(jelly.transform.position))
        {
            Vector3 returnVec = (GameManager.Instance.SetPosition() - jelly.transform.position).normalized;
            jelly.SetMoveSpeed(returnVec * 0.5f);
            jelly.ChangeState(new WalkState(jelly));
        }

        if (duration <= 0f)
        {
            jelly.ChangeState(new IdleState(jelly));
        }
    }

    public void Exit()
    {
        jelly.GetComponent<Animator>().SetBool("isWalk", false);
    }
}
