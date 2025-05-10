using UnityEngine;

public class PickState : IState
{
    private Jelly jelly;

    public PickState(Jelly jelly)
    {
        this.jelly = jelly;
    }

    public void Enter()
    {
    }

    public void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = mousePos.y;
        jelly.transform.position = mousePos;
        
    }

    public void Exit()
    {
    }
}
