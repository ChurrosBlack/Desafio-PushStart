using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BuildingType type;
    GameObject buildingObj;
    Building buildingScript;
    Vector2 offset;
    Vector3 startPosition;
    Vector3 offsetToMouse;
    float zDistToCamera;
    public int cashCost;
    GameManager manager;
    [SerializeField]
    CameraMovement cameraMov;

    void Start()
    {
        manager = GameManager.managerSingleton;
        manager.buildingHUDList.Add(new BuildingHUD(GetComponent<Image>(), this));
        manager.UpdateHUDList();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        cameraMov.building = true;
        startPosition = transform.position;
        zDistToCamera = Mathf.Abs(startPosition.z - Camera.main.transform.position.z);
        offsetToMouse = startPosition - Camera.main.ScreenToWorldPoint(
             new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistToCamera)
         );
        buildingObj = manager.InstantiateBuilding(type, transform.position) as GameObject;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;

        buildingObj.transform.position = Camera.main.ScreenToWorldPoint(
           new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistToCamera)) + offsetToMouse;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition;

        buildingScript = buildingObj.GetComponent<Building>();
        if (buildingScript.canBuild)
        {
            buildingScript.AddToManagerList();
            manager.PayCash(cashCost);
        }
        else
        {
            Destroy(buildingObj);
        }
        cameraMov.building = false;
    }


}
