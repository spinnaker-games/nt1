using System;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour//TODO: investigate the benefit of having seperate force recievers for the player and enemies
{
    [SerializeField] CharacterController _charcaterController;
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] float _drag = 0.3f;

    Vector3 _dampingVelocity;
    Vector3 _impact;

    float _vertivalVelocity;

    public Vector3 Movement => _impact + Vector3.up * _vertivalVelocity;//this allows us to maintain gravity across different player states

    void Update() //TODO: Should this be FixedUpdate since it involves physics?
    {
        if (_vertivalVelocity < 0f && _charcaterController.isGrounded)
        {
            _vertivalVelocity = Physics.gravity.y * Time.deltaTime;// this prevents the player from constantly falling event if they are slightly off the ground
        }
        else
        {
            _vertivalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag);//TODO: Understand this function

        if (_navMeshAgent != null)
        {
            
            if (_impact.sqrMagnitude < 0.2f * 0.2f)
            /*
                Use a small threshold instead of Vector3.zero because floating point
                values rarely reach exactly zero. This prevents tiny residual movement
                that can cause "running on the spot" before re-enabling the NavMeshAgent.
            */
            {
                _impact = Vector3.zero;
                _navMeshAgent.enabled = true; //turn agent back on since we are no longer applying phusics
            }
        }
    }


    public void Reset()
    {
        _impact = Vector3.zero;
        _vertivalVelocity = 0f;
    }

    public void AddForce(Vector3 force)
    {
        _impact += force;
        if (_navMeshAgent != null)
        {
            _navMeshAgent.enabled = false; //turn agent off to avoid any issues with applying physics to enemy
        }
    }

    public void Jump(float jumpForce)
    {
        _vertivalVelocity = jumpForce;
    }
}