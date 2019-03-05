using UnityEngine;
using System.Collections;

public class MoveProjectile : MonoBehaviour {

    float firedTime;
    public float Speed;
    public Direction projectileDirection;

	// Use this for initialization
	void Start () {
        firedTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0, 0, Speed);

        if (Time.time - firedTime > 2f)
            Destroy(gameObject);
	}
}
