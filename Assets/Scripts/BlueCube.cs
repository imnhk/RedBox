using UnityEngine;
using System.Collections;

public enum EnemyStatus { DEFAULT, STEADY }

public class BlueCube : MonoBehaviour {

    public GameObject blueCube;

    private Vector3 CurrentScreenPos;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        CurrentScreenPos = Camera.main.WorldToScreenPoint(transform.position);

        if(CurrentScreenPos.x < 0 || CurrentScreenPos.y < 0 || CurrentScreenPos.x > Screen.width || CurrentScreenPos.y > Screen.height)
            Destroy(gameObject);
	}

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Projectile")
        {
            GameManager.score += 1;
            Destroy(gameObject);
        }
    }

    public IEnumerator SteadyMoveFrom(Vector3 spawnPos, Direction movingDirection, float speed)
    {
        GameObject newCube = (GameObject)Instantiate(blueCube, spawnPos, Quaternion.identity);
        
        while(newCube != null)
        {
            newCube.transform.Translate(DirectionToVector3(movingDirection, speed * Time.deltaTime));

            yield return null;
        }
    }

    Vector3 DirectionToVector3(Direction dir, float size)
    {
        switch (dir)
        {
            case Direction.UP:
                return new Vector3(0, 0, size);
            case Direction.DOWN:
                return new Vector3(0, 0, -size);
            case Direction.LEFT:
                return new Vector3(-size, 0, 0);
            case Direction.RIGHT:
                return new Vector3(size, 0, 0);

            default:
                Debug.Log("Impossible input at directionToVector3");
                return new Vector3(0, 0, 0);
        }
    }
}
