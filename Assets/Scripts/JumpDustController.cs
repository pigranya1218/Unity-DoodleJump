using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDustController : MonoBehaviour
{
    Animator _ani;
    SpriteRenderer _sr;
    void OnEnable()
    {
        _sr.flipX = PlayerController.Instance.xFlip;
    }
    void Awake()
    {
        _ani = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }

    void AnimationFinish()
    {
        gameObject.SetActive(false);
    }
}
