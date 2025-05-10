using UnityEngine;

public class IdleState : IState
{
    private Jelly jelly;
    private float duration;

    public IdleState(Jelly jelly)
    {
        this.jelly = jelly;
    }

    public void Enter()
    {
        if (GameManager.Instance.CheckBorder(jelly.transform.position))
        {
            jelly.transform.position = GameManager.Instance.SetPosition();
        }
        
        duration = Random.Range(2f, 4f);
        jelly.GetComponent<Animator>().SetBool("isWalk", false);
    }

    public void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            Vector2 randomVec = new Vector2(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f)).normalized;
            jelly.SetMoveSpeed(randomVec);
            jelly.ChangeState(new WalkState(jelly));
        }
    }

    public void Exit()
    {
    }
}
