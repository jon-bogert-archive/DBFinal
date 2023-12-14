using UnityEngine;
using TMPro;


[System.Serializable]
public class PlayerInfo
{
    public int id;
    public string username;
    public string password;
    public int points;
}

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    PlayerInfo _info = new PlayerInfo();
    [SerializeField] Gun _equippedGun = null;

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
            Debug.Log("Game Over");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
    }

    public void AddPoints(int points)
    {
        _info.points += points;
        _text.text = "Scores: " + _info.points;
    }

    public bool LoadInfo(string username, string password)
    {
        // TODO Load from Database
        return false;
    }

    public bool Equip(string gunName)
    {
        //TODO Check if player ownes gun and equip
        return false;
    }
}