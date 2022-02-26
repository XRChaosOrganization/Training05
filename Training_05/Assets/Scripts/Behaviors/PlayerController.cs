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
    public LineRenderer aimLr;
    public int aimPrevNumberOfPoints;
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
        
        RaycastHit2D _hit = Physics2D.Raycast (vcam.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0) && isBallStopped == true && _hit.collider == null && isDragging == false)
        {
             touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0) && isBallStopped == true && _hit.collider == null && isDragging ==false)
        {
            
            Vector3 delta = touchStart - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
            transposer.m_TrackedObjectOffset += delta * cameraPreviewSpeed;
            
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
            DisplayTrajectory();
            aimLr.positionCount = aimPrevNumberOfPoints;

        }
    }
    
    private void OnMouseUp()
    {
        if (isBallStopped == true)
        {
            isDragging = false;
            lr.enabled = false;
            aimLr.enabled = false;
            aimLr.positionCount = 0;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(Vector2.ClampMagnitude( dragDir,maxDrag) * power, ForceMode2D.Impulse);
            isBallStopped = false;
        }
    }
    void DisplayTrajectory()
    {
        
        Vector3[] points = new Vector3[aimPrevNumberOfPoints];
        for (int i = 0; i < aimPrevNumberOfPoints; i++)
        {
            aimLr.positionCount++;
            points[i] = (Vector2)transform.position + Vector2.ClampMagnitude(dragDir, maxDrag) * power * 0.1f * i + Physics2D.gravity * 0.1f * i *0.1f* i / 2;
        }
        aimLr.SetPositions(points);
        aimLr.enabled = true;
    }
    IEnumerator StopBall()
    {
        yield return new WaitForSeconds(0.5f);
        isBallStopped = true;
    }
}
