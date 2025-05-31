using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        // 가로가 우선
        if (inputX != 0)
        {
            _moveInput = new Vector2(inputX, 0f);
        }
        else if (inputY != 0)
        {
            _moveInput = new Vector2(0f, inputY);
        }
        else
        {
            _moveInput = Vector2.zero;
        }

        if (_moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f); // 오른쪽 보기
        }
        else if (_moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f); // 왼쪽 보기 (Z축 반전)
        }
        else if (_moveInput.y < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (_moveInput.y > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }


    void FixedUpdate()
    {
        _rb.velocity = _moveInput * moveSpeed;
    }
    
}
