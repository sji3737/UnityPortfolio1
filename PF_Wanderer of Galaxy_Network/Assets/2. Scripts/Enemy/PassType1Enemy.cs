using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassType1Enemy : Enemy
{
    Rigidbody _rdBody;
    Vector3 _startAngle;
    MeshRenderer _mRenderer;
    protected override void Awake()
    {
        base.Awake();
        _hp = 50;
        _type = PublicDefine.eEnemyType.PASS;
        _rdBody = GetComponent<Rigidbody>();
        _startAngle = transform.eulerAngles;
        _mRenderer = GetComponentInChildren<MeshRenderer>();
        _state = PublicDefine.eEnemyState.IDLE;
        _score = 50;
    }

    void Update()
    {
        Rotate();
        Move();
    }

    public override void SetLockOnColor(Color c)
    {
        _mRenderer.material.SetColor("_TexColor", c);
    }

    protected override void Rotate()
    {
        float sinY = Mathf.Sin(Time.time) * 50;
        Vector3 currentRotate = transform.eulerAngles;
        currentRotate.y = _startAngle.y + sinY;
        transform.eulerAngles = currentRotate;
    }

    public override void Damaged(int damage)
    {
        if (_hp <= 0 && _state != PublicDefine.eEnemyState.DEAD)
        {
            Dead();
        }
        else
        {
            StartCoroutine(SetBulletHitShaderEffect());
            _hp -= damage;
        }
    }

    private void Move()
    {
        _rdBody.velocity = transform.forward * 25;
    }

    IEnumerator SetBulletHitShaderEffect()
    {
        Color hitColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        _mRenderer.material.SetColor("_TexColor", hitColor);
        yield return new WaitForSeconds(0.07f);
        _mRenderer.material.SetColor("_TexColor", Color.black);
        yield return new WaitForSeconds(0.07f);
        _mRenderer.material.SetColor("_TexColor", hitColor);
        yield return new WaitForSeconds(0.07f);
        _mRenderer.material.SetColor("_TexColor", Color.black);
    }
}
