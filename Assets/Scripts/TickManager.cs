using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    private static TickManager _instance;

    public static TickManager Instance_
    {
        get
        {
            if (_instance == null)
                _instance = new TickManager();
            return _instance;
        }
    }

    private TickManager()
    {
    }

    public MovementList playerMoveSet_;
    public MovementList enemyMoveSet_;

    private int i_player = 0;
    private int i_enemy = 0;

    public CombatManager combatman;

    public float tickTime = 0.5f;

    public bool tickingActive = false;

    private bool combatManagerFound = false;

    // Start is called before the first frame update
    void Start()
    {
    }


    public IEnumerator DoTick()
    {
        if (!combatManagerFound)
        {
            combatman = FindObjectOfType<CombatManager>();
            if (combatman != null)
            {
                combatManagerFound = true;
            }
        }

        if (playerMoveSet_.movements_.Count == 0)
        {
            Debug.Log("0 MOVEMENTS PLAYER");
        }
        
        if (combatManagerFound)
        {
            // Player Action
            if (playerMoveSet_.movements_.Count > 0)
            {
                if (i_player >= playerMoveSet_.movements_.Count)
                    i_player = 0;

                var move = playerMoveSet_.movements_[i_player];

                switch (move.action_)
                {
                    case Action.Action_MoveBackwards:
                        combatman.PlayerRobot.MoveRobot(RobotController.Directions.Backwards);
                        break;

                    case Action.Action_MoveForwards:
                        combatman.PlayerRobot.MoveRobot(RobotController.Directions.Forward);
                        break;

                    case Action.Action_MoveLeft:
                        combatman.PlayerRobot.MoveRobot(RobotController.Directions.Left);
                        break;

                    case Action.Action_MoveRight:
                        combatman.PlayerRobot.MoveRobot(RobotController.Directions.Right);
                        break;

                    case Action.Action_AttackNormal:
                        combatman.PlayerRobot.AttackNormal();
                        break;

                    case Action.Action_AttackLow:
                        combatman.PlayerRobot.AttackLow();
                        break;

                    case Action.Action_AttackLeft:
                        combatman.PlayerRobot.AttackLeft();
                        break;

                    case Action.Action_AttackRight:
                        combatman.PlayerRobot.AttackRight();
                        break;

                    case Action.Action_AttackUppercut:
                        combatman.PlayerRobot.AttackUppercut();
                        break;

                    case Action.Action_BlockNormal:
                        combatman.PlayerRobot.BlockNormal();
                        break;

                    case Action.Action_BlockLow:
                        combatman.PlayerRobot.BlocKLow();
                        break;

                    case Action.Action_JumpForward:
                        combatman.PlayerRobot.JumpFront();
                        break;

                    default:
                        Debug.Log("Player: No Action");
                        break;
                }

                if (move.action_ != Action.Action_AttackLeft || move.action_ != Action.Action_AttackRight)
                {
                    combatman.PlayerRobot.LookAtEnemy();
                }
                                combatman.PlayerRobot.RobotKeyRotation();


                i_player++;
            }

            // Enemy Action
            if (enemyMoveSet_.movements_.Count > 0)
            {
                if (i_enemy >= enemyMoveSet_.movements_.Count)
                    i_enemy = 0;

                var move = enemyMoveSet_.movements_[i_enemy];

                switch (move.action_)
                {
                    case Action.Action_MoveBackwards:
                        combatman.EnemyRobot.MoveRobot(RobotController.Directions.Backwards);
                        break;

                    case Action.Action_MoveForwards:
                        combatman.EnemyRobot.MoveRobot(RobotController.Directions.Forward);
                        break;

                    case Action.Action_MoveLeft:
                        combatman.EnemyRobot.MoveRobot(RobotController.Directions.Left);
                        break;

                    case Action.Action_MoveRight:
                        combatman.EnemyRobot.MoveRobot(RobotController.Directions.Right);
                        break;

                    case Action.Action_AttackNormal:
                        combatman.EnemyRobot.AttackNormal();
                        break;

                    case Action.Action_AttackLow:
                        combatman.EnemyRobot.AttackLow();
                        break;

                    case Action.Action_AttackLeft:
                        combatman.EnemyRobot.AttackLeft();
                        break;

                    case Action.Action_AttackRight:
                        combatman.EnemyRobot.AttackRight();
                        break;

                    case Action.Action_AttackUppercut:
                        combatman.EnemyRobot.AttackUppercut();
                        break;

                    case Action.Action_BlockNormal:
                        combatman.EnemyRobot.BlockNormal();
                        break;

                    case Action.Action_BlockLow:
                        combatman.EnemyRobot.BlocKLow();
                        break;

                    case Action.Action_JumpForward:
                        combatman.EnemyRobot.JumpFront();
                        break;

                    default:
                        Debug.Log("Enemy: No Action");
                        break;
                }

                if (move.action_ != Action.Action_AttackLeft || move.action_ != Action.Action_AttackRight)
                {
                    combatman.EnemyRobot.LookAtEnemy();
                }
                combatman.EnemyRobot.RobotKeyRotation();
                
                i_enemy++;
            }
        }

        yield return new WaitForSeconds(tickTime);
        if (tickingActive)
        {
            StartCoroutine(DoTick());
        }
        
    }


    // Update is called once per frame
    void Update()
    {
    }
}