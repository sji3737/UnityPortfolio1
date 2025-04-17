using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WoG_Server
{
    class DBManager
    {
        bool _isConnect;
        MySqlConnection connect;
        long _nextUUID;

        public void ConnectDB()
        {
            using(connect = new MySqlConnection("Server = localhost; Port = 3306; Database = WoGDatabase; UID = root; Pwd = 596841"))
            {
                try
                {
                    connect.Open();
                    _isConnect = true;
                    connect.Close();
                }
                catch(Exception ep)
                {
                    Console.WriteLine("실패");
                    Console.WriteLine(ep.ToString());
                }
            }
        }

        public void SetNextUUID()
        {
            string q = "SELECT MAX(UUID) AS CURRENTUUID FROM userinfo";
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object val = command.ExecuteScalar();
                if (val.ToString() != "")
                {
                    _nextUUID = (long)int.Parse(val.ToString());
                    _nextUUID++;
                }
                else
                {
                    _nextUUID = 1000000;
                }
            }
            catch (Exception ep)
            {
                Console.WriteLine("실패");
                Console.WriteLine(ep.ToString());
            }
            connect.Close();
        }

        public long GetNextUUID()
        {
            return _nextUUID;
        }

        public void IncreaseUUID()
        {
            _nextUUID++;
        }

        public bool InsertUserInfo(UserInfo info)
        {
            string query = "INSERT INTO userinfo(UUID, ID, PW, NAME, GOLD, CASH, CLEARSTAGE)" +
                "VALUES('" + info.uuid.ToString() + "', '" + info.id + "', '" + info.pw + "', '" + info.name + "', " + info.gold + ", " 
                + info.cash + ", " + info.clearStage + ")";

            bool isSuccess = false;

            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(query, connect);

                if (command.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine("INSERT 성공");
                    isSuccess = true;
                }
                else
                {
                    Console.WriteLine("INSERT 실패");
                }
                connect.Close();
            }
            catch (Exception ep)
            {
                Console.WriteLine("실패");
                Console.WriteLine(ep.ToString());
            }
            //connect.Close();

            return isSuccess;
        }

        public int CheckMatchLoginInfo(string id, string pw)
        {
            int result = 1;
            string q = "SELECT PW FROM userinfo WHERE ID = '" + id + "'";
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(string.Compare(pw,value.ToString()) == 0)
                {
                    result = 0;
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("실패");
                Console.WriteLine(ep.ToString());
            }
            connect.Close();

            return result;
        }

        public long GetUUID(string id)
        {
            long uuid = -1;
            string query = "SELECT UUID FROM userinfo WHERE ID = '" + id + "'";
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(query, connect);
                object value = command.ExecuteScalar();
                uuid = long.Parse(value.ToString());
            }
            catch (Exception ep)
            {
                Console.WriteLine("실패");
                Console.WriteLine(ep.ToString());
            }
            connect.Close();

            return uuid;
        }

        public bool CheckIDOverlap(string id)
        {
            bool isOverlap = false;
            string q = "SELECT ID FROM userinfo WHERE ID = '" + id + "'";
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                MySqlDataReader rData = command.ExecuteReader();
                while (rData.Read())
                {
                    if(rData[0] != null)
                    {
                        connect.Close();
                        return true;
                    }
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("실패");
                Console.WriteLine(ep.ToString());
            }
            connect.Close();

            return isOverlap;
        }

        public bool UpdateGoldValue(long uuid, int value)
        {
            bool isSuccess = false;
            string q = "UPDATE userinfo SET GOLD = GOLD + " + value + " WHERE UUID = " + uuid;
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                if (command.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine("GOLD 정보 업데이트 성공");
                    isSuccess = true;
                }
                else
                {
                    Console.WriteLine("GOLD 정보 업데이트 실패");
                }              
                connect.Close();
            }
            catch(Exception ep)
            {
                connect.Close();
                Console.WriteLine(q + " 실패");
                Console.WriteLine(ep.ToString());
            }

            return isSuccess;
        }
        
        public void UpdateCashValue(long uuid, int value)
        {
            string q = "UPDATE userinfo SET CASH = CASH + " + value + "WHERE UUID = " + uuid;
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                if (command.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine("CASH 정보 업데이트 성공");
                }
                else
                {
                    Console.WriteLine("CASH 정보 업데이트 실패");
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("실패");
                Console.WriteLine(ep.ToString());
            }
        }

        public void UpdateClearStage(long uuid)
        {
            string q = "UPDATE userinfo SET CLEARSTAGE = CLEARSTAGE + 1 WHERE UUID = " + uuid;
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                if(command.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine("ClearStage 정보 업데이트 성공");
                }
                else
                {
                    Console.WriteLine("ClearStage 정보 업데이트 실패");
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("실패");
                Console.WriteLine(ep.ToString());
            }
        }

        public bool IncreaseUpgrade(long uuid, eUpgradeType type, int requireCredit)
        {
            bool isSuccess = false;
            string q = "";
            switch (type)
            {
                case eUpgradeType.Hp:
                    q = "UPDATE userinfo SET HpUpgrade = HpUpgrade + 1 WHERE UUID = " + uuid;
                    break;
                case eUpgradeType.Atk:
                    q = "UPDATE userinfo SET AtkUpgrade = AtkUpgrade + 1 WHERE UUID = " + uuid;
                    break;
                case eUpgradeType.MissileAtk:
                    q = "UPDATE userinfo SET MissileAtkUpgrade = MissileAtkUpgrade + 1 WHERE UUID = " + uuid;
                    break;
                case eUpgradeType.Speed:
                    q = "UPDATE userinfo SET SpeedUpgrade = SpeedUpgrade + 1 WHERE UUID = " + uuid;
                    break;
                case eUpgradeType.AccSpeed:
                    q = "UPDATE userinfo SET AccSpeedUpgrade = AccSpeedUpgrade + 1 WHERE UUID = " + uuid;
                    break;
                case eUpgradeType.RotateSpeed:
                    q = "UPDATE userinfo SET RotateSpeedUpgrade = RotateSpeedUpgrade + 1 WHERE UUID = " + uuid;
                    break;
            }
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                if(command.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine("HpUpgrade 정보 업데이트 성공");
                    isSuccess = true;
                }
                else
                {
                    connect.Close();                    
                    Console.WriteLine("HpUpgrade 정보 업데이트 실패");
                    UpdateGoldValue(uuid, requireCredit);
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                connect.Close();
                UpdateGoldValue(uuid, requireCredit);
                Console.WriteLine(q + " 실패");
                Console.WriteLine(ep.ToString());
            }

            return isSuccess;
        }

        public bool GameResultHandle(long uuid, int credit, int stage)
        {
            bool success = false;
            string qGold = "UPDATE userinfo SET GOLD = GOLD + " + credit + " WHERE UUID = " + uuid;
            string qStage = "UPDATE userinfo SET CLEARSTAGE = " + stage + " WHERE UUID = " + uuid;
            connect.Open();
            MySqlTransaction transaction = connect.BeginTransaction();

            try
            {                
                MySqlCommand commandGold = new MySqlCommand(qGold, connect);
                MySqlCommand commandStage = new MySqlCommand(qStage, connect);

                commandGold.ExecuteNonQuery();
                commandStage.ExecuteNonQuery();

                transaction.Commit();

                success = true;
            }
            catch(Exception ep)
            {
                Console.WriteLine("GameResultHandle 실패");
                Console.WriteLine(ep.ToString());
                transaction.Rollback();
            }
            finally
            {
                connect.Close();
            }

            return success;
        }

        public int GetGoldValue(long uuid)
        {
            int gold = -1;
            string q = "SELECT GOLD FROM userinfo WHERE UUID = " + uuid;
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(value.ToString() != "")
                {
                    gold = int.Parse(value.ToString());
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine(q + " 실패");
                Console.WriteLine(ep.ToString());
            }

            return gold;
        }

        public int GetCashValue(long uuid)
        {
            int cash = -1;
            string q = "SELECT CASH FROM userinfo WHERE UUID = " + uuid;
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(value.ToString() != "")
                {
                    cash = int.Parse(value.ToString());
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("SELECT CASH 실패");
                Console.WriteLine(ep.ToString());
            }
            return cash;
        }

        public int GetClearStageInfo(long uuid)
        {
            int clearStage = -1;
            string q = "SELECT CLEARSTAGE FROM userinfo WHERE UUID = " + uuid;

            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(value.ToString() != "")
                {
                    clearStage = int.Parse(value.ToString());
                }

                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("SELECT CLEARSTAGE 실패");
                Console.WriteLine(ep.ToString());
            }

            return clearStage;
        }

        public string GetNickName(long uuid)
        {
            string name = "";
            string q = "SELECT NAME FROM userinfo WHERE UUID = " + uuid;

            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(value.ToString() != "")
                {
                    name = value.ToString();
                }

                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("SELECT NAME 실패");
                Console.WriteLine(ep.ToString());
            }

            return name;
        }

        public int GetBaseStatID(long uuid)
        {
            int baseStatId = -1;
            string q = "SELECT BaseStatID FROM userinfo WHERE UUID = " + uuid;

            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if (value.ToString() != "")
                {
                    baseStatId = int.Parse(value.ToString());
                }

                connect.Close();
            }
            catch (Exception ep)
            {
                Console.WriteLine("SLECT BaseStatID 실패");
                Console.WriteLine(ep.ToString());
            }

            return baseStatId;
        }

        public int GetBaseHPInfo(long uuid)
        {
            int baseHpInfo = -1;
            int baseStatId = GetBaseStatID(uuid);
            string q = "SELECT BaseHP FROM basestatinfo WHERE BaseStatID = " + baseStatId;

            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(value.ToString() != "")
                {
                    baseHpInfo = int.Parse(value.ToString());
                }

                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("SELECT BaseHP 실패");
                Console.WriteLine(ep.ToString());
            }

            return baseHpInfo;
        }

        public int GetBaseAtkInfo(long uuid)
        {
            int baseATKInfo = -1;
            int baseStatId = GetBaseStatID(uuid);
            string q = "SELECT BaseAtk FROM basestatinfo WHERE BaseStatID = " + baseStatId;

            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if (value.ToString() != "")
                {
                    baseATKInfo = int.Parse(value.ToString());
                }

                connect.Close();
            }
            catch (Exception ep)
            {
                Console.WriteLine("SELECT BaseAtk 실패");
                Console.WriteLine(ep.ToString());
            }

            return baseATKInfo;
        }

        public int GetBaseMissileAtkInfo(long uuid)
        {
            int baseMissileATKInfo = -1;
            int baseStatId = GetBaseStatID(uuid);
            string q = "SELECT BaseMissileAtk FROM basestatinfo WHERE BaseStatID = " + baseStatId;

            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if (value.ToString() != "")
                {
                    baseMissileATKInfo = int.Parse(value.ToString());
                }

                connect.Close();
            }
            catch (Exception ep)
            {
                Console.WriteLine("SELECT BaseMissileAtk 실패");
                Console.WriteLine(ep.ToString());
            }

            return baseMissileATKInfo;
        }

        public float GetBaseMaxSpeedInfo(long uuid)
        {
            float baseMaxSpeedInfo = -1;
            int baseStatId = GetBaseStatID(uuid);
            string q = "SELECT BaseMaxSpeed FROM basestatinfo WHERE BaseStatID = " + baseStatId;

            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if (value.ToString() != "")
                {
                    baseMaxSpeedInfo = float.Parse(value.ToString());
                }

                connect.Close();
            }
            catch (Exception ep)
            {
                Console.WriteLine("SELECT BaseMaxSpeed 실패");
                Console.WriteLine(ep.ToString());
            }

            return baseMaxSpeedInfo;
        }

        public float GetBaseAccSpeedInfo(long uuid)
        {
            float baseAccSpeedInfo = -1;
            int baseStatId = GetBaseStatID(uuid);
            string q = "SELECT BaseAccSpeed FROM basestatinfo WHERE BaseStatID = " + baseStatId;

            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if (value.ToString() != "")
                {
                    baseAccSpeedInfo = float.Parse(value.ToString());
                }

                connect.Close();
            }
            catch (Exception ep)
            {
                Console.WriteLine("SELECT BaseMaxSpeed 실패");
                Console.WriteLine(ep.ToString());
            }

            return baseAccSpeedInfo;
        }

        public float GetBaseRotateSpeedInfo(long uuid)
        {
            float baseRotateSpeedInfo = -1;
            int baseStatId = GetBaseStatID(uuid);
            string q = "SELECT BaseRotateSpeed FROM basestatinfo WHERE BaseStatID = " + baseStatId;

            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if (value.ToString() != "")
                {
                    baseRotateSpeedInfo = float.Parse(value.ToString());
                }

                connect.Close();
            }
            catch (Exception ep)
            {
                Console.WriteLine("SELECT BaseMaxSpeed 실패");
                Console.WriteLine(ep.ToString());
            }

            return baseRotateSpeedInfo;
        }

        public int GetHpUpgradeInfo(long uuid)
        {
            int hpUpgradeInfo = -1;
            string q = "SELECT HpUpgrade FROM userinfo WHERE UUID = " + uuid;

            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(value.ToString() != "")
                {
                    hpUpgradeInfo = int.Parse(value.ToString());
                }

                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("SELECT HpUpgrade 실패");
                Console.WriteLine(ep.ToString());
            }

            return hpUpgradeInfo;
        }

        public int GetAtkUpgradeInfo(long uuid)
        {
            int atkUpgradeInfo = -1;
            string q = "SELECT AtkUpgrade FROM userinfo WHERE UUID = " + uuid;
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(value.ToString() != "")
                {
                    atkUpgradeInfo = int.Parse(value.ToString());
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("SELECT AtkUpgrade 실패");
                Console.WriteLine(ep.ToString());
            }

            return atkUpgradeInfo;
        }

        public int GetMissileAtkUpgradeInfo(long uuid)
        {
            int missileAtkUpgradeInfo = -1;
            string q = "SELECT MissileAtkUpgrade FROM userinfo WHERE UUID = " + uuid;
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(value.ToString() != "")
                {
                    missileAtkUpgradeInfo = int.Parse(value.ToString());
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("SELECT MissileAtkUpgrade 실패");
                Console.WriteLine(ep.ToString());
            }

            return missileAtkUpgradeInfo;
        }

        public int GetSpeedUpgradeInfo(long uuid)
        {
            int speedUpgradeInfo = -1;
            string q = "SELECT SpeedUpgrade FROM userinfo WHERE UUID = " + uuid;
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(value.ToString() != "")
                {
                    speedUpgradeInfo = int.Parse(value.ToString());
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("SELECT SpeedUpgrade 실패");
                Console.WriteLine(ep.ToString());
            }

            return speedUpgradeInfo;
        }

        public int GetAccSpeedUpgradeInfo(long uuid)
        {
            int accSpeedUpgradeInfo = -1;
            string q = "SELECT AccSpeedUpgrade FROM userinfo WHERE UUID = " + uuid;
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(value.ToString() != "")
                {
                    accSpeedUpgradeInfo = int.Parse(value.ToString());
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("SELECT AccSpeedUpgrade 실패");
                Console.WriteLine(ep.ToString());
            }

            return accSpeedUpgradeInfo;
        }

        public int GetRotateSpeedUpgradeInfo(long uuid)
        {
            int rotateSpeedUpgradeInfo = -1;
            string q = "SELECT RotateSpeedUpgrade FROM userinfo WHERE UUID = " + uuid;
            try
            {
                connect.Open();
                MySqlCommand command = new MySqlCommand(q, connect);
                object value = command.ExecuteScalar();
                if(value.ToString() != "")
                {
                    rotateSpeedUpgradeInfo = int.Parse(value.ToString());
                }
                connect.Close();
            }
            catch(Exception ep)
            {
                Console.WriteLine("SELECT RotateSpeedUpgrade 실패");
                Console.WriteLine(ep.ToString());
            }
            return rotateSpeedUpgradeInfo;
        }

    }
}
