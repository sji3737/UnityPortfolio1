using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightType1Enemy : Enemy
{
    [SerializeField] GameObject _firePos;
    Rigidbody _rdBody;
    MeshRenderer _mRenderer;
    EnemyIndicator _indicator;
    GameObject _prefabIndicatorObj;
    Vector3 _dodgePoint;
    Quaternion _goalDodgeAngle;
    float _rotateSpeed;
    float _moveSpeed;
    bool _isDodge;
    bool _isOffScreen;

    protected override void Awake()
    {
        base.Awake();
        _hp = 100;
        _bulletSpeed = 3000;
        _fireRateTime = 1.5f;
        _bullet = Resources.Load<GameObject>("Prefabs/EnemyBullet");
        _prefabIndicatorObj = Resources.Load<GameObject>("Prefabs/EnemyIndicator");
        _state = PublicDefine.eEnemyState.IDLE;
        _rotateSpeed = 100f;
        _moveSpeed = 30f;
        _type = PublicDefine.eEnemyType.FIGHT1;
        _rdBody = GetComponent<Rigidbody>();
        _mRenderer = GetComponent<MeshRenderer>();
        _isDodge = false;
        _isFire = false;
        _isOffScreen = false;
        _score = 100;
    }
    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case PublicDefine.eEnemyState.IDLE:
                if (Vector3.Distance(transform.position, _player.transform.position) < 100)
                {
                    _state = PublicDefine.eEnemyState.PlayerTrack;
                }
                break;
            case PublicDefine.eEnemyState.PlayerTrack:
                PlayerTracking();
                Move();
                if (Vector3.Distance(transform.position, _player.transform.position) < 50)
                {
                    _state = PublicDefine.eEnemyState.ATTACK;
                }
                break;
            case PublicDefine.eEnemyState.ATTACK:
                Move();
                PlayerTracking();
                Attack();
                if (Vector3.Distance(transform.position, _player.transform.position) < 30)
                {
                    _state = PublicDefine.eEnemyState.DODGE;
                }
                break;
            case PublicDefine.eEnemyState.DODGE:
                Move();
                Dodge();
                if (Vector3.Distance(transform.position, _player.transform.position) > 40)
                {
                    _state = PublicDefine.eEnemyState.ATTACK;
                }
                break;
        }
        CheckOffScreen();
    }

    void PlayerTracking()
    {
        Quaternion goalAngle = Quaternion.LookRotation(_player.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, goalAngle, Time.deltaTime * _rotateSpeed);
    }

    void Dodge()
    {
        if (!_isDodge)
        {
            StartCoroutine(SetDodgePoint());
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _goalDodgeAngle, Time.deltaTime * _rotateSpeed);
    }

    void Move()
    {
        _rdBody.velocity = transform.forward * _moveSpeed;
    }

    void CheckOffScreen()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if ((screenPos.x > 0 && screenPos.x < Camera.main.pixelWidth) && (screenPos.y > 0 && screenPos.y < Camera.main.pixelHeight) && screenPos.z > 0)
        {
            _isOffScreen = false;
            if(_indicator != null)
            {
                _indicator.StopIndicate(false);
            }            
        }
        else
        {
            if (!_isOffScreen)
            {
                if(_indicator == null)
                {
                    GameObject root = GameObject.FindGameObjectWithTag("EnemyIndicatorRoot");
                    _indicator = Instantiate(_prefabIndicatorObj, root.transform).GetComponent<EnemyIndicator>();
                }
                _indicator.StartIndicate(true, this.gameObject.transform);
            }
            _isOffScreen = true;
        }
    }

    protected override void Attack()
    {
        if (!_isFire)
        {
            StartCoroutine(SetBulletFire());
        }
    }

    public override void SetLockOnColor(Color c)
    {
        _mRenderer.material.SetColor("_TexColor", c);
    }

    public override void Damaged(int damage)
    {
        if (_hp <= 0 && _state != PublicDefine.eEnemyState.DEAD)
        {           
            InGameManager._Instance.EnemyKill();
            if(_indicator != null)
            {
                Destroy(_indicator.gameObject);
            }         
            Dead();
        }
        else
        {
            StartCoroutine(SetBulletHitShaderEffect());
            _hp -= damage;
        }
    }

    protected override void RemoveInScene()
    {
        base.RemoveInScene();
        if (_indicator != null)
        {
            Destroy(_indicator.gameObject);
        }
    }

    IEnumerator SetDodgePoint()
    {
        _isDodge = true;
        float ranForwardVal = Random.Range(5, 25);
        _dodgePoint = -_player.transform.forward * ranForwardVal;
        float ranRightVal = Random.Range(-45, 45);
        _dodgePoint = _player.transform.right * ranRightVal;
        _goalDodgeAngle = Quaternion.LookRotation(_dodgePoint - transform.position);
        yield return new WaitForSeconds(2.0f);
        _isDodge = false;
    }

    IEnumerator SetBulletFire()
    {
        _isFire = true;
        GameObject go = Instantiate(_bullet, _firePos.transform.position, Quaternion.identity);
        Bullet b = go.GetComponent<Bullet>();
        b.BulletInit(_bulletSpeed, 10f, 10f, transform.forward);
        yield return new WaitForSeconds(_fireRateTime);
        _isFire = false;
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
