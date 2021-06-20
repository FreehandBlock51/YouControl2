using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MechanicBehaviour
{
    public Portal other;

    public bool tele;

    // Start is called before the first frame update
    void Start()
    {
        tele = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (tele && collision.gameObject.GetComponent<Player>())
        {
            other.tele = false;
            collision.gameObject.GetComponent<Rigidbody2D>().MovePosition(other.gameObject.GetComponent<Rigidbody2D>().position);
        }
        else if (tele)
        {
            other.tele = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            tele = true;
        }
    }
}
