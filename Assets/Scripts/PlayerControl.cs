using UnityEngine;
using System.Collections;

public enum Direction { NONE, UP, DOWN, LEFT, RIGHT };
public enum PlayerState { DEFAULT, ATTACKING, MOVING };
public enum SpecialState { NONE, FOCUSING };

public class PlayerControl : MonoBehaviour {

    public class PlayerStatus
    {
        public PlayerState status;
        public SpecialState status_special;
        public Direction lookingDirection;
        public Vector3 CurrentScreenPos;
        public int health;

        public float DefaultMoveDistance;
        public float CurrentMoveDistance;
        public float DefaultMoveTime;
        public float CurrentMoveTime;
    }  

    public Direction inputDirection;
    
    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private float movedTime;

    public GameObject defaultProjectile;

    public PlayerStatus player = new PlayerStatus();

	void Start ()
    {
        player.status = PlayerState.DEFAULT;
        player.status_special = SpecialState.NONE;
        player.health = 4;

        player.DefaultMoveDistance = 1.4f;
        player.DefaultMoveTime = 0.12f;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(true) //무적시간 추가할 자리
        {
            if (collider.tag == "Enemy")
            {
                switch(player.health)
                {
                    case 4:
                        gameObject.transform.localScale = new Vector3(1.25f, 1, 1.25f);
                        player.DefaultMoveDistance = 1.1f;
                        player.DefaultMoveTime = 0.09f;
                        break;

                    case 3:
                        gameObject.transform.localScale = new Vector3(1.00f, 1, 1.00f);
                        player.DefaultMoveDistance = 0.8f;
                        player.DefaultMoveTime = 0.06f;
                        break;

                    case 2:
                        gameObject.transform.localScale = new Vector3(0.75f, 1, 0.75f);
                        player.DefaultMoveDistance = 0.6f;
                        player.DefaultMoveTime = 0.03f;
                        break;

                    case 1:
                        Destroy(gameObject);
                        break;
                }
                player.health--;
            }
        }
    }

    void Update()
    {
        player.CurrentScreenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (player.CurrentScreenPos.x < 0 || player.CurrentScreenPos.y < 0 || player.CurrentScreenPos.x > Screen.width || player.CurrentScreenPos.y > Screen.height)
            Destroy(gameObject);

        switch (player.status)
        {
            case PlayerState.DEFAULT:
                inputDirection = GetDirectionInput();
                SpecialMove();

                if (inputDirection == Direction.NONE)
                    break;
                else
                    player.status = PlayerState.ATTACKING;

                break;

            case PlayerState.ATTACKING:
                switch (player.status_special)
                {
                    case SpecialState.NONE:
                        DefaultAttack();
                        break;

                    case SpecialState.FOCUSING:
                        FocusAttack();
                        break;
                }
                break;

            case PlayerState.MOVING:
                Move(inputDirection, player.CurrentMoveDistance);
                break;
        }

        switch (player.status_special)
        {
            case SpecialState.NONE:
                player.CurrentMoveTime = player.DefaultMoveTime;
                player.CurrentMoveDistance = player.DefaultMoveDistance;
                break;

            case SpecialState.FOCUSING:
                player.CurrentMoveTime = player.DefaultMoveTime / 2;
                player.CurrentMoveDistance = player.DefaultMoveDistance / 3;
                break;
        }

    }

    private void FocusAttack()
    {
        Shoot(player.lookingDirection);
        player.status = PlayerState.MOVING;

        startingPosition = transform.position;
        targetPosition = startingPosition + DirectionToVector3(inputDirection, player.CurrentMoveDistance);
        movedTime = 0;
    }

    private void DefaultAttack()
    {
        player.lookingDirection = inputDirection;
        Shoot(player.lookingDirection);
        player.status = PlayerState.MOVING;

        startingPosition = transform.position;
        targetPosition = startingPosition + DirectionToVector3(inputDirection, player.CurrentMoveDistance);
        movedTime = 0;
    }

    Direction GetDirectionInput()
    {

        if (Input.GetKey("w"))
            return Direction.UP;

        if (Input.GetKey("s"))
            return Direction.DOWN;

        if (Input.GetKey("a"))
            return Direction.LEFT;

        if (Input.GetKey("d"))
            return Direction.RIGHT;

        //입력이 없는 경우
        return Direction.NONE;
    }

    void SpecialMove()
    {
        if (Input.GetKey("space"))
        {
            player.status_special = SpecialState.FOCUSING;
        }
        else
        {
            player.status_special = SpecialState.NONE;
        }
    }


    void Move(Direction direction, float distance)
    {
        movedTime += Time.deltaTime;
        transform.position = Vector3.Lerp(startingPosition, targetPosition, movedTime / player.CurrentMoveTime);

        if(transform.position == targetPosition)
        {
            player.status = PlayerState.DEFAULT;
        }
    }

    void Shoot(Direction direction)
    {
        GameObject projectile = (GameObject)Instantiate(defaultProjectile, transform.position, Quaternion.identity);

        switch(direction)
        {
            case Direction.UP:
                projectile.transform.Rotate(new Vector3(0, 0, 0));
                break;

            case Direction.DOWN:
                projectile.transform.Rotate(new Vector3(0, 180, 0));
                break;

            case Direction.LEFT:
                projectile.transform.Rotate(new Vector3(0, 270, 0));
                break;

            case Direction.RIGHT:
                projectile.transform.Rotate(new Vector3(0, 90, 0));
                break;
        }
        projectile.GetComponent<MoveProjectile>().projectileDirection = direction;
    }

    Vector3 DirectionToVector3(Direction dir, float size)
    {
        switch(dir)
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