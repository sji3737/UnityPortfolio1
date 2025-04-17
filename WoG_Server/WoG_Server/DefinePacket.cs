using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WoG_Server
{
    class DefinePacket
    {
        #region [SendPacket]
        [StructLayout(LayoutKind.Sequential)]
        public struct Send_Connect
        {
            [MarshalAs(UnmanagedType.U1)]
            public bool _isSuccess;
            [MarshalAs(UnmanagedType.U8)]
            public long _clientID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Send_JoinResult
        {
            [MarshalAs(UnmanagedType.U4)]
            public int _resultCode;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Send_LoginResult
        {
            [MarshalAs(UnmanagedType.U4)]
            public int _resultCode;
            [MarshalAs(UnmanagedType.U4)]
            public int _gold;
            [MarshalAs(UnmanagedType.U4)]
            public int _cash;
            [MarshalAs(UnmanagedType.U4)]
            public int _stage;
            [MarshalAs(UnmanagedType.U4)]
            public int _baseHP;
            [MarshalAs(UnmanagedType.U4)]
            public int _baseAtk;
            [MarshalAs(UnmanagedType.U4)]
            public int _baseMissileAtk;
            [MarshalAs(UnmanagedType.R4)]
            public float _baseMaxSpeed;
            [MarshalAs(UnmanagedType.R4)]
            public float _baseAccSpeed;
            [MarshalAs(UnmanagedType.R4)]
            public float _baseRotateSpeed;
            [MarshalAs(UnmanagedType.U4)]
            public int _hpUpgrade;
            [MarshalAs(UnmanagedType.U4)]
            public int _atkUpgrade;
            [MarshalAs(UnmanagedType.U4)]
            public int _missileAtkUpgrade;
            [MarshalAs(UnmanagedType.U4)]
            public int _speedUpgrade;
            [MarshalAs(UnmanagedType.U4)]
            public int _accSpeedUpgrade;
            [MarshalAs(UnmanagedType.U4)]
            public int _rotateSpeedUpgrade;
            [MarshalAs(UnmanagedType.U8)]
            public long _uuid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] _nickName;

            public void ModData(string name)
            {
                _nickName = new byte[20];

                byte[] _nameString = new UTF8Encoding(true, true).GetBytes(name);
                Array.Copy(_nameString, 0, _nickName, 0, _nameString.Length);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Send_LoginResultTest
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] _nickName;

            public void ModData(string name)
            {
                _nickName = new byte[20];

                byte[] _nameString = new UTF8Encoding(true, true).GetBytes(name);
                Array.Copy(_nameString, 0, _nickName, 0, _nameString.Length);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Send_GoldValue
        {
            [MarshalAs(UnmanagedType.U4)]
            public int _gold;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Send_CashValue
        {
            [MarshalAs(UnmanagedType.U4)]
            public int _cash;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Send_CurrentClearStage
        {
            [MarshalAs(UnmanagedType.U4)]
            public int _stage;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Send_IncreaseUpgradeResult
        {
            [MarshalAs(UnmanagedType.U1)]
            public bool _success;
            [MarshalAs(UnmanagedType.U4)]
            public int _upgradeType;
            [MarshalAs(UnmanagedType.U4)]
            public int _spendedCredit;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Send_GameResultHandle
        {
            [MarshalAs(UnmanagedType.U1)]
            public bool _success;
            [MarshalAs(UnmanagedType.U4)]
            public int _credit;
            [MarshalAs(UnmanagedType.U4)]
            public int _stage;
        }

        #endregion

        #region [ReceivePacket]
        [StructLayout(LayoutKind.Sequential)]
        public struct Receive_Join
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] _id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] _pw;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] _nickName;

            public void ModData(string id, string pw, string name)
            {
                _id = new byte[20];
                _pw = new byte[20];
                _nickName = new byte[20];

                byte[] _idString = new UTF8Encoding(true, true).GetBytes(id);
                Array.Copy(_idString, 0, _id, 0, _idString.Length);
                byte[] _pwString = new UTF8Encoding(true, true).GetBytes(pw);
                Array.Copy(_pwString, 0, _pw, 0, _pwString.Length);
                byte[] _nameString = new UTF8Encoding(true, true).GetBytes(name);
                Array.Copy(_nameString, 0, _nickName, 0, _nameString.Length);
            }

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Receive_Login
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] _id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] _pw;

            public void ModData(string id, string pw)
            {
                _id = new byte[20];
                _pw = new byte[20];

                byte[] _idString = new UTF8Encoding(true, true).GetBytes(id);
                Array.Copy(_idString, 0, _id, 0, _idString.Length);
                byte[] _pwString = new UTF8Encoding(true, true).GetBytes(pw);
                Array.Copy(_pwString, 0, _pw, 0, _pwString.Length);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Receive_GoldChangeValue
        {
            [MarshalAs(UnmanagedType.U8)]
            public int _uuid;
            [MarshalAs(UnmanagedType.U4)]
            public int _goldChange;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Receive_CashChangeValue
        {
            [MarshalAs(UnmanagedType.U8)]
            public int _uuid;
            [MarshalAs(UnmanagedType.U4)]
            public int _cashChange;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Receive_StageClear
        {
            [MarshalAs(UnmanagedType.U8)]
            public int _uuid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Receive_IncreaseUpgradeRequest
        {
            [MarshalAs(UnmanagedType.U4)]
            public int _upgradeType;
            [MarshalAs(UnmanagedType.U4)]
            public int _credit;
            [MarshalAs(UnmanagedType.U8)]
            public long _uuid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Receive_GameResult
        {
            [MarshalAs(UnmanagedType.U4)]
            public int _credit;
            [MarshalAs(UnmanagedType.U4)]
            public int _stage;
            [MarshalAs(UnmanagedType.U8)]
            public long _uuid;
        }
        #endregion
    }
}
