using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float cameraPreviewSpeed;
    public float cameraZoomingSpeed;
    public float power;
    public float maxDrag;
    public float forceStopSensibility;
    bool isDragging;
    bool canShoot = true;
    Vector2 dragDir;
    float dragValue;
    public Rigidbody2D rb;
    public LineRenderer lr;
    private Vector2 touchStart;
    public CinemachineVirtualCamera vcam;
    float doubleClickTime = .2f;
    float lastClickTime;
    int cameraOriginalSize = 17;
    public bool isBallStopped = false;
    float timeOnGround;

    
    private void Start()
    {
        if (UIManager.current != null)
            UIManager.current.menuGizmo.Strokes = 0;
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        vcam = GameObject.FindWithTag("Vcam").GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = this.transform;
        vcam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<PolygonCollider2D>();
        StartCoroutine(StopBall());
    }

    private void Update()
    {
        vcam.transform.position = Camera.main.transform.position;
        RaycastHit2D _hit = Physics2D.Raycast (vcam.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
      
        
        if (Input.mouseScrollDelta.y != 0 && isBallStopped)
        {
            vcam.m_Lens.OrthographicSize = Mathf.Clamp(vcam.m_Lens.OrthographicSize - Input.mouseScrollDelta.y *cameraZoomingSpeed,6,40) ;
        }
        if (Input.GetMouseButtonDown(0) )
        {
            if (isBallStopped && _hit.collider == null && !isDragging)
            {
                vcam.Follow = null;
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                DoubleClickCheck(Time.time - lastClickTime);
            }
            else if (!isBallStopped && canShoot)
            {
                StartCoroutine(StopBall());
            }
        }
        if (Input.GetMouseButton(0) && isBallStopped && _hit.collider == null && !isDragging)
        {
            DragCamera(touchStart - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
    private void OnMouseDrag()
    {
        isDragging = true;
        if (isBallStopped)
        {
            OnDrag();
        }
    }
    
    private void OnMouseUp()
    {
        if (isBallStopped)
        {
            Shoot();
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Killzone"))
        {
            //DEFAITE
            UIManager.current.SwitchToMenu(2);
            Destroy(this.gameObject);
        }
        if (col.CompareTag("NoDragNShoot"))
        {
            canShoot = false;
        }
        if (col.CompareTag("GravityZone"))
        {
            rb.gravityScale = -rb.gravityScale;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("NoDragNShoot"))
        {
            canShoot = true;
        }
        if (col.CompareTag("GravityZone"))
        {
            rb.gravityScale = - rb.gravityScale;
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Hole"))
        {
            //VICTOIRE
            Destroy(this.gameObject);
            UIManager.current.SwitchToMenu(2);
        }
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.CompareTag("Obstacle"))
        {
            timeOnGround +=  Time.deltaTime;
            if (timeOnGround > 2f && !isBallStopped)
            {
              if (rb.velocity.x < forceStopSensibility || rb.velocity.x > -forceStopSensibility)
              {
                  StartCoroutine(StopBall());
              }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Obstacle"))
        {
            timeOnGround = 0;
        }
       
    }

    IEnumerator StopBall()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.5f);
        isBallStopped = true;
    }
    void DoubleClickCheck(float _lastClickTime)
    {
        if (_lastClickTime <= doubleClickTime)
        {
            vcam.Follow = this.transform;
            vcam.m_Lens.OrthographicSize = cameraOriginalSize;
        }
        lastClickTime = Time.time;
    }
    void DragCamera(Vector3 _delta)
    {
        vcam.transform.position += _delta * cameraPreviewSpeed;
    }
    void OnDrag()
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
    void Shoot()
    {
        timeOnGround = 0;
        isDragging = false;
        lr.enabled = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(Vector2.ClampMagnitude(dragDir, maxDrag) * power, ForceMode2D.Impulse);
        isBallStopped = false;
        vcam.Follow = this.transform;
        if(UIManager.current != null)
            UIManager.current.menuGizmo.Strokes++;
    }
    
}
