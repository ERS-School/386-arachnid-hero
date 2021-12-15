using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float Sensitivity = 2;

    public float movementSpeed = 6;
    public float SprintBoost = 3;
    public float JumpPower = 8;
    public float AirSpeed = 5;
    bool mouseDown;
    bool collided;
    Rigidbody rb;


    public Image[] crosshairs = new Image[2];
    Vector2 rotation = new Vector2(360,0);
    Camera playercam;
    public LayerMask layermask;

    private void Start()
    {
        playercam = transform.GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
    }

    void GrappleHook()
    {
        RaycastHit hit;
        Ray ray = playercam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out hit, GetComponent<GrappleHook>().GrappleRange, layermask))
        {
            crosshairs[1].enabled = true;

            if (Input.GetMouseButtonDown(1))
            {
                GetComponent<GrappleHook>().GrappleTo(hit); //Tells the grapple hook script where there was a hit
                GetComponent<SphereCollider>().material.dynamicFriction = 0f;
            }
        }
        else
        {
            crosshairs[1].enabled = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            GetComponent<GrappleHook>().UnGrapple(); //Tells grapple hook script that have now let go
            GetComponent<SphereCollider>().material.dynamicFriction = 1f;
        }
    }

    void playerMovement()
    {
        int usermoving = 0;
        Vector3 movedir = Vector3.zero;
        float temp_movS = movementSpeed;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (rb.velocity.sqrMagnitude > 16)
            {
                GetComponent<SphereCollider>().material.dynamicFriction = 0.1f;
            }
            else
            {
                GetComponent<SphereCollider>().material.dynamicFriction = 1f;
                movementSpeed *= 0.5f;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed += SprintBoost;
        }

        if (Input.GetKey(KeyCode.W))
        {

            Vector3 forwards = new Vector3(transform.forward.x, 0, transform.forward.z);
            if (collided)
            {
                movedir += forwards * movementSpeed;
                usermoving += 1;
            }
            else
            {
                rb.AddForce(forwards * AirSpeed);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 forwards = new Vector3(transform.forward.x, 0, transform.forward.z);
            if (collided)
            {
                movedir -= forwards * movementSpeed;
                usermoving += 1;
            }
            else
            {
                rb.AddForce(-forwards * AirSpeed);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (collided)
            {
                movedir -= transform.right * movementSpeed;
                usermoving += 1;
            }
            else
            {
                rb.AddForce(-transform.right * AirSpeed);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (collided)
            {
                movedir += transform.right * movementSpeed;
                usermoving += 1;
            }
            else
            {
                rb.AddForce(transform.right * AirSpeed);
            }
        }
        if (Input.GetKey(KeyCode.Space) && collided)
        {
            movedir += Vector3.up * JumpPower;
            usermoving = 1;
        }

        if (usermoving > 0)
        {
            rb.velocity = movedir / ((usermoving > 1) ? 1.414213562f : 1);
        }

        movementSpeed = temp_movS;
        CheckGrounded();
    }

    void CheckGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.6f, layermask))
        {
            collided = true;
            rb.drag = 1;
        }
        else
        {
            collided = false;
            rb.drag = 0.2f;
        }
    }



    void MenuControls()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = (CursorLockMode)(((int)Cursor.lockState + 1) % 2);
            Cursor.visible = !Cursor.visible;
        }
    }

    private void Update()
    {
        MenuControls();
        if ((int)Cursor.lockState == 1)
        {
            rotation.y += Input.GetAxis("Mouse X") * Sensitivity;
            rotation.x += -Input.GetAxis("Mouse Y") * Sensitivity;
            rotation.x = Mathf.Clamp(rotation.x, 270, 440);
            transform.eulerAngles = rotation;
        }

        GrappleHook();

        playerMovement();
    }
}
