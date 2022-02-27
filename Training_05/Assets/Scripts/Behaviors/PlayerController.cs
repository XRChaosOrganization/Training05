using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float cameraPreviewSpeed;
    public float power;
    public float maxDrag;
    bool isDragging;
    Vector2 dragDir;
    float dragValue;
    public Rigidbody2D rb;
    public LineRenderer lr;
    private Vector2 touchStart;
    public CinemachineVirtualCamera vcam;

    //GameManager
    private bool isBallStopped = false;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        vcam.transform.position = Camera.main.transform.position;
        RaycastHit2D _hit = Physics2D.Raycast (vcam.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0) && isBallStopped == true && _hit.collider == null && isDragging == false)
        {
             vcam.Follow = null;
             touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0) && isBallStopped == true && _hit.collider == null && isDragging ==false)
        {
            
            Vector3 delta = touchStart - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vcam.transform.position += delta * cameraPreviewSpeed;
            
        }
        if (Input.GetMouseButtonDown(0) && isBallStopped == false)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(StopBall());
        }
    }

    private void OnMouseDrag()
    {
        isDragging = true;
        if (isBallStopped == true)
        {
            vcam.Follow = this.transform;
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
            isDragging = false;
            lr.enabled = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(Vector2.ClampMagnitude( dragDir,maxDrag) * power, ForceMode2D.Impulse);
            isBallStopped = false;
        }
    }
   
    IEnumerator StopBall()
    {
        yield return new WaitForSeconds(0.5f);
        isBallStopped = true;
    }
}
