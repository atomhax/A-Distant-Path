using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControls : MonoBehaviour {
    public Camera cam;

    public float zoomSpeed = 2.0f;
    //private Vector3 zoomVelocity = Vector3.zero;
    public float smoothTime = 1.0f;
    public float maxZoom = 10.0f;
    public float minZoom = 1.0f;

    public Vector3 camPos = new Vector3(0, 0, -12f);

    public GameObject character;
    public GameObject pivot;
    float dragFactor = 3.0f;

    private float rotateSpeed = 125.0f;
    private float rotateThreshold = 0.2f;

    Vector3 turnDir = Vector3.zero;

    public static int type = 0;

    public Material SkyBoxMat;

    private float x = 45;
    private float y = 35;

    // Use this for initialization
    void Start() {
        //height = character.GetComponent<CapsuleCollider>().height;

        cam = pivot.GetComponentInChildren<Camera>();        
        //x = pivot.transform.rotation.y * 360 / Mathf.PI;
        //y = pivot.transform.rotation.x * 360 / Mathf.PI;
        //pivot.transform.Rotate(new Vector3(45f, 0, 0));
        cam.gameObject.transform.localPosition = camPos;

        character.GetComponent<PlayerControls>().setPivotPoint(pivot);

        pivot.transform.rotation = Quaternion.Euler(new Vector3(y, x, 0));
    }

    private float targetAngle;

    // Update is called once per frame
    void Update() {        
        //Camera Displacement
        Vector3 distance = character.transform.position - transform.position;
        if (distance.sqrMagnitude > 0) {
            if (distance.sqrMagnitude < 0.0001) {
                transform.position = character.transform.position;
            } else {
                Vector3 pos = Vector3.Lerp(transform.position, character.transform.position, Time.deltaTime * dragFactor);
                pos.y = character.transform.position.y;
                transform.position = pos;
            }
        }


        //Camera Rotation
        switch (type) {
            case 0:
                if(Input.GetAxis("RightTrigger") <= 0.9) {
                    turnDir.x = Input.GetAxis("RightJoystickHorizontal");
                    turnDir.y = Input.GetAxis("RightJoystickVertical");

                    if (turnDir.sqrMagnitude >= rotateThreshold) {
                        if (turnDir.x >= rotateThreshold || turnDir.x <= -rotateThreshold) {
                            x = Mathf.LerpAngle(x, turnDir.x * rotateSpeed + x, Time.deltaTime);
                        }

                        if (turnDir.y >= rotateThreshold || turnDir.y <= -rotateThreshold) {
                            y = Mathf.Lerp(y, turnDir.y * rotateSpeed + y, Time.deltaTime);
                        }
                        y = Mathf.Clamp(y, 30, 80);
                        pivot.transform.rotation = Quaternion.Euler(new Vector3(y, x, 0));
                    }
                }
                break;
            case 1:
                /*if (DPadButtons.leftOnPressed) {
                    targetAngle += 90;
                    horAxisUsed = true;
                } else if (DPadButtons.rightOnPressed) {
                    targetAngle -= 90;
                    verAxisUsed = true;
                } else {
                    horAxisUsed = false;
                    verAxisUsed = false;
                }

                if (Mathf.Abs(targetAngle - x) < 0.001) {
                    x = targetAngle;
                }

                if (x != targetAngle) {
                    x = Mathf.Lerp(x, targetAngle, Time.deltaTime * 4);
                    pivot.transform.rotation = Quaternion.Euler(new Vector3(30, x, 0));
                }
                */
                break;
        }
    }

    public void setCharacter(GameObject character) {
        this.character = character;
    }

    public void setCamera(Camera cam) {
    }
}
