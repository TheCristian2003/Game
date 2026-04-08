using UnityEngine;
using System.Collections;

public class Movimiento : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed  = 5f;
    public float jumpForce  = 8f;

    [Header("Boost")]
    public float speedBoost = 2f;
    public float jumpBoost = 3f;

    [Header("Deteccion de Suelo")]
    public float     groundCheckDistance;
    public LayerMask groundLayer;

    Rigidbody rb;
    Animator  anim;
    private  bool    isGrounded;
    private float     h, v;

    void Start()
    {
        rb       = GetComponent<Rigidbody>();
        anim     = GetComponent<Animator>();
    }


    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        CheckGround();
        Move();
        Jump();
        UpdateAnimator();
        Attack();
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    void Move()
    {
        Vector3 movement  = new Vector3(h, 0f, v) * moveSpeed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

        if (movement.magnitude > 0.1f && v >= 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.angularVelocity = Vector3.zero;
            anim.SetTrigger("Jump");
        }
    }

    void UpdateAnimator()
    {
        float speed = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;
        anim.SetFloat("Speed",  speed);
        anim.SetBool("isGrounded", isGrounded);
    }

    public void ApplyJumpForce()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }
    }

    IEnumerator BoostTemporal()
    {
        moveSpeed += speedBoost;
        jumpForce += jumpBoost;

        yield return new WaitForSeconds(5f);

        moveSpeed -= speedBoost;
        jumpForce -= jumpBoost;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            Debug.Log("PowerUp recogido!");

            StartCoroutine(BoostTemporal());

            Destroy(other.gameObject);
        }
    }
}

