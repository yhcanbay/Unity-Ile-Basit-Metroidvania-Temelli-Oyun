using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [Header("References")]
    public InputActionReference shootActionRef;
    public Weapon currentWeapon;
    public Player player;

    private ItemTypes currentWeaponType;
    private bool shootButtonHeld;
    private bool shootCooldownOver = true;

    [Header("Raycast Stuff")]
    [SerializeField] private LayerMask whatToHit;
    [SerializeField] private LineRenderer lineRenderer;
    private bool isShootLineActive = false;
    private Vector3 startPoint;
    private Vector3 endPoint;

    public static Action<Sprite,int,int,int> OnUpdateAllInfo; 
    public static Action<int,int,int> OnUpdateAmmo;

    private void OnEnable()
    {
        shootActionRef.action.performed += TryToShoot;
        shootActionRef.action.canceled += StopShoot;
    }

    private void OnDisable()
    {
        shootActionRef.action.performed -= TryToShoot;
        shootActionRef.action.canceled -= StopShoot;
    }

    void Start()
    {
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
        OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite,currentWeapon.currentAmmo,currentWeapon.maxAmmo,currentWeapon.storageAmmo);
    }

    // Update is called once per frame
    void Update()
    {

        if (isShootLineActive)
        {
            lineRenderer.SetPosition(0, currentWeapon.shootingPoint.position);
            lineRenderer.SetPosition(1, endPoint);
        }

        if(currentWeapon.isAutomatic && shootButtonHeld && shootCooldownOver)
        {
            Shoot();
        }
    }
    private void TryToShoot(InputAction.CallbackContext context)
    {
        if (currentWeapon == null
            || player.stateMachine.currentState == PlayerStates.State.Ladders
            || player.stateMachine.currentState == PlayerStates.State.Dash
            || player.stateMachine.currentState == PlayerStates.State.WallSlide
            || player.stateMachine.currentState == PlayerStates.State.Knockback)
        {
            return;
        }

        if (currentWeapon.currentAmmo <= 0 || shootButtonHeld || shootCooldownOver == false)
        {
            return;
        }

        if (currentWeapon.isAutomatic)
        {
            shootButtonHeld = true;
        }

        shootButtonHeld = true;
        Shoot();
    }

    private void StopShoot(InputAction.CallbackContext context)
    {
        shootButtonHeld = false;
    }

    private void Shoot()
    {
        if(currentWeapon.currentAmmo <= 0)
        {
            return;
        }
        lineRenderer.positionCount = 2;
        Vector3 directiron = currentWeapon.shootingPoint.right;
        RaycastHit2D hitInfo = Physics2D.Raycast(currentWeapon.shootingPoint.position, directiron, Mathf.Infinity, whatToHit);
        if (hitInfo)
        {
            startPoint = currentWeapon.shootingPoint.position;
            endPoint = hitInfo.point;
            //lineRenderer.SetPosition(0, startPoint);
            //lineRenderer.SetPosition(1, endPoint);
            Debug.Log("Hit Something");
        }
        else
        {
            startPoint = currentWeapon.shootingPoint.position;
            endPoint = currentWeapon.transform.position + directiron*10;
            //lineRenderer.SetPosition(0, startPoint);
            //lineRenderer.SetPosition(1, endPoint);
            Debug.Log("Dont Hit Anything");
        }
        IDamagable damageObject = hitInfo.collider.GetComponent<IDamagable>();
        if(damageObject != null)
        {
            damageObject.TakeDamage(currentWeapon.damage);
        }

        currentWeapon.currentAmmo -= 1;
        isShootLineActive = true;
        StartCoroutine(ShootDelay());
        StartCoroutine(ResetShootLine());
        OnUpdateAmmo?.Invoke(currentWeapon.currentAmmo,currentWeapon.maxAmmo,currentWeapon.storageAmmo);

    }

    private IEnumerator ShootDelay()
    {
        shootCooldownOver = false;
        yield return new WaitForSeconds(currentWeapon.shootCooldown);
        shootCooldownOver = true;
    }

    private IEnumerator ResetShootLine()
    {
        yield return new WaitForSeconds(currentWeapon.visibleLineTime);
        lineRenderer.positionCount = 0;
        isShootLineActive = false;
    }
}
