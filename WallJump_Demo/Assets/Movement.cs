using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float dir;
    public Transform cam;
    public float cameraMovingTime;
    public bool isClicked;
    [SerializeField] private float jumpForce;
    private Collision col;
    [SerializeField] private float wallSlideSpeed;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collision>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!col.onWall)
        {
            rb.velocity = (new Vector2(dir * speed, rb.velocity.y));
        }
        else
        {
            rb.velocity = (new Vector2(dir * speed, -1f * wallSlideSpeed));
        }
        if (Input.GetButtonDown("ChangeDir"))
        {
            ChangeDirection();
        }
    }

    private void OnMouseDown()
    {
        isClicked = true;
    }

    private void OnMouseUp()
    {
        if (isClicked)
        {
            Vector3 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(camPos + ", " + this.transform.position);
            Vector3 tempVector = new Vector3(camPos.x - this.transform.position.x, camPos.y - this.transform.position.y, 0);
            Vector2 jumpingDir = new Vector2 (tempVector.normalized.x, tempVector.normalized.y);
            //Debug.Log(jumpingDir);
            if (jumpingDir.x * dir <0)
            {
                StartCoroutine(ChangeCameraPosition());
            }
            dir = jumpingDir.x;
            rb.velocity += jumpForce * jumpingDir;
            isClicked = false;
        }
    }

    void ChangeDirection ()
    {
        StartCoroutine(ChangeCameraPosition());
        dir *= -1f;
    }

    IEnumerator ChangeCameraPosition ()
    {
        Vector3 startPosition = cam.localPosition;
        Vector3 destination = new Vector3(-1f * cam.localPosition.x, 2f, -10);
        float elapsedTime = 0f;
        float t;
        while(true)
        {
            if (cam.localPosition == destination)
            {
                break;
            }
            elapsedTime += Time.deltaTime;
            t = Mathf.Clamp(elapsedTime / cameraMovingTime, 0f, 1f);
            //t = Mathf.Sin(elapsedTime / cameraMovingTime * Mathf.PI * 0.5f);
            cam.localPosition = Vector3.Lerp(startPosition, destination, t);
            yield return null;
        }
    }
}
