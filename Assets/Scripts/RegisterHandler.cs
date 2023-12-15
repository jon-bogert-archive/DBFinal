using Mono.Data.Sqlite;
using System.Data;
using TMPro;
using UnityEngine;

public class RegisterHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_InputField _usernameInput;
    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] TMP_InputField _playerFirstNameInput;
    [SerializeField] TMP_InputField _playerLastNameInput;
    [SerializeField] TMP_InputField _dobInput;
    [SerializeField] TMP_InputField _emailInput;
    [SerializeField] TMP_Text _resultText;

    string _dbName = "URI=file:game.db";

    public void AddEntry()
    {
        if (_usernameInput.text == "" || _passwordInput.text == "" || _playerFirstNameInput.text == "" || _playerLastNameInput.text == "" || _dobInput.text == "" || _emailInput.text == "")
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

        cmd.CommandText = "insert into player (username, password, FirstName, LastName, dateofbirth, email)" +
            "values ('" + _usernameInput.text + "', '" + _passwordInput.text + "', '" + _playerFirstNameInput.text + "', '" + _playerLastNameInput.text + "', '" + _dobInput.text + "', '" + _emailInput.text + "');";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "select * from player where username = '" + _usernameInput.text + "';";
        reader = cmd.ExecuteReader();
        reader.Read();
        AppData.activePlayerID = reader.GetInt32(reader.GetOrdinal("id"));
        reader.Close();

        cmd.CommandText = "insert into PlayerGun(PlayerId, GunName) values (" + AppData.activePlayerID + ", 'Pistol');";
        cmd.ExecuteNonQuery();

        cmd.Dispose();
        connection.Close();
        connection.Dispose();

        _resultText.text = "Thank you for registering " + _playerFirstNameInput.text + "!";
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
