using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckUIClick : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;

    private void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    private void Update()
    {
        List<RaycastResult> results = CheckUIElement();

        if (results != null && results.Count == 0)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 newPosJoystick = Camera.main.ScreenToViewportPoint(touch.position);
                    if (newPosJoystick.x > -1 && newPosJoystick.x < 1 && newPosJoystick.y > -1 && newPosJoystick.y < 1)
                    {
                        target.transform.position = touch.position;
                    }
                }
            }
        }
    }

    private List<RaycastResult> CheckUIElement()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            m_PointerEventData.position = touch.position;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);
            return results;
        }
        return null;
    }
}