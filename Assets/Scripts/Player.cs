using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerInfo
{
    public int id;
    public string username;
    public string password;
    public float points;
}

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    PlayerInfo _info = new PlayerInfo();
    Gun _equippedGun = new Gun();

    Rigidbody2D _rigidbody;
    float _shotTimer = 0f;

    [SerializeField] float _health;
    [SerializeField] float _speed;
    [SerializeField] LayerMask _hitLayer;
    [SerializeField] GameObject _shotPrefab;
    [SerializeField] TextMeshProUGUI _text;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        LoadInfo();
    }

    private void Update()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        Vector2 velocity = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            velocity += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            velocity += Vector2.down;
        if (Input.GetKey(KeyCode.A))
            velocity += Vector2.left;
        if (Input.GetKey(KeyCode.D))
            velocity += Vector2.right;

        velocity.Normalize();
        velocity *= _speed;

        _rigidbody.velocity = velocity;
    }

    private void Shoot()
    {
        _shotTimer += Time.deltaTime;

        if (_equippedGun == null)
            return;
        if (_shotTimer < _equippedGun.fireLimit)
            return;


        if (Input.GetMouseButton(0))
        {
            Debug.Log("Player: Shoot");
            _shotTimer = 0f;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - _rigidbody.position;
            direction.Normalize();
            RaycastHit2D result = Physics2D.Raycast(_rigidbody.position, direction, 100, _hitLayer);

            if (_shotPrefab != null)
            {
                Shot shot = Instantiate(_shotPrefab).GetComponent<Shot>();
                shot.SetPositions(_rigidbody.position, _rigidbody.position + (direction * 100));
            }

            if (result.rigidbody is null)
            {
                Debug.Log("Did not hit");
                return;
            }

            Enemy enemy = result.transform.gameObject.GetComponent<Enemy>();
            if (enemy is null)
                return;

            enemy.AddHurt(_equippedGun.damage);
        }
    }

    public void AddHurt(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            UpdatePoints();

            Debug.Log("Game Over");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
        }
    }

    public void AddPoints(int points)
    {
        _info.points += points;
        _text.text = "Points: " + _info.points;
    }

    public bool LoadInfo()
    {
        AppData.DBConnect(out SqliteConnection connection, out SqliteCommand cmd);

        cmd.CommandText = "select * from player where player.id = " + AppData.activePlayerID;
        IDataReader playerReader = cmd.ExecuteReader();
        playerReader.Read();
        _info.points = (float)playerReader["points"];
        _info.username = (string)playerReader["Username"];
        _info.password = (string)playerReader["Password"];
        _info.id = AppData.activePlayerID;
        playerReader.Close();

        cmd.CommandText = "select * from gun where name = '" + AppData.activeGunName + "';";
        IDataReader gunReader = cmd.ExecuteReader();
        gunReader.Read();
        _equippedGun.name = (string)gunReader["name"];
        _equippedGun.damage = (float)gunReader["damage"];
        _equippedGun.fireLimit = (float)gunReader["fireLimit"];
        _equippedGun.price = (float)gunReader["price"];
        gunReader.Close();

        AppData.DBClose(ref connection, ref cmd);

        _text.text = "Points: " + _info.points;

        return true;
    }

    public void GotoMainMenu()
    {
        UpdatePoints();
        SceneManager.LoadScene("Main Menu");
    }

    private void UpdatePoints()
    {
        AppData.DBConnect(out SqliteConnection connection, out SqliteCommand cmd);
        cmd.CommandText = "update player set points = " + _info.points + " where id = " + _info.id + ";";
        cmd.ExecuteNonQuery();
        AppData.DBClose(ref connection, ref cmd);
    }
}
