using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsControl : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Coyote")]
    [SerializeField] public float coyoteTimer;
    [SerializeField] private float coyoteSetTimer;

    [Header("Ground")]
    [SerializeField] private float groundRayDistance;
    [SerializeField] private Transform leftGroundPoint;
    [SerializeField] private Transform rightGroundPoint;
    [SerializeField] private LayerMask whatToDetect;
    
    public bool grounded;

    [Header("Interpolation")]
    [SerializeField] private RigidbodyInterpolation2D interpolation;
    [SerializeField] private RigidbodyInterpolation2D extrapolate;

    private RaycastHit2D hitIntoLeft;
    private RaycastHit2D hitIntoRight;

    [Header("Wall")]
    [SerializeField] private float wallRayDistance;
    [SerializeField] private Transform upperWallPoint;
    [SerializeField] private Transform lowerWallPoint;

    public bool wallDetected;

    [Header("Colliders")]
    [SerializeField] private Collider2D standingCollider;
    [SerializeField] private Collider2D crouchCollider;

    private RaycastHit2D hitIntoUpper;
    private RaycastHit2D hitIntoLower;

    private float gravityScale;

    private bool canAirDash = true;

    void Start()
    {
        gravityScale = rb.gravityScale;
        coyoteTimer = coyoteSetTimer;
    }
    private bool CheckWall()
    {
        hitIntoUpper = Physics2D.Raycast(upperWallPoint.position, transform.right, wallRayDistance, whatToDetect);
        hitIntoLower = Physics2D.Raycast(lowerWallPoint.position, transform.right, wallRayDistance, whatToDetect);

        Debug.DrawRay(upperWallPoint.position, new Vector3(wallRayDistance, 0, 0), Color.red);
        Debug.DrawRay(lowerWallPoint.position, new Vector3(wallRayDistance, 0, 0), Color.red);

        if (hitIntoUpper || hitIntoLower)
        {
            return true;
        }
        return false;
    }
    private bool CheckGround()
    {
        hitIntoLeft = Physics2D.Raycast(leftGroundPoint.position, Vector2.down, groundRayDistance, whatToDetect);
        hitIntoRight = Physics2D.Raycast(rightGroundPoint.position, Vector2.down, groundRayDistance, whatToDetect);

        bool isOnMovingPlatform = transform.parent != null && transform.parent.CompareTag("MovingPlatform");

        Debug.DrawRay(leftGroundPoint.position, new Vector3(0, -groundRayDistance, 0), Color.red);
        Debug.DrawRay(rightGroundPoint.position, new Vector3(0, -groundRayDistance, 0), Color.red);

        if(hitIntoLeft || hitIntoRight || isOnMovingPlatform)
        {
            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        grounded = CheckGround();
        wallDetected = CheckWall();
        if (grounded || wallDetected)
            canAirDash = true;
    }
    private void Update()
    {
        if (!grounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = coyoteSetTimer;
        }
    }
    public void EnableGravity()
    {
        rb.gravityScale = gravityScale;
    }
    public void DisableGravity()
    {
        rb.gravityScale = 0;
    }
    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }
    public void SwitchCollider()
    {
        crouchCollider.enabled = !crouchCollider.enabled;
        standingCollider.enabled = !standingCollider.enabled;
    }

    public bool getCanAirDash()
    {
        return canAirDash;
    }
    public void setCanAirDash(bool _canAirDash)
    {
        canAirDash = _canAirDash;
    }

    public void setInterpolate()
    {
        
    }

    public void setExtrapolate()
    {

    }

    public bool TryDropThrough()
    {
        Collider2D collider;
        if (hitIntoLeft.collider.CompareTag("OneWayPlatform"))
        {
            collider = hitIntoLeft.collider;
            StartCoroutine(disableOneWayCollider(collider));
        }else if (hitIntoRight.collider.CompareTag("OneWayPlatform")){
            collider = hitIntoRight.collider;
            StartCoroutine(disableOneWayCollider(collider));
        }
        else
        {
            return false;
        }
        return true;
    }

    private IEnumerator disableOneWayCollider(Collider2D platformCollider)
    {
        Physics2D.IgnoreCollision(crouchCollider, platformCollider, true);
        Physics2D.IgnoreCollision(standingCollider, platformCollider, true);

        yield return new WaitForSeconds(1.0f);

        Physics2D.IgnoreCollision(crouchCollider, platformCollider, false);
        Physics2D.IgnoreCollision(standingCollider, platformCollider, false);
    }
}
