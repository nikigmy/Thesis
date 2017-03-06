using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;

public class DatabaseLayer
{
    const string connectionPath = "URI=file:C:/Users/NikolaiMilanov/Documents/UnityProjects/Thesis/Assets/Database/Database.s3db";
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

    //tested for connect
    public int ConnectPlayer(string facebookID, int connectionID)
    {
        //string sqlQuery = "SELECT FacebookID,Description,Name,OnlineStatus " + "FROM Players" + " WHERE OnlineStatus = 1";
        string sqlQuery = " UPDATE Players SET OnlineStatus = " + 1 + ", ConnectionID = " + connectionID + " WHERE FacebookID = " + facebookID + ";";
        int cols = ExecuteNonQuery(sqlQuery);
        return cols;
        //if (changed == 0)
        //{
        //    string query = "INSERT INTO Players ( FacebookID, CurrentIP, OnlineStatus) VALUES(" + facebookID + ", " + connectionID + ", " + 1 + ");";
        //}
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

    private int ExecuteNonQuery(string query)
    {
        IDbCommand dbcmd = databaseConnection.CreateCommand();
        dbcmd.CommandText = query;
        int changedColumns = dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;
        return changedColumns;
    }

    //Outdated UpdateThis 
    public void AddPlayer(string facebookID, string currentIP)
    {
        string query = "INSERT INTO Players ( FacebookID, Description, CurrentIP, OnlineStatus) VALUES(" + facebookID + ", " + " " + ", " + currentIP + ", " + 1 + ");";
        ExecuteNonQuery(query);
    }

    private IDataReader ReadFromDatabase(string query)
    {
        IDbCommand dbcmd = databaseConnection.CreateCommand();
        dbcmd.CommandText = query;
        IDataReader reader = dbcmd.ExecuteReader();
        return reader;
    }

    //for notifications
    private List<int> GetFriendIDs(int cliendDbId)
    {
        List<int> friendIds = new List<int>();
        string query = "SELECT Friend1ID, Friend2ID FROM Friends WHERE Friend1ID = " + cliendDbId + " OR Friend2ID = " + cliendDbId + ";";

        IDataReader reader = ReadFromDatabase(query);
        while (reader.Read())
        {
            int ID1 = reader.GetInt32(0);
            int ID2 = reader.GetInt32(1);
            if (ID1 == cliendDbId)
            {
                friendIds.Add(ID2);
            }
            else
            {
                friendIds.Add(ID1);
            }
        }

        reader.Close();
        reader = null;
        return friendIds;
    }

    //for registration
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

    public void DisconectPlayer(int connectionID)
    {
        string sqlQuery = " UPDATE Players SET OnlineStatus = " + 0 + ", ConnectionID = -1 WHERE ConnectionID = " + connectionID + ";";
        int a = ExecuteNonQuery(sqlQuery);
    }

    public void SaveFlapyBirdScore(int connectionId, int score)
    {
        int DbId = 0;
        DbId = GetDbId(connectionId);

        string alreadyPlayedQuery = "SELECT COUNT(ID) AS count FROM FlappyBirdScores WHERE PlayerID = " + DbId +";";
        IDataReader alreadyPlayedReader = ReadFromDatabase(alreadyPlayedQuery);
        int count = 0;
        while (alreadyPlayedReader.Read())
        {
            count = alreadyPlayedReader.GetInt32(0);
        }
        alreadyPlayedReader.Close();
        alreadyPlayedReader = null;

        if (count == 0)
        {
            string addQuery = "INSERT INTO FlappyBirdScores ( PlayerID, Score) VALUES(" + DbId + ", " + score + ");";
            ExecuteNonQuery(addQuery);
        }
        else
        {
            int currentHighScore = 0;
            string getScoreQuery = "SELECT Score FROM FlappyBirdScores WHERE PlayerID = " + DbId + ";";
            IDataReader currentHighScoreReader = ReadFromDatabase(getScoreQuery);
            while (currentHighScoreReader.Read())
            {
                currentHighScore = currentHighScoreReader.GetInt32(0);
            }
            currentHighScoreReader.Close();
            currentHighScoreReader = null;

            if (currentHighScore < score)
            {
                string updateQuery = " UPDATE FlappyBirdScores SET Score = " + score + " WHERE PlayerID = " + DbId + ";";
                ExecuteNonQuery(updateQuery);
            }
        }

    }

    public void SaveEndlessRunnerScore(int connectionId, int score)
    {
        int DbId = 0;
        DbId = GetDbId(connectionId);

        string alreadyPlayedQuery = "SELECT COUNT(ID) AS count FROM EndlessRunnerScores WHERE PlayerID = " + DbId + ";";
        IDataReader alreadyPlayedReader = ReadFromDatabase(alreadyPlayedQuery);
        int count = 0;
        while (alreadyPlayedReader.Read())
        {
            count = alreadyPlayedReader.GetInt32(0);
        }
        alreadyPlayedReader.Close();
        alreadyPlayedReader = null;

        if (count == 0)
        {
            string addQuery = "INSERT INTO EndlessRunnerScores ( PlayerID, Score) VALUES(" + DbId + ", " + score + ");";
            ExecuteNonQuery(addQuery);
        }
        else
        {
            int currentHighScore = 0;
            string getScoreQuery = "SELECT Score FROM EndlessRunnerScores WHERE PlayerID = " + DbId + ";";
            IDataReader currentHighScoreReader = ReadFromDatabase(getScoreQuery);
            while (currentHighScoreReader.Read())
            {
                currentHighScore = currentHighScoreReader.GetInt32(0);
            }
            currentHighScoreReader.Close();
            currentHighScoreReader = null;

            if (currentHighScore < score)
            {
                string updateQuery = " UPDATE EndlessRunnerScores SET Score = " + score + " WHERE PlayerID = " + DbId + ";";
                ExecuteNonQuery(updateQuery);
            }
        }
    }

    public int GetEndlessRunnerHighscore(int connectionId)
    {
        int DbId = GetDbId(connectionId);
        int currentHighScore = 0;
        string getScoreQuery = "SELECT Score FROM EndlessRunnerScores WHERE PlayerID = " + DbId + ";";
        IDataReader currentHighScoreReader = ReadFromDatabase(getScoreQuery);
        while (currentHighScoreReader.Read())
        {
            currentHighScore = currentHighScoreReader.GetInt32(0);
        }
        currentHighScoreReader.Close();
        currentHighScoreReader = null;

        return currentHighScore;
    }

    int GetDbId(int connectionId)
    {
        int DbId = 0;
        string getIDQuery = "SELECT ID FROM Players WHERE ConnectionID = " + connectionId + ";";
        IDataReader idReader = ReadFromDatabase(getIDQuery);
        while (idReader.Read())
        {
            DbId = idReader.GetInt32(0);
        }
        idReader.Close();
        idReader = null;

        return DbId;
    }

    string GetFbId(int dbId)
    {
        string query = "SELECT FacebookID FROM Players WHERE ID = " + dbId + ";";
        IDataReader reader = ReadFromDatabase(query);
        string fbId = "";
        while (reader.Read())
        {
            fbId = reader.GetString(0);
        }
        reader.Close();
        reader = null;

        return fbId;
    }
    /// <summary>
    /// Gets scores of friends
    /// </summary>
    /// <param name="connectionID"></param>
    /// <returns>Dictionary with Facebook ids and scores for friends</returns>
    public Dictionary<string, int> GetFlapyBirdScores(int connectionID)
    {
        Dictionary<string, int> fbIdsAndScores = new Dictionary<string, int>();
        List<int> friendIds = GetFriendIDs(GetDbId(connectionID));
        if (friendIds.Count == 0)
            return null;
        StringBuilder query = new StringBuilder();
        query.Append("SELECT PlayerID, Score FROM FlappyBirdScores WHERE ");

        query.Append("PlayerID = " + friendIds[0]);
        for (int i = 1; i < friendIds.Count; i++)
        {
            query.Append(" OR PlayerID = " + friendIds[i]);
        }
        query.Append(";");

        IDataReader reader = ReadFromDatabase(query.ToString());
        int dbId = 0;
        int score = 0;
        while (reader.Read())
        {
            dbId = reader.GetInt32(0);
            score = reader.GetInt32(1);
            string fbId = GetFbId(dbId);
            fbIdsAndScores.Add(fbId, score);
        }
        reader.Close();
        reader = null;

        return fbIdsAndScores;
    }
    public Dictionary<string, int> GetEndlessRunnerScores(int connectionID)
    {
        Dictionary<string, int> fbIdsAndScores = new Dictionary<string, int>();
        List<int> friendIds = GetFriendIDs(GetDbId(connectionID));
        if (friendIds.Count == 0)
            return null;
        StringBuilder query = new StringBuilder();
        query.Append("SELECT PlayerID, Score FROM FlappyBirdScores WHERE ");

        query.Append("PlayerID = " + friendIds[0]);
        for (int i = 1; i < friendIds.Count; i++)
        {
            query.Append(" OR PlayerID = " + friendIds[i]);
        }
        query.Append(";");

        IDataReader reader = ReadFromDatabase(query.ToString());
        int dbId = 0;
        int score = 0;
        while (reader.Read())
        {
            dbId = reader.GetInt32(0);
            score = reader.GetInt32(1);
            string fbId = GetFbId(dbId);
            fbIdsAndScores.Add(fbId, score);
        }
        reader.Close();
        reader = null;

        return fbIdsAndScores;
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

    public List<string> GetOnlineFriends(int connectionID)
    {
        int DbId = 0;
        string getIDQuery = "SELECT ID FROM Players WHERE ConnectionID = " + connectionID + ";";
        IDataReader reader = ReadFromDatabase(getIDQuery);
        while (reader.Read())
        {
            DbId = reader.GetInt32(0);
        }
        reader.Close();
        reader = null;

        List<int> friendDbIds = GetFriendIDs(DbId);
        StringBuilder query = new StringBuilder();
        query.Append("SELECT FacebookID FROM Players WHERE ");
        query.Append("(ID = " + friendDbIds[0] + " AND OnlineStatus = 1)");
        for (int i = 1; i < friendDbIds.Count; i++)
        {
            query.Append(" OR (ID = " + friendDbIds[i] + " AND OnlineStatus = 1)");
        }
        query.Append(";");

        IDataReader connectionIdReader = ReadFromDatabase(query.ToString());
        List<string> onlineFriendsFbIds = new List<string>();
        while (connectionIdReader.Read())
        {
            string facebookId = connectionIdReader.GetString(0);
            onlineFriendsFbIds.Add(facebookId);
        }
        connectionIdReader.Close();
        connectionIdReader = null;

        return onlineFriendsFbIds;
    }

    public Dictionary<string, List<int>> GetOnlineFriendsIds(int connectionID)
    {
        int DbId = 0;
        string FbId = "";
        string getIDQuery = "SELECT ID, FacebookID FROM Players WHERE ConnectionID = " + connectionID + ";";
        //string getCountQuery = "SELECT COUNT(ID) AS count FROM Players;";
        IDataReader reader = ReadFromDatabase(getIDQuery);
        while (reader.Read())
        {
            DbId = reader.GetInt32(0);
            FbId = reader.GetString(1);
        }
        reader.Close();
        reader = null;

        List<int> friendDbIds = GetFriendIDs(DbId);
        if (friendDbIds.Count == 0) return new Dictionary<string, List<int>>();
        StringBuilder query = new StringBuilder();
        query.Append("SELECT ConnectionID FROM Players WHERE ");
        query.Append("(ID = " + friendDbIds[0] + " AND OnlineStatus = 1)");
        for (int i = 1; i < friendDbIds.Count; i++)
        {
            query.Append(" OR (ID = " + friendDbIds[i] + " AND OnlineStatus = 1)");
        }
        query.Append(";");
        IDataReader connectionIdReader = ReadFromDatabase(query.ToString());
        Dictionary<string, List<int>> conectionToFbDictionary = new Dictionary<string, List<int>>();
        conectionToFbDictionary.Add(FbId, new List<int>());
        while (connectionIdReader.Read())
        {
            int connectionId = connectionIdReader.GetInt32(0);
            conectionToFbDictionary.First().Value.Add(connectionId);
        }
        connectionIdReader.Close();
        connectionIdReader = null;
        return conectionToFbDictionary;
    }

    public int GetConnectionID(string fbId)
    {
        string query = "SELECT ConnectionID FROM Players WHERE FacebookID = " + fbId + ";";
        IDataReader reader = ReadFromDatabase(query);
        int connectionId = 0;
        while (reader.Read())
        {
            connectionId = reader.GetInt32(0);
        }
        reader.Close();
        reader = null;
        return connectionId;
    }

    public string GetFacebookID(int connectionId)
    {
        string query = "SELECT FacebookID FROM Players WHERE ConnectionID = " + connectionId + ";";
        IDataReader reader = ReadFromDatabase(query);
        string facebookId = "";
        while (reader.Read())
        {
            facebookId = reader.GetString(0);
        }
        reader.Close();
        reader = null;
        return facebookId;
    }
}
