﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TwoButtonBaseWindow : MonoBehaviour
{
    public virtual void ClickOkButton()
    {
        gameObject.SetActive(false);
    }

    public virtual void ClickCancleButton()
    {
        gameObject.SetActive(false);
    }
}
