using UnityEngine;

[RequireComponent(typeof(ArmyController))]
public abstract class AbstractCommandInput : MonoBehaviour, ICommandInput
{
    protected ArmyController _army;

    void Awake()
    {
        _army = GetComponent<ArmyController>();
    }

    protected virtual void OnEnable()
    {
        _army.HighlightAvailableUnits();
    }

    protected virtual void OnDisable()
    {
        _army.UnhighlightUnits();
    }
}