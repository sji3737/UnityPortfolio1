using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoG_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPServer _server = new TCPServer(80);
            while (true)
            {
                _server.MainProcess();
            }
        }

        
    }

    public class Constants
    {
        public const int JOIN_SUCCESS = 0;
        public const int JOIN_FAIL_IDOVERLAP = 1;
        public const int JOIN_FAIL_ETC = 2;
    }

    public struct UserInfo
    {
        public long uuid;
        public string id;
        public string pw;
        public string name;
        public int gold;
        public int cash;
        public int clearStage;
        public int hpUpgrade;
        public int atkUpgrade;
        public int missileAtkUpgrade;
        public int speedUpgrade;
        public int accSpeedUpgrade;
        public int rotateSpeedUpgrade;

        public UserInfo(long _uuid, string _id, string _pw, string _name)
        {
            uuid = _uuid;
            id = _id;
            pw = _pw;
            name = _name;
            gold = 1000;
            cash = 0;
            clearStage = 11;
            hpUpgrade = 0;
            atkUpgrade = 0;
            missileAtkUpgrade = 0;
            speedUpgrade = 0;
            accSpeedUpgrade = 0;
            rotateSpeedUpgrade = 0;
        }
    }

    public enum eUpgradeType
    {
        Hp = 0,
        Atk,
        MissileAtk,
        Speed,
        AccSpeed,
        RotateSpeed
    }
}
