using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [Header("Movement")]
    private float moveZ;
    private float speed;
    public float jumpForce;
    public float gravity;

    public float currentSpeed;
    private float inicialSpeed;
    public float freeFallSpeed;
    public float sprintSpeed;
    public float sprintTime;
    public float maxSprintTime;

    public float maxJetpack;
    public float jetpackFuel;
    public float jetpackConsumption;
    private bool jetpackActivo = false;
    private bool jumping;
    private bool freeFall;
    public float Move_Z;

    public float timeJetpack;
    private bool hasStartedJumping;
    public float timeToFreeFall;
    private bool wasFreeFall = false;


    [Header("Look")]
    public float sensitivity;
    private float xRotation;
    private float yRotation;
    public GameObject cam;

    [Header("Objects")]
    //public Image sprintBar;
    //public Image sprintBar2;
    //public Animator sprintAnim;
    //public Animator sprintAnim2;
    private bool dissapear;
    public Animator anim;
    public ParticleSystem[] jetPackEffect;
    public ParticleSystem dirt;
    public GameObject weaponHolder;


    [Header("Audio")]
    public AudioSource jetPack;
    public AudioSource freeFallWind;
    public AudioSource stomp;

    [Header("Raycast")]
    private Outline activeOutline;
    public TextMeshProUGUI text;

    private void Start()
    {
        jetPack.Play();
        jetPack.Pause();   
        freeFallWind.Play();  
        speed = currentSpeed;
        inicialSpeed = currentSpeed;
        jetpackFuel = maxJetpack;
        sprintTime = maxSprintTime;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();
        Sprint();
        Look();
        Raycast();
    }

    private void Movement()
    {
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
     
        
        moveZ += (gravity * Time.deltaTime );
        Move_Z = moveZ;
        freeFallWind.volume = -moveZ * 25;

        if (controller.isGrounded)
        {
            if (moveZ < 0 && Input.GetKey(KeyCode.Space) == false) { moveZ = -0.00005f; }
        }
        if (Input.GetKey(KeyCode.Space) && jetpackFuel > 0)
        {
            moveZ += jumpForce * Time.deltaTime;
            moveZ = Mathf.Clamp(moveZ, -1000, 50);

            anim.SetBool("IsJumping", true);
            jumping = true;

            jetPack.UnPause();
            

            if(hasStartedJumping== false) 
            {
                timeJetpack = 0;
                hasStartedJumping = true; 
            }   
            timeJetpack += Time.deltaTime;
            
        }
        if (Input.GetKey(KeyCode.Space) == false)
        {
            anim.SetBool("IsJumping", false);
            jumping = false;
            hasStartedJumping= false;
            jetPack.Pause();
        }


        if (timeJetpack > timeToFreeFall && controller.isGrounded == false && jumping == false || moveZ < -5f)
        {
            anim.SetBool("FreeFall", true);
            freeFall = true;
        }
        else if(timeJetpack < timeToFreeFall || controller.isGrounded)
        {
            anim.SetBool("FreeFall", false);
            freeFall = false;
        }
        if (wasFreeFall && !freeFall && controller.isGrounded )
        {
            Debug.Log("Impacto contra el suelo");
            dirt.Play();       
            stomp.Play();
        }
        wasFreeFall = freeFall;
        if (controller.isGrounded) timeJetpack = 0;

        bool quiereActivar = Input.GetKey(KeyCode.Space) && jetpackFuel > 0;
        if (quiereActivar && !jetpackActivo)
        {
            for (int i = 0; i < jetPackEffect.Length; i++)
            {
                jetPackEffect[i].Play();
            }            
            jetpackActivo = true;
        }
        else if (!quiereActivar && jetpackActivo)
        {
            for (int i = 0; i < jetPackEffect.Length; i++)
            {
                jetPackEffect[i].Stop();
            }
            jetpackActivo = false;
        }

        controller.Move((transform.forward * moveY + transform.right * moveX + Vector3.up * moveZ * Time.timeScale * Time.deltaTime));
        if (moveX != 0 || moveY != 0)
        {
            anim.SetBool("IsRunning", true);
        }
        else anim.SetBool("IsRunning", false);

        if (freeFall)
        {
            currentSpeed = freeFallSpeed;
        }else currentSpeed = inicialSpeed;

    }
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
         

        xRotation -= mouseY;
        yRotation -= mouseX;

        
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        //cam.transform.localRotation = Quaternion.Euler(Vector3.right * xRotation);
        Quaternion targetRotation = Quaternion.Euler(Vector3.right * xRotation);
        cam.transform.localRotation= Quaternion.Lerp(cam.transform.localRotation, targetRotation, Time.deltaTime * 28f);

        Quaternion targetRotation2= Quaternion.Euler(Vector3.up * -yRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation2, Time.deltaTime * 28f);

    }
    private void Sprint()
    {

        if (Input.GetKey(KeyCode.LeftShift) && freeFall == false)
        {
            dissapear = false;
            if (sprintTime > 0)
            {
                sprintTime -= Time.deltaTime;
                speed = sprintSpeed;
            }
            else speed = currentSpeed;

        }
        else
        {
            speed = currentSpeed;
            sprintTime += Time.deltaTime;
            dissapear = false;
            if (sprintTime > maxSprintTime)
            {
                dissapear = true;
                sprintTime = maxSprintTime;
            }
        }
        //sprintBar.fillAmount = sprintTime / maxSprintTime;
        //sprintBar2.fillAmount = sprintTime / maxSprintTime;

        //if (dissapear)
        //{
        //sprintAnim.SetBool("Appear", false);
        //sprintAnim2.SetBool("Appear", false);
        //}
        //else
        //{
        //sprintAnim.SetBool("Appear", true);
        //sprintAnim2.SetBool("Appear", true);
        //}
    }

    IEnumerator enteredJetpack()
    {
        yield return new WaitForSeconds(speed);
    }

    public Weapon GetActiveWeapon() 
    {
        for (int i = 0; i < weaponHolder.transform.childCount; i++) 
        {
            GameObject activeChild = weaponHolder.transform.GetChild(i).gameObject;
            if (activeChild.activeSelf)
            {
                return activeChild.transform.GetChild(0).GetComponent<Weapon>();
            }
        }
        return null;
       
    }

    private void Raycast()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3))
        {
            if(hit.collider.GetComponent<Item>() != null)
            {
                activeOutline = hit.collider.GetComponent<Item>().GetOutline();
                text.text = hit.collider.GetComponent<Item>().GetText();
                text.enabled = true;

                activeOutline.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.GetComponent<Item>().Interact();   
                }
            }
            else
            {
                if(text != null)
                    text.enabled=false;
                if(activeOutline != null)
                    activeOutline.enabled = false;
            }
        }
        else
        {
            if (text != null)
                text.enabled = false;
            if (activeOutline != null)
                activeOutline.enabled = false;
        }
        Debug.DrawRay(ray.origin, ray.direction * 3, Color.red);
    }

}
