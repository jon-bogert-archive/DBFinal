using Mono.Data.Sqlite;
using System.Data;
using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
    string _dbName = "URI=file:game.db";

    [SerializeField] TMP_InputField _usernameInput;
    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] TMP_Text outputText;

    public void ShowEntry()
    {
        if (_usernameInput.text == "" || _passwordInput.text == "")
        {
            outputText.text = "Username or Password was empty";
            return;
        }

        SqliteConnection connection = new SqliteConnection(_dbName);
        connection.Open();
        SqliteCommand cmd = connection.CreateCommand();

        cmd.CommandText = "select * from users where username = '" + _usernameInput.text + "';";
        IDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            string password = (string)reader["password"];
            if (password != _passwordInput.text)
            {
                outputText.text = "incorrect password for user: " + _usernameInput.text;
                reader.Close();
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
                return;
            }
            string name = (string)reader["playerName"];

            outputText.text = "Welcome " + name + "!";
        }
        else
        {
            outputText.text = "No user with username: " + _usernameInput.text;
        }

        reader.Close();
        cmd.Dispose();
        connection.Close();
        connection.Dispose();
    }
}
