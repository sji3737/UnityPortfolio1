using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHairControl : MonoBehaviour
{
    RectTransform targetRectTr;
    Image _crossHairImg;

    private void Awake()
    {
        targetRectTr = GetComponent<RectTransform>();
        _crossHairImg = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        targetRectTr.position = Input.mousePosition;
    }


    public void SetLockOnMode()
    {
        _crossHairImg.color = new Color(1, 0, 0);
    }

    public void CancelLockOnMode()
    {
        _crossHairImg.color = new Color(1, 1, 1);
    }
}
