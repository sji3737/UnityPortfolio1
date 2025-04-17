using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public enum ePlayerState
    {
        Ready = 0,
        Play,
        End
    }
    [SerializeField] GameObject _prefabBullet;
    [SerializeField] GameObject _prfabMissile;
    [SerializeField] GameObject _firePos;
    [SerializeField] GameObject _missileFirePos;
    RectTransform _crossHairRTR;
    Rigidbody _rdBody;
    Enemy _lockEnemy;
    ePlayerState _state;
    float _playerMaxHP;
    float _playerHP;
    float _baseWeaponAtk;
    float _missileWeaponAtk;
    float _maxVelocity;
    float _accelateValue;
    float _rotateSpeed;
    float _fireRateSpeed;
    float _missileFireDelay;
    float _missileFireDelayCnt;
    bool _isFire;
    bool _isMissileReady;
    bool _isMissileDelayEnd;
    // Start is called before the first frame update
    void Start()
    {
        _crossHairRTR = GameObject.FindGameObjectWithTag("CrossHair").GetComponent<RectTransform>();
        _rdBody = GetComponent<Rigidbody>();
        int baseHp = NetworkGameDataManager._Instance.GetBaseHp();
        int hpUpgrade = NetworkGameDataManager._Instance.GetHpUpgrade();
        int increaseHp = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(hpUpgrade, "HP") * baseHp);
        _playerMaxHP = baseHp + increaseHp;
        _playerHP = _playerMaxHP;

        int baseAtk = NetworkGameDataManager._Instance.GetBaseAtk();
        int atkUpgrade = NetworkGameDataManager._Instance.GetAtkUpgrade();
        int increaseAtk = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(atkUpgrade, "BasicWeaponDamage") * baseAtk);
        _baseWeaponAtk = baseAtk + increaseAtk;

        int baseMissileAtk = NetworkGameDataManager._Instance.GetBaseMissileAtk();
        int missileAtkUpgrade = NetworkGameDataManager._Instance.GetMissileAtkUpgrade();
        int increaseMissileAtk = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(missileAtkUpgrade, "MissileWeaponDamage") * baseMissileAtk);
        _missileWeaponAtk = baseMissileAtk + increaseMissileAtk;

        float baseMaxSpeed = NetworkGameDataManager._Instance.GetBaseMaxSpeed();
        int maxSpeedUpgrade = NetworkGameDataManager._Instance.GetMaxSpeedUpgrade();
        int increaseMaxSpeed = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(maxSpeedUpgrade, "MaxSpeed") * (int)baseMaxSpeed);
        _maxVelocity = baseMaxSpeed + increaseMaxSpeed;

        float baseAccSpeed = NetworkGameDataManager._Instance.GetBaseAccSpeed();
        int accSpeedUpgrade = NetworkGameDataManager._Instance.GetAccSpeedUpgrade();
        int increaseAccSpeed = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(accSpeedUpgrade, "Acceleration") * (int)baseAccSpeed);
        _accelateValue = baseAccSpeed + increaseAccSpeed;

        float baseRotateSpeed = NetworkGameDataManager._Instance.GetBaseRotateSpeed();
        int rotateSpeedUpgrade = NetworkGameDataManager._Instance.GetRotateSpeedUpgrade();
        int increaseRotate = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(rotateSpeedUpgrade, "Rotate") * (int)baseRotateSpeed);
        _rotateSpeed = baseRotateSpeed + increaseRotate;

        _fireRateSpeed = 0.2f;
        _missileFireDelay = 5.0f;
        _missileFireDelayCnt = _missileFireDelay;
        _isFire = false;
        _isMissileReady = false;
        _isMissileDelayEnd = true;
        _state = ePlayerState.Ready;
    }

    // Update is called once per frame
    void Update()
    {
        if(_state == ePlayerState.Play)
        {
            PlayerRotate();
            //PlayerMove();
            PlayerGunFire();
            PlayerMissileFire();
            PlayerRotatedTest();
        }       
    }

    private void FixedUpdate()
    {
        if (_state == ePlayerState.Play)
        {
            PlayerMove();
        }
    }

    void PlayerRotate()
    {
        if (_isMissileReady) return;
        Vector3 pos = _crossHairRTR.position;
        Vector3 crossHairPos = Camera.main.ScreenToWorldPoint(pos);
        crossHairPos.y = 0;   
        Quaternion goalAngle = Quaternion.LookRotation(crossHairPos - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, goalAngle, Time.deltaTime * _rotateSpeed);
        //InGameUIManager._Instance.SetDebugText("_rotateSpeed 값: " + _rotateSpeed);
    }

    void PlayerRotatedTest()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 rot = transform.eulerAngles;
            rot.y -= 100 * Time.deltaTime;
            transform.eulerAngles = rot;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 rot = transform.eulerAngles;
            rot.y += 100 * Time.deltaTime;
            transform.eulerAngles = rot;
        }
    }

    void PlayerGunFire()
    {
        if (Input.GetButton("Fire1") && !_isFire && !_isMissileReady)
        {
            _isFire = true;
            StartCoroutine(FireBullet());
        }
    }

    void PlayerMissileFire()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && _isMissileDelayEnd)
        {
            if (_isMissileReady)
            {
                InGameManager._Instance.CancelPlayerLockOn();
                InGameUIManager._Instance.DeSelectMissileSkill();
                _isMissileReady = false;
                return;
            }
            _isMissileReady = true;
            InGameManager._Instance.SetPlayerLockOn();
            InGameUIManager._Instance.SelectMissileSkill();
        }
        if (_isMissileReady)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {               
                if (hit.collider.CompareTag("Enemy"))
                {
                    if(_lockEnemy != null)
                    {
                        _lockEnemy.SetLockOnColor(Color.black);
                    }
                    _lockEnemy = hit.collider.gameObject.GetComponentInParent<Enemy>();
                    _lockEnemy.SetLockOnColor(Color.red);
                }
                

                if (Input.GetButtonDown("Fire1"))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        GameObject go = Instantiate(_prfabMissile, _missileFirePos.transform.position, transform.rotation);
                        PlayerMissile _missile = go.GetComponent<PlayerMissile>();
                        _missile.Init(hit.collider.gameObject, 80, 120, 10, _missileWeaponAtk);
                        InGameManager._Instance.CancelPlayerLockOn();
                        InGameUIManager._Instance.DeSelectMissileSkill();
                        InGameUIManager._Instance.SetMissileDelayTimer(_missileFireDelay);
                        _lockEnemy.SetLockOnColor(Color.black);
                        _lockEnemy = null;
                        _isMissileReady = false;
                        _isMissileDelayEnd = false;
                        StartCoroutine(WaitMissileDelay());
                    }
                }
            }
            else if (_lockEnemy != null)
            {
                _lockEnemy.SetLockOnColor(Color.black);
                _lockEnemy = null;
            }
        }
    }

    void PlayerMove()
    {
        if (Input.GetButton("Fire2") && !_isMissileReady)
        {
            if (_rdBody.velocity.magnitude <= _maxVelocity)
            {
                _rdBody.AddForce(transform.forward * _accelateValue, ForceMode.Force);
            }            
        }
    }

    public void PlayerDamaged(float damage)
    {
        _playerHP -= damage;
        InGameUIManager._Instance.SetHpBarValue(_playerHP / _playerMaxHP);
        if(_playerHP < 0)
        {
            gameObject.SetActive(false);
            InGameManager._Instance.PlayerDead();
        }
    }

    public void SetPlayerStatePlay()
    {
        if(_state != ePlayerState.Play)
        {
            _state = ePlayerState.Play;
        }        
    }

    public void SetPlayerStateEnd()
    {
        _state = ePlayerState.End;
        gameObject.SetActive(false);
    }

    IEnumerator FireBullet()
    {        
        GameObject go = Instantiate(_prefabBullet, _firePos.transform.position, Quaternion.identity);
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.BulletInit(7500, 5.0f, _baseWeaponAtk, transform.forward);
        yield return new WaitForSeconds(_fireRateSpeed);
        _isFire = false;
    }

    IEnumerator WaitMissileDelay()
    {
        yield return new WaitForSeconds(_missileFireDelay);
        _isMissileDelayEnd = true;
    }
}
