using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedType2Enemy : Enemy
{
    [SerializeField] GameObject _head;
    [SerializeField] Transform _lFirePos;
    [SerializeField] Transform _rFirePos;
    MeshRenderer[] _renChild = new MeshRenderer[2];

    EnemyIndicator _indicator;
    GameObject _prefabIndicatorObj;

    bool _isOffScreen;

    protected override void Awake()
    {
        base.Awake();
        _hp = 100;
        _type = PublicDefine.eEnemyType.FIXED2;
        _bulletSpeed = 3000;
        _fireRateTime = 2f;
        _bullet = Resources.Load<GameObject>("Prefabs/EnemyBullet");
        _prefabIndicatorObj = Resources.Load<GameObject>("Prefabs/EnemyIndicator");
        for (int i = 0; i < 2; i++)
        {
            _renChild[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
        }

        _isOffScreen = false;
        _score = 200;
    }

    private void Update()
    {
        switch (_state)
        {
            case PublicDefine.eEnemyState.IDLE:
                if (Vector3.Distance(_player.gameObject.transform.position, transform.position) < 75)
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

    protected override void Rotate()
    {
        base.Rotate();
        Vector3 playerPos = _player.transform.position;
        Quaternion goalAngle = Quaternion.LookRotation(playerPos - transform.position);
        _head.transform.rotation = Quaternion.RotateTowards(_head.transform.rotation, goalAngle, Time.deltaTime * 50);
        if (Vector3.Distance(_player.gameObject.transform.position, transform.position) > 75)
        {
            _state = PublicDefine.eEnemyState.IDLE;
        }
    }

    protected override void Attack()
    {
        if (!_isFire)
        {
            StartCoroutine(FireBullet());
        }
    }

    public override void SetLockOnColor(Color c)
    {
        for (int i = 0; i < 2; i++)
        {
            _renChild[i].material.SetColor("_TexColor", c);
        }
    }

    public override void Damaged(int damage)
    {
        if (_hp <= 0 && _state != PublicDefine.eEnemyState.DEAD)
        {                    
            InGameManager._Instance.EnemyKill();
            if (_indicator != null)
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
        for (int i = 0; i < 3; i++)
        {
            GameObject leftPos = Instantiate(_bullet, _lFirePos.position, Quaternion.identity);
            Bullet leftBullet = leftPos.GetComponent<Bullet>();
            leftBullet.BulletInit(_bulletSpeed, 10f, 10f, _head.transform.forward);
            GameObject rightPos = Instantiate(_bullet, _rFirePos.position, Quaternion.identity);
            Bullet rightBullet = rightPos.GetComponent<Bullet>();
            rightBullet.BulletInit(_bulletSpeed, 10f, 10f, _head.transform.forward);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(5.0f);
        _isFire = false;
    }

    IEnumerator SetBulletHitShaderEffect()
    {
        Color hitColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        for (int i = 0; i < 2; i++)
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
