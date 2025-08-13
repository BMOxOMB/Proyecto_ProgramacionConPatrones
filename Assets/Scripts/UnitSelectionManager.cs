using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    //Singleton instance
    public static UnitSelectionManager instance { get; set; }

    public List<GameObject> selectedUnits = new List<GameObject>();
    public List<GameObject> allUnits = new List<GameObject>();

    private Camera cam;
    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;
    public LayerMask attackable;
    public bool attackCursorVisible;

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //Checks if a clickable object was clicked
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                //Leftshift is held down, so we will multi-select units
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    //If not holding shift, select the clicked object
                    SelectByClicking(hit.collider.gameObject);
                }

            }
            else
            {//If no clickable object was clicked, deselect all units
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectAll();
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedUnits.Count>0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //Checks if a clickable object was clicked
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;

                groundMarker.SetActive(false);
                groundMarker.SetActive(true);

            }
        }

        //Attack target selection
        if (selectedUnits.Count>0 && AtLeastOneOffensiveUnit(selectedUnits))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //Checks if a clickable object was clicked
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable))
            {
                Debug.Log("Enemy Hovered with mouse");

                attackCursorVisible = true;

                //If a target is clicked, set the target for all selected units
                if (Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;
                    foreach (var unit in selectedUnits)
                    {
                        if (unit.GetComponent<AttackController>())
                        {
                            unit.GetComponent<AttackController>().targetToAttack=target;
                        }

                    }
                }

            }
            else
            {
                //If no clickable object was clicked, hide the attack cursor
                    attackCursorVisible = false;
            }
        }
    }

    private bool AtLeastOneOffensiveUnit(List<GameObject> selectedUnits)
    {
        foreach (var unit in selectedUnits)
        {
            if (unit.GetComponent<AttackController>())
            {
                return true; // Found at least one unit with an AttackController
            }

        }
        return false; // No unit with an AttackController found

    }

    private void MultiSelect(GameObject unit)
    {
        if(selectedUnits.Contains(unit))
        {
            //If the unit is already selected, deselect it
            selectedUnits.Remove(unit);
            SelectUnit(unit, false);
        }
        else
        {
            //If the unit is not selected, add it to the selection
            selectedUnits.Add(unit);
            SelectUnit(unit, true);
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in selectedUnits)
        {
            SelectUnit(unit, false);
        }
        groundMarker.SetActive(false);
        selectedUnits.Clear();
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();

        selectedUnits.Add(unit);
        SelectUnit(unit, true);

    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        TriggerSelectionIndicator(unit, isSelected);
        EnableUnitMovement(unit, isSelected);
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isVisible);
    }

    internal void DragSelect(GameObject unit)
    {
        if (selectedUnits.Contains(unit)==false)
        {
            selectedUnits.Add(unit);
            SelectUnit(unit, true);
        }
    }
}
