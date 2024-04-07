using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Rigidbody rb;
    private bool isSprinting=false;
    public float runSpeed = 2f;
    public Camera MainCamera;
    public RawImage SprintingOverlay;
    public RawImage StaminaOverlay;
    public float MaxStamina =100;
    private float Stamina;
    public AudioSource walk;
    public AudioSource run;
    public bool interacted=false;
    public bool hidden=false;
    public GameObject startCamera;
    public GameObject playerCamera;
    public bool start=true;
    public bool end=false;
    public GameObject bob;
    public GameObject gob;
    public GameObject playerSpawn;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Stamina=MaxStamina;
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        playerCamera.transform.position=MainCamera.transform.position;
        playerCamera.transform.rotation=MainCamera.transform.rotation;
        MainCamera.transform.position=startCamera.transform.position;
        MainCamera.transform.rotation=startCamera.transform.rotation;
        run.enabled=false;
        walk.enabled=false;
        GameObject.Find("Confirm Button").GetComponent<ConfirmButton>().Generate();
        GameObject.Find("Player").GetComponent<Map>().hasMap=false;
    }
    void Update(){
        if (start==true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Reset Player
                transform.position=playerSpawn.transform.position;
                transform.rotation=playerSpawn.transform.rotation;
                isSprinting=false;
                Stamina=MaxStamina;
                interacted=false;
                hidden=false;
                start=false;
                end=false;
                GameObject.Find("Locked Door").GetComponent<LockedDoorInteractable>().hasKey=false;
                //Reset Doors
                GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
                foreach (var door in doors)
                {
                    door.GetComponent<DoorInteractable>().Restart();
                }
                GameObject.Find("Locked Door").GetComponent<LockedDoorInteractable>().Restart();
                //Reset Bob
                bob.transform.position=GameObject.Find("Bob").GetComponent<BobController>().start.position;
                GameObject.Find("Bob").GetComponent<BobController>().spottedTarget=false;
                GameObject.Find("Bob").GetComponent<BobController>().roamCooldown=0;
                GameObject.Find("Bob").GetComponent<BobController>().chaseTimer=0;
                //Reset Gob
                gob.transform.position=GameObject.Find("Gob").GetComponent<BobController>().start.position;
                GameObject.Find("Gob").GetComponent<BobController>().spottedTarget=false;
                GameObject.Find("Gob").GetComponent<BobController>().roamCooldown=0;
                GameObject.Find("Gob").GetComponent<BobController>().chaseTimer=0;
                //START
                UnityEngine.Cursor.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale=1;
                MainCamera.transform.position=playerCamera.transform.position;
                MainCamera.transform.rotation=playerCamera.transform.rotation;
            }
        }else if(end==true){
            if (Input.GetKeyDown(KeyCode.E))
            {
                start=true;
                end=false;
                MainCamera.transform.position=startCamera.transform.position;
                MainCamera.transform.rotation=startCamera.transform.rotation;
            }
        }else if (GameObject.Find("Player").GetComponent<Pause>().isPaused==false&&start==false&&end==false)        //If the game isnt paused and player isnt interacted does the thing
        {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Sprint on");
            isSprinting=true;
            MainCamera.fieldOfView=70f;
            SprintingOverlay.enabled=true;


        };
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("Sprint off");
            isSprinting=false;
            MainCamera.fieldOfView=60f;
            SprintingOverlay.enabled=false;
        };
        StaminaOverlay.transform.localScale = new Vector3 (1.05f+(Stamina/100),1.05f+(Stamina/100),1);
    }
        }
        
    private void FixedUpdate()
    {
        if (interacted==false&&start==false&&GameObject.Find("Player").GetComponent<Pause>().isPaused==false&&end==false)
        {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 verticalMovement = transform.forward * moveSpeed * verticalInput;
        Vector3 horizontalMovement = transform.right * moveSpeed * horizontalInput;
        
        if (verticalInput>0.1||horizontalInput>0.1||verticalInput<-0.1||horizontalInput<-0.1)
        {
            if (isSprinting==true)
            {
                walk.enabled=false;
                run.enabled=true;
            }else
            {
                run.enabled=false;
                walk.enabled=true;
            }
        }else
        {
            walk.enabled=false;
            run.enabled=false;
        }
        
        if (isSprinting==true)
        {
            if (Stamina>0)
            {
                horizontalMovement *= runSpeed;
                verticalMovement *= runSpeed;
            }else
            {
                Debug.Log("Sprint off");
                isSprinting=false;
                MainCamera.fieldOfView=60f;
                SprintingOverlay.enabled=false;
                run.enabled=false;
                walk.enabled=true;
            }
        }

        if (isSprinting==true)
        {
            Stamina-=0.3f;
        }else
        {
            if (Stamina<MaxStamina)
            {
                Stamina+=0.1f;
            }
        }

        Vector3 movement = verticalMovement + horizontalMovement;

        //rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);        old bad movePosition
        rb.velocity = movement;                                             //  good new velocity
        }
        
    }
}