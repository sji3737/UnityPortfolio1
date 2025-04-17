using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    bool _isWork;
    Transform _indicateEnemyTransform;
    RectTransform _rectTransform;

    private void Awake()
    {
        gameObject.SetActive(false);
        _rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (_isWork)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(_indicateEnemyTransform.position);
            Vector3 screenCenter = new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0) / 2;

            if (screenPos.z < 0)
            {
                screenPos *= -1;
            }

            screenPos -= screenCenter;

            float angle = Mathf.Atan2(screenPos.y, screenPos.x);
            angle -= 90 * Mathf.Deg2Rad;

            float cos = Mathf.Cos(angle);
            float sin = -Mathf.Sin(angle);

            screenPos = screenCenter + new Vector3(sin * 150, cos * 150, 0);

            float m = cos / sin;
            Vector3 screenBounds = screenCenter * 0.9f;
            if (cos > 0)
            {
                screenPos = new Vector3(screenBounds.y / m, screenBounds.y, 0);
            }
            else
            {
                screenPos = new Vector3(-screenBounds.y / m, -screenBounds.y, 0);
            }

            if (screenPos.x > screenBounds.x)
            {
                screenPos = new Vector3(screenBounds.x, screenBounds.x * m, 0);
            }
            else if (screenPos.x < -screenBounds.x)
            {
                screenPos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
            }

            //screenPos += screenCenter;

            _rectTransform.localPosition = screenPos * 0.9f;
            transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + 90);
        }
    }

    public void StartIndicate(bool work, Transform indicateObj)
    {
        gameObject.SetActive(true);
        _isWork = work;
        _indicateEnemyTransform = indicateObj;

    }

    public void StopIndicate(bool work)
    {
        gameObject.SetActive(false);
        _isWork = work;
    }
}
