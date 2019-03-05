using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    public enum Pattern { NONE, RANDOM };

    public Pattern curPattern;
    public GameObject blueCube;
    private BlueCube blueCubeScript;
    //public SpawnType type;

    private Vector3 spawnPos;
    private Vector3 bottomLeft_world;
    private Vector3 upperRight_world;
    private float mapHeight;
    private float mapWidth;

    public int RandomPatternFrequency;

    int counter = 0; //게임 실행 프레임을 센다. 현재 패턴을 시간에 따라 바꾸는 데 사용중.

	// Use this for initialization
	void Start () {
        bottomLeft_world = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        upperRight_world = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        mapHeight = upperRight_world.z - bottomLeft_world.z;
        mapWidth = upperRight_world.x - bottomLeft_world.x;

        blueCubeScript = blueCube.GetComponent<BlueCube>();

        curPattern = Pattern.RANDOM;
        RandomPatternFrequency = 20; 
	}
	
	// Update is called once per frame
	void Update () {
        counter++;

        //패턴 바꾸는 함수 필요

        switch(curPattern)
        {
            case Pattern.NONE:
                break;
                
            case Pattern.RANDOM:
                if (counter % RandomPatternFrequency == 0)
                    Pattern_Random();
                break;

            default:
                Debug.Log("curPattern Error");
                break;
        }       
	}

    void Pattern_Random() //무작위 방향에서 BlueCube 하나가 튀어나옴
    {
        int spawnDirection = Random.Range((int)Direction.NONE, (int)Direction.RIGHT + 1); //무작위로 방향 생성
        Vector3 spawnPos = new Vector3(Random.Range(bottomLeft_world.x, upperRight_world.x), 0, Random.Range(bottomLeft_world.z, upperRight_world.z));

        switch(spawnDirection)
        {
            case 1:
                spawnPos.z = bottomLeft_world.z;
                StartCoroutine(blueCubeScript.SteadyMoveFrom(spawnPos, Direction.UP, 10f));
                break;
            case 2:
                spawnPos.z = upperRight_world.z;
                StartCoroutine(blueCubeScript.SteadyMoveFrom(spawnPos, Direction.DOWN, 10f));
                break;
            case 3:
                spawnPos.x = upperRight_world.x;
                StartCoroutine(blueCubeScript.SteadyMoveFrom(spawnPos, Direction.LEFT, 10f));
                break;
            case 4:
                spawnPos.x = bottomLeft_world.x;
                StartCoroutine(blueCubeScript.SteadyMoveFrom(spawnPos, Direction.RIGHT, 10f));
                break;
        }
    }

    /*
    IEnumerator LinearRight(int count)
    {
        Debug.Log("LR");
        spawnPos.x = bottomLeft_world.x;
        spawnPos.z = upperRight_world.z;

        for (; spawnPos.z > bottomLeft_world.z; spawnPos.z -= mapHeight / count)
        {
            StartCoroutine(blueCubeScript.SteadyMoveFrom(spawnPos, Direction.RIGHT, 10f));
          
            yield return new WaitForSeconds(0.08f);
        }
    }

    IEnumerator LinearLeft(int count)
    {
        Debug.Log("LL");
        spawnPos.x = upperRight_world.x;
        spawnPos.z = bottomLeft_world.z;

        for (; spawnPos.z < upperRight_world.z; spawnPos.z += mapHeight / count)
        {
            StartCoroutine(blueCubeScript.SteadyMoveFrom(spawnPos, Direction.LEFT, 10f));

            yield return null;
        }
    }

    
    IEnumerator Pattern1(Direction direction, int count)
    {
        switch (direction)
        {
            case Direction.RIGHT:
                {
                    spawnPos.x = bottomLeft_world.x;
                    spawnPos.z = upperRight_world.z;

                    for (; spawnPos.z > bottomLeft_world.z; spawnPos.z -= mapHeight / count)
                    {
                        StartCoroutine(blueCubeScript.SteadyMoveFrom(spawnPos, direction, 10f));

                        yield return new WaitForSeconds(0.07f);
                    }
                    break;
                }
            case Direction.LEFT:
                {
                    spawnPos.x = upperRight_world.x;
                    spawnPos.z = bottomLeft_world.z;

                    for (; spawnPos.z < upperRight_world.z; spawnPos.z += mapHeight / count)
                    {
                        StartCoroutine(blueCubeScript.SteadyMoveFrom(spawnPos, direction, 10f));

                        yield return new WaitForSeconds(0.11f);
                    }
                    break;
                }
        }
    } */

}
