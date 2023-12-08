using UnityEngine;

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
    PlayerInfo _info;
    Gun _equippedGun = null;

    Rigidbody2D _rigidbody;
    float _shotTimer = 0f;

    [SerializeField] float speed;

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
        velocity *= speed;

        _rigidbody.velocity = velocity;
    }

    private void Shoot()
    {
        if (_equippedGun == null)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - _rigidbody.position;
        direction.Normalize();
        RaycastHit2D result = Physics2D.Raycast(_rigidbody.position, direction);
    }

    /// <summary>
    /// Loads Info from database
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool LoadInfo(string username, string password)
    {
        // TODO
        return false;
    }

    public bool Equip(string gunName)
    {
        //TODO Check if player ownes gun and equip
        return false;
    }
}
