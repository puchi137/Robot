using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float swayIntensity;
    public float swaySpeed;  
    public float positionSpeed;
    
    public Transform weaponPos;
    public Transform cameraHolder;
    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayIntensity;
        float mouseY = Input.GetAxis("Mouse Y") * swayIntensity;

        Quaternion quaternionX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion quaternionY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion combination = quaternionX * quaternionY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, combination, swaySpeed * Time.deltaTime);
        transform.forward = cameraHolder.forward;


        transform.position = Vector3.Slerp(transform.position, weaponPos.position, positionSpeed * Time.deltaTime);
    }
}
