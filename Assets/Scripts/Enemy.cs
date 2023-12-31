using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
	[SerializeField] private float _speed;
	[SerializeField] private float _damage;
	[SerializeField] private float _health;
	[SerializeField] private Rigidbody2D _rb2D;
	
	private Player _player;
	
	private void Awake()
	{
		_player = FindObjectOfType<Player>();
	}
	
	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
		_rb2D.velocity = new Vector2(0f, 0f);
	}

	public void AddHurt(float damage)
    {
		Debug.Log("enemy hit");
		_health -= damage;
		if (_health <= 0)
		{
			_player.AddPoints(1);
			Debug.Log("enemy died");
			Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (!collision.collider.CompareTag("Player"))
			return;

		_player.AddHurt(_damage);
    }
}