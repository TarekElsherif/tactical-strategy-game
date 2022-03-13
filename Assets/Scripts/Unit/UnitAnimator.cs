using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class UnitAnimator : MonoBehaviour
{
    UnitController _controller;

    [SerializeField] Animator _animator;

    void Awake()
    {
        _controller = GetComponent<UnitController>();
    }

    void OnEnable()
    {
        _controller.OnStartMovement += StartMovement;
        _controller.OnEndMovement += EndMovement;
        _controller.OnStartAttack += Shoot;
        _controller.OnDamageTaken += TakeDamage;
        _controller.OnDie += Die;
    }

    void OnDisable()
    {
        _controller.OnStartMovement -= StartMovement;
        _controller.OnEndMovement -= EndMovement;
        _controller.OnStartAttack -= Shoot;
        _controller.OnDamageTaken -= TakeDamage;
        _controller.OnDie -= Die;
    }

    void StartMovement(Vector3 origin, Vector3 dest)
    {
        _animator.SetBool("moving", true);
    }

    void EndMovement(Vector3 dest)
    {
        _animator.SetBool("moving", false);
    }

    void Shoot(UnitController victim)
    {
        _animator.SetTrigger("shoot");
    }

    void TakeDamage(float damage)
    {
        _animator.SetTrigger("hit");
    }

    void Die(UnitController unit)
    {
        _animator.SetBool("dead", true);
    }
}
