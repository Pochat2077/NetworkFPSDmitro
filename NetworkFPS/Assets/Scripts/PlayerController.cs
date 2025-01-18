using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Runtime.CompilerServices;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private PhotonView pView;
    private Vector3 smoothMove, moveAmount;
    private float walkSpeed = 1f, sprintSpeed = 1f, mouseSensitivity = 1f, jumpForce = 1f, smoothTime = 1f, verticalLookRotation;
    private bool isGround;

    private void Start()
    {
        if(!pView.IsMine)
        {
            Destroy(playerCamera);
        }
    }
    private void Update()
    {
        if(!pView.IsMine) return;
        Look();
        Movement();
    }
    private void FixedUpdate()
    {
        if(!pView.IsMine) return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 90f);
        playerCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    private void Movement()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKeyDown(KeyCode.LeftShift)? sprintSpeed: walkSpeed), ref smoothMove, smoothTime);
    }


}
