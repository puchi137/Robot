using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class CameraController : MonoBehaviour
{
    [Header("CameraMovement")]
    [SerializeField]
    private float duration;

    private int zone;
    private bool isChangingPos;


    [Header("Parallax")]
    public float intensity = 2f;
    public float smooth = 5f;

    private Vector3 basePosition;
    private Vector3 targetPos;

   
    [Header("Objects")]
    public GameObject menu;
    public GameObject inventoryPanel;
    private bool activated = false;
    private bool activation = false;
    public PlayerMovement player;
    
    public Transform playPosition;
    private void Start()
    {
        basePosition = transform.localPosition;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (isChangingPos == false)
        {
            Parallax();
        }
        
        if(SceneManager.GetActiveScene().name != "Menu") 
        {
            if(Input.GetKeyDown(KeyCode.Escape) && activated==false) 
            {
                activation = !activation;
            }
            menu.SetActive(activation);
            if(activation && activated== false)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                player.GetActiveWeapon().isAbleToShoot = false;
            }
            else if (activated && activation == false)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                player.GetActiveWeapon().isAbleToShoot = false;
            }
            else if(activation == false &&  activated == false) 
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                player.GetActiveWeapon().isAbleToShoot = true;
            }


            inventoryPanel.SetActive(activated);
            if (Input.GetKeyDown(KeyCode.Tab) && activation==false)
            {
                activated = !activated;
            }
            

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            activated = true;
            inventoryPanel.SetActive(activated);
        }
        

        
        
    }
    public void LookAt(Transform target)
    {  
        //transform.DOLookAt(target.position, duration);
        StartCoroutine(changePosition(target));
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Zone1");
        // + zone.ToString();
    }
    public void zoneSelect(int zoneNumber)
    {
        zone = zoneNumber;
    }
    public void MainMenu()
    {
        activation =false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    IEnumerator changePosition(Transform position)
    {
        isChangingPos = true;
        basePosition = position.position;
        float time = 0f;
        float duration = 1f;

        Vector3 startPos = transform.position;
        Vector3 endPos = position.position;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(position.forward);

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            transform.position = Vector3.Slerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }
        isChangingPos = false;
    }
    private void Parallax()
    {
        float mouseX = Input.mousePosition.x / Screen.width - 0.5f;
        float mouseY = Input.mousePosition.y / Screen.height - 0.5f;

        Vector3 offset = (transform.right * mouseX + transform.up * mouseY) * intensity;

        targetPos = basePosition + offset;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smooth);
    }
}

