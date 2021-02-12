using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scrollSense;
    public float panSense;

    public float maxZoom, minZoom;
    public float zoom;

    private bool cameraMove;

    public float cursorMoveStartX, cursorMoveStartY;

    void Start()
    {
        zoom = 5;
    }

    void Update()
    {
        zoom += -Input.GetAxis("Mouse ScrollWheel") * scrollSense;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        this.GetComponent<Camera>().orthographicSize = zoom;

        if (Input.GetMouseButtonDown(2))
        {
            cameraMove = true;
            cursorMoveStartX = Input.mousePosition.x / Screen.width;
            cursorMoveStartY = Input.mousePosition.y / Screen.height;
        }
        else if (Input.GetMouseButtonUp(2)){
            cameraMove = false;
        }

        if (cameraMove)
        {
            float currentCursorX, currentCursorY;
            currentCursorX = Input.mousePosition.x / Screen.width;
            currentCursorY = Input.mousePosition.y / Screen.height;

            this.transform.Translate(new Vector3((cursorMoveStartX - currentCursorX) * -panSense, (cursorMoveStartY - currentCursorY) * -panSense, 0));
        }
    }
}
