using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance
    {
        get
        {
            return instance;
        }
    }

    static PlayerController instance;

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
        if(_bc != null)
        {
            if (_isGround)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Vector2 boxPos = transform.position;
            boxPos.y += _bc.offset.y;
            Gizmos.DrawWireCube(boxPos, _bc.size);
        }
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _ani = GetComponent<Animator>();
        _isGround = false;
    }

    void Update()
    {
        _moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        if(_isGround && _rb.velocity.y == 0)
        {
            DoJump();
        }

        CheckFlip();
    }

    void FixedUpdate()
    {
        // 좌우 움직임 처리
        if(_moveDirection.x != 0)
        {
            _rb.AddForce(_moveDirection * speed);
            _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -maxSpeed, maxSpeed), _rb.velocity.y);
        }
        else
        {
            _rb.velocity = new Vector2(_rb.velocity.x * 0.9f, _rb.velocity.y);
        }
        // 상하 움직임 처리
        if (!_isGround)
        {
            CheckGround();
        }
        _ani.SetFloat("VelocityY", _rb.velocity.y);
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
        if(_rb.velocity.y <= 0)
        {
            Vector2 boxPos = transform.position;
            boxPos.y += _bc.offset.y;
            RaycastHit2D hit = Physics2D.BoxCast(boxPos, _bc.size, 0.0f, Vector2.down, 0.1f, whatIsPlatform);
            if (hit.collider != null)
            {
                GameManager.Instance.setCameraPos(transform.position.y);
                _isGround = true;
            }
        }
    }

    void Jump() // 점프를 실행하는 함수
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0.1f);
        _rb.AddForce(Vector2.up * jumpPower);
        _isGround = false;
    }

    void DoJump() // 점프하는 함수
    {
        _ani.SetTrigger("Jump");
    }
}
