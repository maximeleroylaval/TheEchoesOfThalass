using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    public float sensHorizontal = 10.0f;
    public float sensVertical = 10.0f;

    private float rotationX = 0;

    private bool control = true;

    void rotateX()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensHorizontal, 0);

        rotationX -= Input.GetAxis("Mouse Y") * sensVertical;
        rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);
    }

    void rotateY()
    {
        transform.localEulerAngles = new Vector3(rotationX, transform.localEulerAngles.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.control)
            return;

        this.rotateX();
        this.rotateY();
    }

    public void disable()
    {
        this.control = false;
    }
}
