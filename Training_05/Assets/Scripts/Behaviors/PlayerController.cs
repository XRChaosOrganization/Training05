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
    

    //GameManager
    private bool isBallStopped = false;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
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
       
        if (Input.GetMouseButtonDown(0) && isBallStopped == true && _hit.collider == null && isDragging == false)
        {
            vcam.Follow = null;
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float timeSinceLastClick = Time.time - lastClickTime;
            if (timeSinceLastClick <= doubleClickTime)
            {
                vcam.Follow = this.transform;
                vcam.m_Lens.OrthographicSize = cameraOriginalSize;
            }
            lastClickTime = Time.time;
        }
        if (Input.GetMouseButton(0) && isBallStopped == true && _hit.collider == null && isDragging ==false)
        {
            {
                Vector3 delta = touchStart - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                vcam.transform.position += delta * cameraPreviewSpeed;
            }
        }
        if (Input.GetMouseButtonDown(0) && isBallStopped == false && canShoot == true)
        {
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
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Killzone"))
        {
            Debug.Log("C'est le deces");
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
            Debug.Log("GG !!");
            Destroy(this.gameObject);
        }
    }
    
   

    IEnumerator StopBall()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.5f);
        isBallStopped = true;
    }
}
