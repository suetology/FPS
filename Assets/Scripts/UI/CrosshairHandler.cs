using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairHandler : MonoBehaviour
{
    private static CrosshairHandler instance;
    public static CrosshairHandler GetInstance() => instance;

    private GameObject crosshair;

    private RectTransform topPart;
    private RectTransform bottomPart;
    private RectTransform rightPart;
    private RectTransform leftPart;

    private float position = 30f;
    private float maxPosition = 120f;
    private float returnSpeed = 150f;

    private void Awake()
    {
        instance = this;

        crosshair = transform.GetChild(0).gameObject;
        //topPart = transform.GetChild(0).GetComponent<RectTransform>();
        //bottomPart = transform.GetChild(1).GetComponent<RectTransform>();
        //rightPart = transform.GetChild(2).GetComponent<RectTransform>();
        //leftPart = transform.GetChild(3).GetComponent<RectTransform>();
    }

    public void Update()
    {
        //ReturnParts();
    }
    public void OnShot()
    {
        MoveParts(60f);
    }

    public void HideCrosshair()
    {
        crosshair.SetActive(false);
    }

    public void ShowCrosshair()
    {
        crosshair.SetActive(true);
    }

    private void MoveParts(float distance)
    {
        if (rightPart.localPosition.x + distance > maxPosition)
            distance = maxPosition;
        else
            distance = rightPart.localPosition.x + distance;
        rightPart.localPosition = new Vector3(distance,
                                              rightPart.localPosition.y,
                                              rightPart.localPosition.z);
        leftPart.localPosition = new Vector3(-distance,
                                             leftPart.localPosition.y,
                                             leftPart.localPosition.z);
        topPart.localPosition = new Vector3(topPart.localPosition.x,
                                            distance,
                                            topPart.localPosition.z);
        bottomPart.localPosition = new Vector3(bottomPart.localPosition.x,
                                             -distance,
                                             bottomPart.localPosition.z);
    }

    private void ReturnParts()
    {
        ReturnTopPart();
        ReturnBottomPart();
        ReturnRightPart();
        ReturnLeftPart();
    }
    private void ReturnTopPart()
    {
        if(topPart.localPosition.y > position)
        {
            topPart.localPosition = new Vector3(topPart.localPosition.x,
                                                topPart.localPosition.y - returnSpeed * Time.deltaTime,
                                                topPart.localPosition.z);
        }
    }
    private void ReturnBottomPart()
    {
        if (bottomPart.localPosition.y < -position)
        {
            bottomPart.localPosition = new Vector3(bottomPart.localPosition.x,
                                                bottomPart.localPosition.y + returnSpeed * Time.deltaTime,
                                                bottomPart.localPosition.z);
        }
    }
    private void ReturnRightPart()
    {
        if (rightPart.localPosition.x > position)
        {
            rightPart.localPosition = new Vector3(rightPart.localPosition.x - returnSpeed * Time.deltaTime,
                                                rightPart.localPosition.y,
                                                rightPart.localPosition.z);
        }
    }
    private void ReturnLeftPart()
    {
        if (leftPart.localPosition.x < -position)
        {
            leftPart.localPosition = new Vector3(leftPart.localPosition.x + returnSpeed * Time.deltaTime,
                                                leftPart.localPosition.y,
                                                leftPart.localPosition.z);
        }
    }
}
