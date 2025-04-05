using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.Tilemaps;

public class Gun : Weapon<GunData>
{
    [SerializeField]
    private Camera playerCam;
    [SerializeField]
    private GameObject bulletPref;
    [SerializeField]
    private GameObject shootFX;
    [SerializeField]
    private Transform muzzle;
    [SerializeField]
    private Animator GunAnimator;
    private bool isReloading = false;
    private int currentAmmo;
     [SerializeField]
    private TMP_Text ammoText;
    
    private PhotonView pView;
    
    public  override void Use()
    {
        Shoot();
    }
    protected override void Awake()
    {
        pView = GetComponent<PhotonView>();
        GunAnimator = GetComponent<Animator>();
        if(!pView.IsMine)
        {
            Destroy(ammoText);
        }

        
    }
    protected override void Start()
    {
        currentAmmo = data.maxAmmo;
        UpdateAmmoUI();
    }
    private void OnEnable()
    {
        UpdateAmmoUI();
    }
    protected override void Update()
    {

    }
    private void Shoot()
    {
        if(currentAmmo < 1 && !isReloading) return;
            currentAmmo--;
            UpdateAmmoUI();
            //GunAnimator.Play("ShootAnim");
            Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        ray.origin = playerCam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?. TakeDamage(((WeaponData)data).Damage);
            pView.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
        if(currentAmmo < 1 && !isReloading)
        {
            isReloading = true;
            //GunAnimator.Play("RealodAnim");
            Invoke("Reload", data.reloadTime);
        }
       
        
        
    }
    [PunRPC]
    private void RPC_Shoot(Vector3 hitPoint, Vector3 hitNormal)
    {
        Collider[] colls = Physics.OverlapSphere(hitPoint, 0.1f);
        if (colls.Length != 0)
        {
            GameObject bulletImp = Instantiate(bulletPref, hitPoint, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletPref.transform.rotation);
            bulletImp.transform.SetParent(colls[0].transform);
        }
    }
    private void Reload()
    {
        currentAmmo = data.maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
    }
    private void UpdateAmmoUI()
    {
        ammoText.text = $"Ammo : {currentAmmo}";
    }
}
