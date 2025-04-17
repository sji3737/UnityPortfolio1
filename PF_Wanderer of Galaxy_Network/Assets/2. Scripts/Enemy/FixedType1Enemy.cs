using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedType1Enemy : Enemy
{
    [SerializeField] Transform _firePos;

    MeshRenderer[] _renChild = new MeshRenderer[3];
    EnemyIndicator _indicator;
    GameObject _prefabIndicatorObj;

    bool _isOffScreen;

    protected override void Awake()
    {
        base.Awake();
        _hp = 100;
        _type = PublicDefine.eEnemyType.FIXED1;
        _bulletSpeed = 3000;
        _fireRateTime = 2f;
        _bullet = Resources.Load<GameObject>("Prefabs/EnemyBullet");
        _prefabIndicatorObj = Resources.Load<GameObject>("Prefabs/EnemyIndicator");
        _state = PublicDefine.eEnemyState.IDLE;
        for (int i = 0; i < 3; i++)
        {
            _renChild[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
        }
        _isOffScreen = false;
        _score = 100;
    }

    private void Update()
    {
        switch (_state)
        {
            case PublicDefine.eEnemyState.IDLE:
                if(Vector3.Distance(_player.gameObject.transform.position, transform.position) < 75)
                {
                    _state = PublicDefine.eEnemyState.ATTACK;
                }
                break;
            case PublicDefine.eEnemyState.ATTACK:
                Rotate();
                Attack();
                break;
        }
        CheckOffScreen();
    }
    protected override void Attack()
    {
        Vector3 playerPos = _player.transform.position;
        Vector3 dir = playerPos - transform.position;
        Quaternion qDir = Quaternion.LookRotation(dir);
        if (!_isFire)
        {
            StartCoroutine(FireBullet());
        }       
    }
    protected override void Rotate()
    {
        base.Rotate();
        Vector3 playerPos = _player.transform.position;
        Quaternion goalAngle = Quaternion.LookRotation(playerPos - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, goalAngle, Time.deltaTime * 400);
        if (Vector3.Distance(_player.gameObject.transform.position, transform.position) > 75)
        {
            _state = PublicDefine.eEnemyState.IDLE;
        }
    }

    public override void SetLockOnColor(Color c)
    {
        for (int i = 0; i < 3; i++)
        {
            _renChild[i].material.SetColor("_TexColor", c);
        }
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

    void CheckOffScreen()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if ((screenPos.x > 0 && screenPos.x < Camera.main.pixelWidth) && (screenPos.y > 0 && screenPos.y < Camera.main.pixelHeight) && screenPos.z > 0)
        {
            _isOffScreen = false;
            if (_indicator != null)
            {
                _indicator.StopIndicate(false);
            }
        }
        else
        {
            if (!_isOffScreen)
            {
                if (_indicator == null)
                {
                    GameObject root = GameObject.FindGameObjectWithTag("EnemyIndicatorRoot");
                    _indicator = Instantiate(_prefabIndicatorObj, root.transform).GetComponent<EnemyIndicator>();
                }
                _indicator.StartIndicate(true, this.gameObject.transform);
            }
            _isOffScreen = true;
        }
    }

    IEnumerator FireBullet()
    {
        _isFire = true;        
        GameObject go = Instantiate(_bullet, _firePos.position, Quaternion.identity);
        Bullet b = go.GetComponent<Bullet>();
        b.BulletInit(_bulletSpeed, 10f, 10f, transform.forward);
        yield return new WaitForSeconds(_fireRateTime);
        _isFire = false;
    }

    IEnumerator SetBulletHitShaderEffect()
    {
        Color hitColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        for (int i = 0; i < 3; i++)
        {
            _renChild[i].material.SetColor("_TexColor", hitColor);
            yield return new WaitForSeconds(0.07f);
            _renChild[i].material.SetColor("_TexColor", Color.black);
            yield return new WaitForSeconds(0.07f);
            _renChild[i].material.SetColor("_TexColor", hitColor);
            yield return new WaitForSeconds(0.07f);
            _renChild[i].material.SetColor("_TexColor", Color.black);
        }
             
    }
}
