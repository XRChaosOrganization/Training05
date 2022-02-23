using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float power;
    public float maxDrag;
    Vector2 dragDir;
    float dragValue;
    public Rigidbody2D rb;
    public LineRenderer lr;

    //GameManager
    private bool isBallStopped = false;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isBallStopped == false)
        {
            isBallStopped = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnMouseDrag()
    {
        if (isBallStopped == true)
        {
            lr.enabled = true;
            lr.SetPosition(1, transform.position);
            dragDir = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragValue = dragDir.magnitude;
            Vector2 endDragPoint;
            
            if (dragValue <= maxDrag)
            {
                endDragPoint = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                endDragPoint = (Vector2)transform.position - dragDir.normalized * maxDrag;
            }
            lr.SetPosition(0, endDragPoint);
        }
    }
    private void OnMouseUp()
    {
        if (isBallStopped == true)
        {
            lr.enabled = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(Vector2.ClampMagnitude( dragDir,maxDrag) * power, ForceMode2D.Impulse);
            isBallStopped = false;
        }
    }
}
