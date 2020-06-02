using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //public variables for speeds
    //panspeed = general movement speed
    public float panSpeed;
    public float rotateSpeed;
    public float rotateAmount;

    private Quaternion rotation;
    //general detection through our screen
    private float panDetect = 25f;

    //zoom ranges
    private float minHeight = 10f;
    private float maxHeight = 100f;

    private ObjectStatus selectedStatus;

    public GameObject selectedObject;


    // Start is called before the first frame update
    void Start()
    {
        rotation = Camera.main.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        RotateCamera();

        if(Input.GetMouseButtonDown(0))
        {
            LeftClick();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.transform.rotation = rotation;
        }
    }

    void MoveCamera()
    {
        float moveX = Camera.main.transform.position.x;
        float moveY = Camera.main.transform.position.y;
        float moveZ = Camera.main.transform.position.z;

        float xPosition = Input.mousePosition.x;
        float yPosition = Input.mousePosition.y;

        //checking if mouse close enough to the edge of the screens.

        if(Input.GetKey(KeyCode.A) || xPosition > 0 && xPosition < panDetect )
        {
            moveX -= panSpeed;
        }

        else if(Input.GetKey(KeyCode.D) || xPosition < Screen.width && xPosition > Screen.width - panDetect)
        {
            moveX += panSpeed;
        }

        if(Input.GetKey(KeyCode.W) || yPosition < Screen.height && yPosition > Screen.height - panDetect)
        {
            moveZ += panSpeed;
        }

        else if (Input.GetKey(KeyCode.S) || yPosition > 0 && yPosition < panDetect)
        {
            moveZ -= panSpeed;
        }

        moveY -= Input.GetAxis("Mouse ScrollWheel") * (panSpeed * 20);
        moveY = Mathf.Clamp(moveY, minHeight, maxHeight);

        Vector3 newPos = new Vector3(moveX, moveY, moveZ);
        Camera.main.transform.position = newPos;
    }

    void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        if(Input.GetMouseButton(2))
        {
            destination.x -= Input.GetAxis("Mouse Y") * rotateAmount;
            destination.y += Input.GetAxis("Mouse X") * rotateAmount;
        }

        if(destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }
    }

    public void LeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit,100))
        {
            if(hit.collider.tag == "Ground")
            {
                selectedStatus.isSelected = false;
                Debug.Log("Deselected");
                
            }

            else if(hit.collider.tag == "Selectable")
            {
                selectedObject = hit.collider.gameObject;
                selectedStatus = selectedObject.GetComponent<ObjectStatus>();

                selectedStatus.isSelected = true;

                Debug.Log("Selected " + selectedStatus.objectName);
            }
        }
    }
}
