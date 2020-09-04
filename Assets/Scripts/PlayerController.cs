using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 10;
    public float maxSpeed = 10;
    public float jumpPower = 300;
    public LayerMask whatIsPlatform;
    public GameObject jumpDust;

    Rigidbody2D _rb;
    BoxCollider2D _bc;
    SpriteRenderer _sr;
    Vector2 _moveDirection;
    Animator _ani;
    bool _isGround;

    void OnDrawGizmos()
    {
            
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _ani = GetComponent<Animator>();
        _isGround = true;
    }

    void Update()
    {
        _moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // StartCoroutine(doJump());
            DoJump();
        }
        CheckFlip();
        
    }

    void FixedUpdate()
    {
        if(_moveDirection.x != 0)
        {
            _rb.AddForce(_moveDirection * speed);
            _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -maxSpeed, maxSpeed), _rb.velocity.y);
        }
        else
        {
            _rb.velocity = new Vector2(_rb.velocity.x * 0.9f, _rb.velocity.y);
        }
        _ani.SetFloat("VelocityY", _rb.velocity.y);
        if (!_isGround)
        {
            CheckGround();
        }
    }

    void CheckFlip()
    {
        if(_moveDirection.x != 0)
        {
            if (_moveDirection.x < 0) // 왼쪽 보게 하기
            {
                _sr.flipX = true;
            }
            else
            {
                _sr.flipX = false;
            }
        }
        
    }

    void CheckGround()
    {
        if(_rb.velocity.y < 0)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, _bc.size, 0.0f, Vector2.down, 0.1f, whatIsPlatform);
            if (hit.collider != null)
            {
                _isGround = true;
                _ani.SetTrigger("Ground");
            }
        }
    }

    void Jump() // 점프를 실행하는 함수
    {
        _rb.AddForce(Vector2.up * jumpPower);
        _isGround = false;
        // jumpDust.SetActive(true);
    }

    void DoJump() // 점프하는 함수
    {
        _ani.SetTrigger("Jump");
    }
}
