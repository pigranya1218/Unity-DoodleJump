using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 10;
    public float maxSpeed = 10;
    public float jumpPower = 300;
    public LayerMask whatIsPlatform;
    Rigidbody2D _rb;
    BoxCollider2D _bc;
    SpriteRenderer _sr;
    Vector2 _moveDirection;
    WaitForSeconds _1s;
    bool _isGround;

    void OnDrawGizmos()
    {
            
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _1s = new WaitForSeconds(1.0f);
    }

    void Update()
    {
        _moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // StartCoroutine(doJump());
            Jump();
        }
        CheckFlip();
        if(!_isGround)
        {
            CheckGround();
        }
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
        RaycastHit2D hit = Physics2D.BoxCast(_bc.transform.position, _bc.size, 0.0f, Vector2.down);

    }

    void Jump() // 점프를 실행하는 함수
    {
        _rb.AddForce(Vector2.up * jumpPower);
    }


    IEnumerator doJump() // 땅에 닿았다고 판단되면 호출되는 함수
    {
        yield return _1s;
        Jump();
    }
}
