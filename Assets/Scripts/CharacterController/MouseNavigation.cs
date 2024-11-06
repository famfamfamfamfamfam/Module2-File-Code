﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseNavigation : MonoBehaviour
{
    [SerializeField]
    Transform head;
    float mouseOnX;
    float rotateSpeed = 180;

    void SetUpView()
    {
        transform.position = head.position - head.forward * 3.5f + head.up * 2;
        transform.rotation = Quaternion.LookRotation(head.position - transform.position);
    }

    void MouseNavigate()
    {
        transform.RotateAround(head.position, head.up, mouseOnX * rotateSpeed * Time.deltaTime);
    }
    void UpdateDirection()
    {
        head.Rotate(0, mouseOnX * rotateSpeed * Time.deltaTime, 0);
    }

    private void LateUpdate()
    {
        SetUpView();
        if (Application.platform == RuntimePlatform.Android) return;
        mouseOnX = Input.GetAxis("Mouse X");
        MouseNavigate();
        UpdateDirection();


        //head.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime, 0);
        //transform.position = head.position - head.forward * 3.5f + head.up * 2 + head.rotation * Vector3.forward;
        //transform.rotation = Quaternion.LookRotation(head.position - transform.position);
    }

}
