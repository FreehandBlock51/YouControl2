using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    public Transform MovementAnchor;

    private void Start()
    {
        if (!MovementAnchor)
        {
            MovementAnchor = GetComponentInChildren<Transform>();
        }

        MovementAnchor.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Input.mousePosition;
            MovementAnchor.position = Camera.main.ScreenToWorldPoint(dragOrigin) + (Vector3.forward * 6);
            MovementAnchor.gameObject.SetActive(true);
            return;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            MovementAnchor.gameObject.SetActive(false);
            return;
        }

        if (!Input.GetMouseButton(2)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed);
        transform.Translate(move, Space.World);
    }


}