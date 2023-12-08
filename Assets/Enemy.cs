using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float _speed;
	[SerializeField] private float _damage;
	
	private Player _player;
	
	private void Awake()
	{
		_player = FindObjectOfType<Player>();
	}
	
	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
	}
}