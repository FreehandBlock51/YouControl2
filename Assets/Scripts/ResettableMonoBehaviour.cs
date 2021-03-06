using UnityEngine;

public class ResettableMonoBehaviour : MechanicBehaviour
{
    Vector2 startPos;

    protected virtual void Start()
    {
        startPos = GetComponent<Rigidbody2D>().position;
    }

    public virtual void ResetPos() => GetComponent<Rigidbody2D>().MovePosition(startPos);
}