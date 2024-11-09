using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPCamera : MonoBehaviour, IOrderOfRunningStart
{
    [SerializeField]
    Joystick joystick;
    [SerializeField]
    Transform theWall;
    [SerializeField]
    GameObject camSight;
    Camera thisCam;

    private void OnEnable()
    {
        camSight.SetActive(true);
        thisCam = GetComponent<Camera>();
    }

    private void OnDisable()
    {
        camSight.SetActive(false);
    }

    Quaternion thisRotation;
    public void Init()
    {
        transform.position = theWall.position - Vector3.forward * 15;
        thisRotation = transform.rotation;
    }
    private void Update()
    {
        CaculateAxis();
        MoveSight();
        Shooting();
        KickBack();
    }

    Vector3 mousePos, axis;
    void CaculateAxis()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 0.02f;
        mousePos = thisCam.ScreenToWorldPoint(mousePos);
        axis = mousePos - transform.position;
        axis.z = 0;
        axis = axis.normalized;
    }

    Vector3 joystickMove;
    void MoveSight()
    {
        if (Application.platform != RuntimePlatform.Android)
            transform.position += axis * 7.5f * Time.deltaTime;
        else
        {
            joystickMove = new Vector3(joystick.Horizontal, joystick.Vertical, 0);
            transform.position += joystickMove * 7.5f * Time.deltaTime;
        }
    }

    Ray shootingRay;
    RaycastHit hit;
    [SerializeField]
    LayerMask zombieLayerMask;

    void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shootingRay = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(shootingRay, out hit, 115, zombieLayerMask) && GameManager.instance.weaponAmount > 0)
            {
                CommunicateManager.instance.CanDieThing(hit.collider.gameObject)?.Die();
            }
            if (GameManager.instance.weaponAmount > 0)
                GameManager.instance.DecreaseValidDamage();
            kick = true;
        }
    }
    bool kick = false;
    float eTime = 0;

    void KickBack()
    {
        if (kick)
        {
            if (eTime < 0.1f)
            {
                transform.rotation *= Quaternion.Euler(-35 * Time.deltaTime, 0f, 0f);
                eTime += Time.deltaTime;
            }
            else if (eTime < 0.25f)
            {
                transform.rotation *= Quaternion.Euler(70 / 3 * Time.deltaTime, 0f, -0f);
                eTime += Time.deltaTime;
            }
            else
            {
                transform.rotation = thisRotation;
                eTime = 0;
                kick = false;
            }
        }
    }
}
