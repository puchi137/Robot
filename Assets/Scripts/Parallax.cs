using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float intensity = 2f;
    public float smooth = 5f;

    private Vector3 basePosition; 
    private Vector3 targetPos;

    void Start()
    {
        basePosition = transform.localPosition; 
    }

    void Update()
    {
        float mouseX = Input.mousePosition.x / Screen.width - 0.5f;
        float mouseY = Input.mousePosition.y / Screen.height - 0.5f;

        Vector3 offset = new Vector3(mouseX * intensity, mouseY * intensity, 0);

        targetPos = basePosition + offset;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smooth);
    }
}
