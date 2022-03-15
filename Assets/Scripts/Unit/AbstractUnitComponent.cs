using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class AbstractUnitComponent : MonoBehaviour
{
    protected UnitController _unit;

    void Awake()
    {
        _unit = GetComponent<UnitController>();
    }
}
