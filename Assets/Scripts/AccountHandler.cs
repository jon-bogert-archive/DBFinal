using Mono.Data.Sqlite;
using System.Data;
using TMPro;
using UnityEngine;


public class AccountHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField _usernameInput;
    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] TMP_InputField _playerNameInput;
    [SerializeField] TMP_Text _resultText;

    string _dbName = "URI=file:game.db";

    public void AddEntry()
    {
        if (_usernameInput.text == "" || _passwordInput.text == "" || _playerNameInput.text == "")
        {
            _resultText.text = "One for the required fields was empty";
            return;
        }

        SqliteConnection connection = new SqliteConnection(_dbName);
        connection.Open();
        SqliteCommand cmd = connection.CreateCommand();

        //test username
        cmd.CommandText = "select * from player where username = '" + _usernameInput.text + "';";
        IDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            _resultText.text = "Username already exists...";
            reader.Close();
            cmd.Dispose();
            connection.Close();
            connection.Dispose();
            return;
        }
        reader.Close();

        cmd.CommandText = "insert into users (username, password, playerName)" +
            "values ('" + _usernameInput.text + "', '" + _passwordInput.text + "', '" + _playerNameInput.text + "');";
        cmd.ExecuteNonQuery();

        cmd.Dispose();
        connection.Close();
        connection.Dispose();

        _resultText.text = "Thank you for rigistering " + _playerNameInput.text + "!";
    }

    public void ShowEntry()
    {
        if (_usernameInput.text == "" || _passwordInput.text == "")
        {
            _resultText.text = "Username or Password was empty";
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
                _resultText.text = "incorrect password for user: " + _usernameInput.text;
                reader.Close();
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
                return;
            }
            string name = (string)reader["playerName"];

            _resultText.text = "Welcome " + name + "!";
        }
        else
        {
            _resultText.text = "No user with username: " + _usernameInput.text;
        }

        reader.Close();
        cmd.Dispose();
        connection.Close();
        connection.Dispose();
    }
}
