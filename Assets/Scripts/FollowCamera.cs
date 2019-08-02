using System.Linq.Expressions;
using UnityEngine;
//Basic Camera Controller Script
//Author: @elijah
public class FollowCamera : MonoBehaviour
{
    
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float zoomSpeed;
    
    
    //cam references
    private Transform defaultCameraTransform;
    private Camera mainCam;
    
    //boosl to determine whether cam can move/zoom/rotate
    private bool canRotate;
    private bool canZoom;

    //rotation variables
    private float camXRotation = 0;
    private float camYRotation = 0;
    
    //serialized totalZoom
    
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    private float zoomTotal;
    private float zoomAmt;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        defaultCameraTransform = transform;
        canRotate = true;
        canZoom = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
      cameraMovement();
    }

    private void cameraMovement()
    {
        transform.position = target.position;

        
        //handle rotation if right mouse button clicked
        if (canRotate && Input.GetMouseButton(1))
        {
            camYRotation += (Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime);
            camXRotation += (Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime);
            
            
            //clamp these two float values
            camXRotation = Mathf.Clamp(camXRotation, -25, 30);
            transform.localEulerAngles =  new Vector3(camXRotation, camYRotation, 0);

        }

        if (canZoom)
        {
            zoomMovement();
        }
    }

    private void zoomMovement()
    {

        //return val between -1 and 1, depending on direction of zoom
        zoomAmt = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        zoomTotal += zoomAmt;
        zoomTotal = Mathf.Clamp(zoomTotal, minZoom, maxZoom);
        
        Vector3 newZoomPos =  mainCam.transform.position + (mainCam.transform.forward * zoomAmt);

        if(zoomTotal > minZoom  && zoomTotal < maxZoom )
            mainCam.transform.position = newZoomPos;
        
        

    }
}
