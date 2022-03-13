using UnityEngine;

public class MouseCommandInput : AbstractCommandInput
{
    void Update()
    {
        if (_army.IsInAction)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                UnitController unit = hit.collider.gameObject.GetComponent<UnitController>();

                if (unit != null)
                {
                    if (_army.IsFriendlyUnit(unit))
                    {
                        _army.SelectUnit(unit);
                    } 
                    else if (_army.SelectedUnit != null)
                    {
                        _army.ShootUnit(_army.SelectedUnit, unit);
                    }
                } else
                {
                    if ((hit.collider.tag == "Platform") && (_army.SelectedUnit != null))
                    {
                        Vector3 dest = new Vector3(hit.point.x, _army.SelectedUnit.transform.position.y, hit.point.z);
                        _army.MoveUnit(_army.SelectedUnit, dest);
                    }
                }
            }
        }
    }
}
