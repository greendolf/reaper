using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    // Update File title
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode jump = KeyCode.Space;


    public float speed = 15;
    public float jumpForce = 25;
    private int extJumps;
    public int extJumpsValue = 1;

    private Rigidbody2D rb;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    public int hpValue = 100;
    public int hp;
    public Text hpDisplay;

    public int coins;
    public Text coinsDisplay;
    public void AddCoins(int count)
    {
        coins += count;
    }
    void Start()
    {
        hp = hpValue;
        extJumps = extJumpsValue;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        hpDisplay.text = hp.ToString();
        coinsDisplay.text = coins.ToString();
        LifeLogic();
        MovementLogic();
        JumpLogic();
    }

    //**************************************************************************************************

    private void LifeLogic()
    {
        if (hp <= 0)
        {
            Flip();
        }
    }
    private void MovementLogic()
    {
        if (Input.GetKey(right))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else if (Input.GetKey(left))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (!facingRight && Input.GetKey(right))
        {
            Flip();
        }
        else if(facingRight && Input.GetKey(left))
        {
            Flip();
        }
    }

    private void JumpLogic()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        /*if (Physics2D.Linecast(transform.position, feetPos.position, 3 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }*/
        if (isGrounded)
        {
            extJumps = extJumpsValue;
        }
        if (Input.GetKeyDown(jump) && !isGrounded && (extJumps > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            extJumps--;
        }
        else if (Input.GetKeyDown(jump) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
//**************************************************************************************************

    /*void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject obj = collider.gameObject;
        if (obj.CompareTag("Enemy"))
        {
            int dmg = obj.GetComponent<MeleeDamage>().dmg;
            GetDamage(dmg);
        }
    }*/

    public void GetDamage(int value, Transform enemy, float knockback)
    {
        Vector3 direction = new Vector3( /*transform.position.x * transform.localScale.x*/10.0f, 2.0f, 0.0f);
        print("Damaged");
        hp -= value;
        //transform.position = new Vector3(10.0f, 2.0f, 0.0f);
        //rb.AddForce(direction * knockback /** -transform.localScale.x*/, ForceMode2D.Impulse);
        Vector3 pushFrom = enemy.position;
        Vector3 pushDirection = (pushFrom - transform.position).normalized;
        pushDirection = new Vector3(pushDirection.x, 10f, 0f);
        // ������� ������ � ������ ����������� � ����� knockback
        rb.AddForce(pushDirection * knockback * enemy.localScale.x, ForceMode2D.Impulse);
    }
}
