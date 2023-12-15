using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Transform _content;
    [SerializeField] GameObject _gunButtonPrefab;
    [SerializeField] GameObject _notEnoughMenu;
    [SerializeField] TMP_Text _notEnoughInfo;
    [SerializeField] ShopConfirm _purchaseMenu;
    [SerializeField] TMP_Text _pointsText;

    List<GunButton> _gunButtons = new List<GunButton>();

    private void Start()
    {
        HideNotEnoughMenu();
        UpdatePointsText();

        AppData.DBConnect(out SqliteConnection connection, out SqliteCommand cmd);

        cmd.CommandText = "select * from Gun;";
        IDataReader reader = cmd.ExecuteReader();

        //get and instantiate all gun buttons
        while (reader.Read())
        {
            Gun newGun = new Gun();
            newGun.name = (string)reader["name"];
            newGun.damage = (float)reader["damage"];
            newGun.fireLimit = (float)reader["fireLimit"];
            newGun.price = (float)reader["price"];

            GunButton newButton = Instantiate(_gunButtonPrefab, _content).GetComponent<GunButton>();
            newButton.gun = newGun;
            newButton.onPress += HandleButton;

            _gunButtons.Add(newButton);
        }
        reader.Close();

        //check if player owns the gun
        for (int i = 0; i < _gunButtons.Count; ++i)
        {
            cmd.CommandText = "select * from PlayerGun where playerid = " + AppData.activePlayerID +
                " and gunname = \"" + _gunButtons[i].gun.name +"\";";

            reader = cmd.ExecuteReader();
            if (reader.Read()) // Entry Exists (Player owns Gun)
            {
                _gunButtons[i].isPurchased = true;
            }
            reader.Close();
        }

        reader.Close();
        AppData.DBClose(ref connection, ref cmd);
    }

    private void HandleButton(bool isPurchased, Gun gun)
    {
        if (isPurchased)
        {
            AppData.activeGunName = gun.name;
            UpdateButtons();
            return;
        }
        AppData.DBConnect(out SqliteConnection connection, out SqliteCommand cmd);

        cmd.CommandText = "select points from player where id = " + AppData.activePlayerID + ";";
        IDataReader reader = cmd.ExecuteReader();
        reader.Read();
        float playerPoints = (float)reader["points"];
        reader.Close();

        if (gun.price > playerPoints) // Player doesn't have enough points
        {
            _notEnoughInfo.text = "Gun: " + gun.name + "\nPrice: " + gun.price;
            _notEnoughMenu.SetActive(true);
        }
        else
        {
            _purchaseMenu.Begin(gun);
            _purchaseMenu.onConfirm += Purchase;
        }

        AppData.DBClose(ref connection, ref cmd);
    }

    private void UpdateButtons()
    {
        AppData.DBConnect(out SqliteConnection connection, out SqliteCommand cmd);
        foreach (GunButton button in _gunButtons)
        {
            button.UpdateBackground();
            cmd.CommandText = "select * from PlayerGun where playerid = " + AppData.activePlayerID +
                " and gunname = \"" + button.gun.name + "\";";

            IDataReader reader = cmd.ExecuteReader();
            if (reader.Read()) // Entry Exists (Player owns Gun)
            {
                button.isPurchased = true;
            }
            reader.Close();
        }
        AppData.DBClose(ref connection, ref cmd);
    }

    public void HideNotEnoughMenu()
    {
        _notEnoughMenu.SetActive(false);
    }

    void Purchase(Gun gun)
    {
        AppData.DBConnect(out SqliteConnection connection, out SqliteCommand cmd);
        cmd.CommandText = "select points from player where id = " + AppData.activePlayerID + ";";
        IDataReader reader = cmd.ExecuteReader();
        reader.Read();
        float playerPoints = (float)reader["points"];
        reader.Close();
        playerPoints -= gun.price;
        cmd.CommandText = "update player set points = " + playerPoints + " where id = "
            + AppData.activePlayerID + ";";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "insert into PlayerGun (PlayerId, GunName) " +
            "values (" + AppData.activePlayerID + ", \"" + gun.name + "\");";
        cmd.ExecuteNonQuery();

        AppData.DBClose(ref connection, ref cmd);
        UpdatePointsText();
        UpdateButtons();
    }
    void UpdatePointsText()
    {
        AppData.DBConnect(out SqliteConnection connection, out SqliteCommand cmd);

        cmd.CommandText = "select points from player where id = " + AppData.activePlayerID + ";";
        IDataReader reader = cmd.ExecuteReader();
        reader.Read();
        float playerPoints = (float)reader["points"];
        reader.Close();

        _pointsText.text = "Player Points: " + playerPoints;

        AppData.DBClose(ref connection, ref cmd);
    }

    public void EnterMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
