using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Runtime.CompilerServices;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using Photon.Realtime;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField]
    private PlayerData playerData;
    
    
    private PlayerManager playerManager;
    [SerializeField] 
    private Animator visualDamageAnimator;
    [SerializeField]
    private Gun[] items;
    
    [SerializeField]
    private Camera playerCamera;
    //[SerializeField]
    //private GameObject destroyFx;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private PhotonView pView;
    [SerializeField]
    private GameObject cameraHolder;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private Image staminaBar;
    
   
    private Vector3 smoothMove, moveAmount;
    private float smoothTime = 0.2f, verticalLookRotation, currentHealth, currentStamina;
    
    private int itemIndex = 1;
    
    public bool isGround {get; set;}


    private void Awake()
    {
        currentHealth = playerData.maxHealth;
        currentStamina = playerData.maxStamina;
    }

    private void Start()
    {
        playerManager = PhotonView.Find((int)pView.InstantiationData[0]).GetComponent<PlayerManager>();
        if(!pView.IsMine)
        {
            Destroy(playerCamera);
            Destroy(ui);
            Destroy(rb);
        }
        else
        {
            EquipeItem(0);
        }
        isGround = true;
        
    }
    private void Update()
    {
        if(!pView.IsMine) return;
        Look();
        Movement();
        Jump();
        SelectWeapon();
        UseItem();
    }
    private void FixedUpdate()
    {
        if(!pView.IsMine) return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * playerData.mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * playerData.mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 90f);
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    private void Movement()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * ((Input.GetKey(KeyCode.LeftShift) && currentStamina >1)? playerData.baseSprintSpeed: playerData.baseWalkSpeed), ref smoothMove, smoothTime);
        if(Input.GetKeyDown(KeyCode.LeftShift) && currentStamina > 0f)
        {
            currentStamina -= 0.15f;
        }
        else
        {
            if(currentStamina < playerData.maxStamina)
            {
                currentStamina += 0.1f;
            }
           
        }
        staminaBar.fillAmount = currentStamina / playerData.maxStamina;
    }
    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(transform.up * playerData.baseJumpForce, ForceMode.Impulse);
            Debug.Log("Jump");
        }
    }
    private void SelectWeapon()
    {
        float mouseAxis = Input.GetAxisRaw("Mouse ScrollWheel");
        if(mouseAxis == 0) return;
        int modifier = 0;
        if(mouseAxis > 0)
        {
            modifier = 1;
        }
        else if(mouseAxis < 0)
        {
            modifier = -1;
        }
        int newIndex = itemIndex + modifier;
        if(newIndex < 0)
        {
            newIndex = items.Length - 1;
        }
        else if(newIndex > items.Length - 1)
        {
            newIndex = 0;
        }
        EquipeItem(newIndex);
    }
    private void EquipeItem(int index)
    {
        if(index == itemIndex) return;
        if(itemIndex != -1)
        {
            items[itemIndex].ItemObject.SetActive(false);
        }
        itemIndex = index;
        items[itemIndex].ItemObject.SetActive(true);
        
       
        if(pView.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("index", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    }
    public void TakeDamage(float damage)
    {
        pView.RPC("RPC_Damage", RpcTarget.All, damage);
    }
    private void UseItem()
    {
        if(Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!pView.IsMine && targetPlayer == pView.Owner)
        {
            EquipeItem((int)changedProps["index"]);
        }
    }
    [PunRPC]
    private void RPC_Damage(float damage)
    {
        if(!pView.IsMine) return;
        currentHealth -= Mathf.Clamp(currentHealth - damage, 0, playerData.maxHealth);
        healthBar.fillAmount = currentHealth / playerData.maxHealth;
        visualDamageAnimator.Play("TakeDamage");
        if(currentHealth <= 0) playerManager.Die(transform.position);
    }



}
