using Mono.Data.Sqlite;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccountInfoHandler : MonoBehaviour
{

    [SerializeField] TMP_Text _accountInfo;

    // Start is called before the first frame update

    string _dbName = "URI=file:game.db";

    void Start()
    {
        SqliteConnection connection = new SqliteConnection(_dbName);
        connection.Open();
        SqliteCommand cmd = connection.CreateCommand();

        //test username
        cmd.CommandText = "select * from player where id = " + AppData.activePlayerID.ToString() + ";";
        IDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            _accountInfo.text = "Username: " + (string)reader["username"] + "\nPassword: "
                + (string)reader["password"] + "\nFirst Name: "
                + (string)reader["firstname"] + "\nLast Name: "
                + (string)reader["lastname"] + "\n Date of Birth: "
                + (string)reader["dateofbirth"] + "\nEmail: "
                + (string)reader["email"];
        }
        reader.Close();
        cmd.Dispose();
        connection.Close();
        connection.Dispose();
    }

    public void DeleteAndLogOut()
    {
        SqliteConnection connection = new SqliteConnection(_dbName);
        connection.Open();
        SqliteCommand cmd = connection.CreateCommand();

        cmd.CommandText = "delete from playergun where PlayerId = " + AppData.activePlayerID.ToString() + ";";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "delete from player where id = " + AppData.activePlayerID.ToString() + ";";
        cmd.ExecuteNonQuery();

        cmd.Dispose();
        connection.Close();
        connection.Dispose();

        SceneManager.LoadScene(0);
    }
}
