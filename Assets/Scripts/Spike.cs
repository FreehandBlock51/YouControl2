using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : ResettableMonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (reset)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().position = prevPos;
            reset = false;
        }
    }

    Vector2 prevPos;
    bool reset = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            collision.gameObject.GetComponent<Player>().Respawn();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            prevPos = GetComponent<Rigidbody2D>().position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            reset = true;
        }
    }
}
