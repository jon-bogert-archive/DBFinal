using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Shot : MonoBehaviour
{
    [SerializeField] float _timer = 0.1f;

    private void Update()
    {
        if (_timer < 0)
            Destroy(gameObject);

        _timer -= Time.deltaTime;
    }

    public void SetPositions(Vector2 origin, Vector2 end)
    {
        LineRenderer renderer = GetComponent<LineRenderer>();
        Vector3[] pos = new Vector3[2];
        pos[0] = origin;
        pos[1] = end;
        renderer.SetPositions(pos);
    }
}
