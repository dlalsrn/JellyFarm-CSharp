using UnityEngine;

public class TouchState : IState
{
    private Jelly jelly;
    private float duration;

    public TouchState(Jelly jelly)
    {
        this.jelly = jelly;
    }

    public void Enter()
    {
        duration = Random.Range(2f, 4f);
        jelly.GetComponent<Animator>().SetTrigger("doTouch");
        jelly.GetExpByClicked();
        GameManager.Instance.AddJelatin((jelly.Id + 1) * jelly.Level * GameManager.Instance.ClickLevel);
        AudioManager.Instance.PlaySfxAudio(Sfx.Touch);
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
