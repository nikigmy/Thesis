using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.Common;
using System.Text;
using Mono.Data.Sqlite;

public class DatabaseLayer
{
    const string connectionPath = "file:C:/Users/NikolaiMilanov/Documents/UnityProjects/Thesis/Assets/Database/Database.s3db";
    private static DatabaseLayer instance;
    private IDbConnection databaseConnection;

    private DatabaseLayer()
    {
        databaseConnection = (IDbConnection)new SqliteConnection(connectionPath);
        databaseConnection.Open();
    }

    public static DatabaseLayer GetInstance()
    {
        if (instance == null)
        {
            DatabaseLayer dbLayer = new DatabaseLayer();
            instance = dbLayer;
            return dbLayer;
        }
        else
        {
            return instance;
        }
    }

    public void ConnectOrAddPlayer(string facebookID, string currentIP)
    {
        IDbCommand dbcmd = databaseConnection.CreateCommand();
        //string sqlQuery = "SELECT FacebookID,Description,Name,OnlineStatus " + "FROM Players" + " WHERE OnlineStatus = 1";
        string sqlQuery = " UPDATE Players SET CurrentIP = " + currentIP + ", OnlineStatus = " + 1 + " WHERE FacebookID = " + facebookID;
        dbcmd.CommandText = sqlQuery;
        int changed = dbcmd.ExecuteNonQuery();
        if (changed == 0)
        {
            string query = "INSERT INTO Players ( FacebookID, CurrentIP, OnlineStatus) VALUES(" + facebookID + ", " + currentIP + ", " + 1 + ");";
        }
        //IDataReader reader = dbcmd.ExecuteReader();
        //while (reader.Read())
        //{
        //    string favebookID = reader.GetString(0);
        //    string description = reader.GetString(1);
        //    string name = reader.GetString(2);
        //    int status = reader.GetInt32(3);

        //    Debug.Log("ID= " + favebookID + "  name =" + name + "  description =" + description + "  status =" + status);
        //}
    }

    public void AddPlayerAndFriends(string facebookID, string currentIP, List<string> friendFbIds)
    {
        int NewPlayerId = GetTotalPlayersCount() + 1;
        AddPlayer(facebookID, currentIP);
        
        List<int> friendIds = GetFriendIDs(friendFbIds);
        PopulateFriendsTable(NewPlayerId, friendIds);
    }

    private void PopulateFriendsTable(int mainId, List<int> friendIds)
    {
        foreach (var friendId in friendIds)
        {
            string query = "INSERT INTO Friends ( Friend1ID, Friend2ID) VALUES(" + mainId + ", " + friendId + ");";
            ExecuteNonQuery(query);
        }
    }

    private int GetTotalPlayersCount()
    {
        int count = 0;
        string getCountQuery = "SELECT COUNT(ID) FROM Players;";
        //string getCountQuery = "SELECT COUNT(ID) AS count FROM Players;";
        IDataReader reader = ReadFromDatabase(getCountQuery);
        count = reader.GetInt32(0);

        reader.Close();
        reader = null;

        return count;
    }

    private void ExecuteNonQuery(string query)
    {
        IDbCommand dbcmd = databaseConnection.CreateCommand();
        dbcmd.CommandText = query;
        dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;
    }

    private void AddPlayer(string facebookID, string currentIP)
    {
        string query = "INSERT INTO Players ( FacebookID, Description, CurrentIP, OnlineStatus) VALUES(" + facebookID + ", " +" "+", "+ currentIP + ", " + 1 + ");";
        ExecuteNonQuery(query);
    }

    private IDataReader ReadFromDatabase(string query)
    {
        IDbCommand dbcmd = databaseConnection.CreateCommand();
        dbcmd.CommandText = query;
        IDataReader reader = dbcmd.ExecuteReader();
        return reader;
    }

    private List<int> GetFriendIDs(List<string> friendFbIds)
    {
        List<int> friendIds = new List<int>();
        StringBuilder queryForFriends = new StringBuilder();
        queryForFriends.Append("SELECT ID FROM Players WHERE ");
        queryForFriends.Append("FacebookID = " + friendFbIds[0]);
        for (int i = 1; i < friendFbIds.Count; i++)
        {
            queryForFriends.Append(" OR FacebookID = " + friendFbIds[i]);
        }
        queryForFriends.Append(";");

        IDataReader reader = ReadFromDatabase(queryForFriends.ToString());
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            friendIds.Add(id);
        }

        reader.Close();
        reader = null;
        return friendIds;
    }

    public void DisconectPlayer(string currentIP)
    {
        IDbCommand dbcmd = databaseConnection.CreateCommand();
        string sqlQuery = " UPDATE Players SET OnlineStatus = " + 0 + ", CurrentIP =  WHERE CurrentIP = " + currentIP + ";";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;
    }

    public void UpdateDescription(string facebookID, string Description)
    {
        IDbCommand dbcmd = databaseConnection.CreateCommand();
        string sqlQuery = " UPDATE Players SET Description = " + Description + " WHERE FacebookID = " + facebookID + ";";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;
    }

    public void GetFriends(string ip)
    {

    }
}
