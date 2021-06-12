using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// The player instance in the level.
    /// There should only be at most one <c>Player</c> Component in each Scene
    /// </summary>
    public static Player Main => GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    public Animator Animator => gameObject.GetComponent<Animator>();

    public string HorizontalAxisName = "Horizontal";
    public string VerticalAxisName = "Vertical";
    public string AttackButtonName = "Jump";
    [Min(1 / float.MaxValue)]
    public float speed = 0.1f;

    private Vector2 CurrentMovement = Vector2.zero;
    Vector2 spawn;

    public new Rigidbody2D rigidbody => GetComponent<Rigidbody2D>();

    // Start is called before the first frame update
    void Start()
    {
        spawn = rigidbody.position;
    }

    Vector2 movement;


    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.MovePosition(rigidbody.position + (new Vector2(Input.GetAxis(HorizontalAxisName), Input.GetAxis(VerticalAxisName)) * speed));
        
        Animate(movement);
    }

    void Animate(Vector2 movement)
    {
        Animator.SetFloat("X", movement.x);
        Animator.SetFloat("Y", movement.y);
    }
}
