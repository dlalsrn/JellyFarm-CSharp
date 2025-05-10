using UnityEngine;

public class Reposition : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (transform.position.x <= -8f)
        {
            transform.position += Vector3.right * 16f;
        }
    }
}
