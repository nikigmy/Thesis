using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.Networking;

public class DatabaseTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //string conn = "URI=file:C:/Users/NikolaiMilanov/Documents/UnityProjects/Thesis/Assets/Database/Database.s3db"; //Path to database.
        //IDbConnection dbconn;
        //dbconn = (IDbConnection)new SqliteConnection(conn);
        //dbconn.Open(); //Open connection to the database.
        //IDbCommand dbcmd = dbconn.CreateCommand();
        //string sqlQuery = "SELECT FacebookID,Description,OnlineStatus " + "FROM Players" + " WHERE OnlineStatus = 0";
        //dbcmd.CommandText = sqlQuery;
        //IDataReader reader = dbcmd.ExecuteReader();
        //while (reader.Read())
        //{
        //    string favebookID = reader.GetString(0);
        //    string description = reader.GetString(1);
        //    int status = reader.GetInt32(2);

        //    Debug.Log("ID= " + favebookID + "  name =" + name + "  description =" + description + "  status =" + status);
        //}
        //reader.Close();
        //dbcmd.Dispose();
        //dbcmd = null;
        //reader = null;
        //dbconn.Close();
        //dbconn = null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
