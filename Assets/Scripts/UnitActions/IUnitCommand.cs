using System.Collections;
using UnityEngine;

public interface IUnitCommand
{
    public System.Action OnExecuted { get; }
    public IEnumerator Execute();
}
