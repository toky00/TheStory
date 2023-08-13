using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PL_Movement : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] float walkSpeed = 15, runSpeed = 25, sneakSpeed = 10, jumpStrength, maxSpeed = 5;
    [SerializeField] LayerMask walkableLayers;
    public GameObject camHeightModifier;
    public float footstepVolume;
    CapsuleCollider capsuleCollider;
    public Rigidbody rig;
    public bool isGrounded, landed, canStand;
    bool standing;
    float currentMaxSpeed, x, z, speed;
    Vector3 moveDirection;

    private void Start()
    {
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        rig = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canStand)
        {
            rig.drag = 0;
            rig.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void CheckGround()
    {
        Ray ray = new Ray(gameObject.transform.position, -gameObject.transform.up);
        if (Physics.SphereCast(ray, 0.1f, out var hit, 1f, walkableLayers))
        {
            _animator.SetFloat("Speed", rig.velocity.magnitude);
            isGrounded = true;
        }
        else
        {
            _animator.SetFloat("Speed", 0);
            isGrounded = false;
        }
    }

    void CheckRoof()
    {
        Ray ray = new Ray(gameObject.transform.position, gameObject.transform.up);
        if (Physics.SphereCast(ray, 0.1f, out var hit, 1f, 9))
        {
            canStand = false;
        }
        else
        {
            canStand = true;
        }
    }

    private void FixedUpdate()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        speed = CurrentSpeedAndStance();

        CheckGround();
        CheckRoof();

        if (!isGrounded)
        {
            rig.drag = 0;
            return;
        }

        if (isGrounded && rig.velocity.magnitude >= currentMaxSpeed)
        {
            rig.drag = 10;
        }
        else if (isGrounded && Mathf.Abs(x) == 0 && Mathf.Abs(z) == 0)
        {
            rig.drag = 10;
        }

        if (Mathf.Abs(x) == 1 && Mathf.Abs(z) == 1)
        {
            speed = speed / 2;
        }

        moveDirection = (x * gameObject.transform.right + z * gameObject.transform.forward);

        moveDirection.y = 0;

        rig.AddForce((moveDirection * speed)/10, ForceMode.VelocityChange);

        switch (standing)
        {
            case true:
                camHeightModifier.transform.localPosition = Vector3.Lerp(camHeightModifier.transform.localPosition, new Vector3(0, 0.75f, 0), Time.fixedDeltaTime * 5);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, 2, Time.fixedDeltaTime * 5);
                break;
            case false:
                camHeightModifier.transform.localPosition = Vector3.Lerp(camHeightModifier.transform.localPosition, new Vector3(0, 0.25f, 0), Time.fixedDeltaTime * 5);
                capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, 1, Time.fixedDeltaTime * 5);
                break;
        }
    }

    /// <summary>
    /// This function is used to return the float value of the current speed
    /// </summary>
    /// <returns></returns>
    private float CurrentSpeedAndStance()
    {
        CheckRoof();
        if (Input.GetKey(KeyCode.LeftShift) && canStand)
        {
            currentMaxSpeed = maxSpeed * 0.5f;
            footstepVolume = 0.5f;
            standing = true;
            return runSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || !canStand)
        {
            currentMaxSpeed = maxSpeed * 0.1f;
            footstepVolume = 0.1f;
            standing = false;
            return sneakSpeed;
        }
        else
        {
            currentMaxSpeed = maxSpeed * 0.25f;
            footstepVolume = 0.25f;
            standing = true;
            return walkSpeed;
        }
    }
}
