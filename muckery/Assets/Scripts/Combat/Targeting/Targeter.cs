using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup _cinemachineTargetGroup;

    Camera _mainCamera;
    public List<Target> _targets = new List<Target>();

    public Target CurrentTarget { get; set; }

    void Start()
    {
        _mainCamera = Camera.main; //TODO: put this in awake????
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        _targets.Add(other.GetComponent<Target>());
        target.OnDestroyedEvent += RemoveTarget;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        _targets.Remove(other.GetComponent<Target>());

        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if (_targets.Count == 0) { return false; }

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity; //largest possible float in unity

        foreach (Target target in _targets)
        {
            Vector2 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);
            //check if the targets position is within the screen
            if (!target.GetComponentInChildren<Renderer>().isVisible)//TODO: explore alternative solutions to checking if target is on screen
            {
                continue;
            }

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < closestTargetDistance) //sqrMagnitude is more performant than magnitude
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null) { return false; }

        CurrentTarget = closestTarget;
        _cinemachineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f); //are weight and radius magic numbers?
        
        return true;
    }

    public void CancelTarget()
    {
        if (CurrentTarget == null) { return; }

        _cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            _cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyedEvent -= RemoveTarget;
        _targets.Remove(target);
    }
}