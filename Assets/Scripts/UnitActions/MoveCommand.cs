using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : IUnitCommand
{
    System.Action _onExectuted;
    UnitController _unit;
    Vector3 _destination;
    float _speed;

    public MoveCommand(UnitController unit, Vector3 dest, float speed, System.Action onExectued = null)
    {
        _unit = unit;
        _destination = dest;
        _speed = speed;
        _onExectuted = onExectued;
    }

    public Action OnExecuted 
    { 
        get => _onExectuted;
    }

    public IEnumerator Execute()
    {
        Vector3 curPos = _unit.transform.position;
        float dist = Vector3.Distance(curPos, _destination);
        float estimatedTime = dist / _speed;
        float timeoutTimer = 0;

        while (Vector3.Distance(curPos, _destination) > 0.01f)
        {
            //float step = _speed * Time.deltaTime;
            //_unit.transform.position = Vector3.MoveTowards(_unit.transform.position, _destination, step);
            //Vector3 dir = (_destination - curPos).normalized;

            //Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            //_unit.transform.rotation = rot;

            _unit.Agent.destination = _destination;

            yield return new WaitForEndOfFrame();
            curPos = _unit.transform.position;

            // Detect timeout
            timeoutTimer += Time.deltaTime;
            if (timeoutTimer > estimatedTime * 2)
                break;
        }

        if (_onExectuted != null)
            _onExectuted();
    }
}
