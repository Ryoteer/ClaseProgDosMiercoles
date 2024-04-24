using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    private Player _parent;

    private void Start()
    {
        _parent = GetComponentInParent<Player>();
    }

    public void Attack()
    {
        _parent.Attack();
    }
    public void AreaAttack()
    {
        _parent.AreaAttack();
    }

    public void AttackState(int state)
    {
        _parent.AttackState(state);
    }

    public void Jump()
    {
        _parent.Jump();
    }
}
