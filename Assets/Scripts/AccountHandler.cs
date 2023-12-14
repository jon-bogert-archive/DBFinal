using Mono.Data.Sqlite;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AccountHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField _usernameInput;
    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] TMP_Text _resultText;

    string _dbName = "URI=file:game.db";

    public void GoToRegister()
    {
        SceneManager.LoadScene(3);
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

        cmd.CommandText = "select * from player where username = '" + _usernameInput.text + "';";
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
            string name = (string)reader["username"];

            _resultText.text = "Welcome " + name + "!";

            AppData.activePlayerID = reader.GetInt32(reader.GetOrdinal("id"));

            SceneManager.LoadScene(1);
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
