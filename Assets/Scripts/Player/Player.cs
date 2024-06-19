using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class Player : MonoBehaviour, IUpdate
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

    [Header("<color=orange>Audio</color>")]
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _attackClips;

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

    private AudioSource _source;
    private Rigidbody _rb;

    private float _xAxis, _zAxis;
    private bool _canAttack = true;

    private Vector3 _dir = new(), _dirCheck = new(), _transformOffset = new();

    private Ray _attackRay, _groundRay, _moveRay;
    private RaycastHit _attackHit;

    private void Awake()
    {
        UpdateManager.Instance.AddObject(this);

        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.angularDrag = 1f;

        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (!_anim) _anim = GetComponentInChildren<Animator>();
    }

    public void ArtUpdate()
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
        else if (Input.GetKeyDown(_areaAttackKey) && _canAttack)
        {
            _anim.SetTrigger((_onAreaAttackName));
        }

        if (Input.GetKeyDown(_jumpKey) && IsGrounded())
        {
            _anim.SetTrigger(_onJumpName);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateManager.Instance.Clear();

            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ArtFixedUpdate()
    {
        if ((_xAxis != 0 || _zAxis != 0) && !IsBlocked(_xAxis, _zAxis))
        {
            Movement(_xAxis, _zAxis);
        }
    }

    public void ArtLateUpdate() { }

    public void Attack()
    {
        _attackRay = new Ray(_attackOrigin.position, transform.forward);

        if(Physics.Raycast(_attackRay, out _attackHit, _attackRange, _attackMask))
        {
            if (_attackHit.collider.GetComponent<Enemy>())
            {
                _attackHit.collider.GetComponent<Enemy>().SetHP(-_attackDmg);
            }
        }
    }

    public void AreaAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(_attackOrigin.position, _areaAttackRadius, _attackMask);

        foreach(Collider enemy in hitEnemies)
        {
            if (!enemy.GetComponent<Enemy>()) return;

            enemy.GetComponent<Enemy>().SetHP(-_attackDmg);
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

    public void PlayStepClip()
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }

        _source.clip = _stepClips[Random.Range(0, _stepClips.Length)];

        _source.Play();
    }

    public void PlayAttackClip()
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }

        _source.clip = _attackClips[Random.Range(0, _attackClips.Length)];

        _source.Play();
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
