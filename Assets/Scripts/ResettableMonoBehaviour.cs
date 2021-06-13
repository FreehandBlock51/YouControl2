using UnityEngine;

public class ResettableMonoBehaviour : MonoBehaviour
{
    Vector2 startPos;

    private void Awake()
    {
        startPos = GetComponent<Rigidbody2D>().position;
    }

    public void ResetPos() => GetComponent<Rigidbody2D>().MovePosition(startPos);
}