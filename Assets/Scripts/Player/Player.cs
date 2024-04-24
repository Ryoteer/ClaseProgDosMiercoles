using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("<color=orange>Animation</color>")]
    [SerializeField] private Animator _anim;
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";
    [SerializeField] private string _isMovingName = "isMoving";
    [SerializeField] private string _onJumpName = "onJump";
    [SerializeField] private string _isGroundedName = "isGrounded";
    [SerializeField] private string _onAttackName = "onAttack";
    [SerializeField] private string _onAreaAttackName = "onAreaAttack";

    [Header("<color=orange>Inputs</color>")]
    [SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _areaAttackKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    [Header("<color=orange>Physics</color>")]
    [SerializeField] private Transform _attackOrigin;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private LayerMask _attackMask;
    [SerializeField] private float _areaAttackRadius = 2f;
    [SerializeField] private float _groundRange = .75f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _moveRange = .75f;
    [SerializeField] private LayerMask _moveMask;

    [Header("<color=orange>Values</color>")]
    [SerializeField] private int _attackDmg = 5;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody _rb;

    private float _xAxis, _zAxis;
    private bool _canAttack = true;

    private Vector3 _dir = new(), _dirCheck = new(), _transformOffset = new();

    private Ray _attackRay, _groundRay, _moveRay;
    private RaycastHit _attackHit;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.angularDrag = 1f;
    }

    private void Start()
    {
        if (!_anim) _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _anim.SetFloat(_xAxisName, _xAxis);
        _zAxis = Input.GetAxis("Vertical");
        _anim.SetFloat(_zAxisName, _zAxis);

        _anim.SetBool(_isMovingName, (_xAxis != 0 || _zAxis != 0));
        _anim.SetBool(_isGroundedName, IsGrounded());

        if (Input.GetKeyDown(_attackKey) && _canAttack)
        {
            _anim.SetTrigger(_onAttackName);
        }
        else if(Input.GetKeyDown(_areaAttackKey) && _canAttack)
        {
            _anim.SetTrigger((_onAreaAttackName));
        }

        if (Input.GetKeyDown(_jumpKey) && IsGrounded())
        {
            _anim.SetTrigger(_onJumpName);
        }        
    }

    private void FixedUpdate()
    {
        if((_xAxis != 0 || _zAxis != 0) && !IsBlocked(_xAxis, _zAxis))
        {
            Movement(_xAxis, _zAxis);
        }
    }

    public void Attack()
    {
        _attackRay = new Ray(_attackOrigin.position, transform.forward);

        if(Physics.Raycast(_attackRay, out _attackHit, _attackRange, _attackMask))
        {
            Debug.Log($"El personaje le pegó al objeto de nombre <color=red>{_attackHit.collider.name}</color>.");
        }
    }

    public void AreaAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(_attackOrigin.position, _areaAttackRadius, _attackMask);

        foreach(Collider enemy in hitEnemies)
        {
            print($"El enemigo de nombre <color=red>{enemy.name}</color> fué afectado por el ataque en área.");
        }
    }

    public void AttackState(int state)
    {
        switch (state)
        {
            case 0:
                _canAttack = false;
                break;
            case 1:
                _canAttack = true;
                break;
        }
    }

    public void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void Movement(float xAxis, float zAxis)
    {
        _dir = (transform.right * xAxis + transform.forward * zAxis).normalized;

        _rb.MovePosition(transform.position + _dir * _moveSpeed * Time.fixedDeltaTime);
    }

    private bool IsBlocked(float xAxis, float zAxis)
    {
        _dirCheck = (transform.right * xAxis + transform.forward * zAxis);

        _moveRay = new Ray(transform.position, _dirCheck);

        return Physics.Raycast(_moveRay, _moveRange, _moveMask);
    }

    private bool IsGrounded()
    {
        _transformOffset = new Vector3(transform.position.x, 
                                       transform.position.y + _groundRange / 4,
                                       transform.position.z);

        _groundRay = new Ray(_transformOffset, -transform.up);

        return Physics.Raycast(_groundRay, _groundRange, _groundMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_moveRay);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_groundRay);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_attackRay);
    }
}
