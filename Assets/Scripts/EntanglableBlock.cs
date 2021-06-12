using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntanglableBlock : MonoBehaviour
{
    public new Rigidbody2D rigidbody => GetComponent<Rigidbody2D>();
    static EntanglableBlock searching;
    static uint entangledBlocks = 0;

    public string ExitEntanglementButton = "Cancel";

    public EntanglableBlock entangledWith;
    public bool entangled;
    Vector2 movement;
    Vector2 prevPos;
    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        entangled = false;
        isMoving = false;
        StartCoroutine(ColorChange());
    }

    // Update is called once per frame
    void Update()
    {
        movement = rigidbody.position - prevPos;
        if (Input.GetButtonUp(ExitEntanglementButton))
        {
            searching = null;
            entangledBlocks = 0;
            entangledWith = null;
            entangled = false;
        }
        else if (entangled && entangledWith && isMoving)
        {
            entangledWith.rigidbody.MovePosition(entangledWith.rigidbody.position - movement);
        }
        prevPos = rigidbody.position;
    }

    IEnumerator ColorChange()
    {
        while (true)
        {
            if (entangled)
            {
                if (searching == this)
                {
                    GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                else
                {
                    GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnMouseUpAsButton()
    {
        if (Vector2.Distance(rigidbody.position, Player.Main.rigidbody.position) > 5)
        {
            return;
        }
        entangled = !entangled;
        if (entangled)
        {
            if (entangledBlocks >= 2)
            {
                entangled = false;
                return;
            }
            entangledBlocks++;
            if (!searching)
            {
                searching = this;
            }
            else
            {
                entangledWith = searching;
                searching = null;
                entangledWith.entangledWith = this;
            }
        }
        else
        {
            entangledBlocks--;
            if (searching == this)
            {
                searching = null;
            }
            else if (entangledWith)
            {
                entangledWith.entangledWith = null;
                searching = entangledWith;
                entangledWith = null;
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isMoving = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isMoving = false;
        }
    }
}
