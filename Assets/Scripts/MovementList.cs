using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public enum Action
{
    Action_None = 0,
    Action_MoveBackwards,
    Action_MoveForwards,
    Action_MoveLeft,
    Action_MoveRight,
    Action_AttackNormal,
    Action_AttackLow,
    Action_AttackLeft,
    Action_AttackRight,
    Action_AttackUppercut,
    Action_BlockNormal,
    Action_BlockLow,
    Action_JumpForward,
};


[CreateAssetMenu(fileName = "New Movement List", menuName = "MovementList")]
public class MovementList : ScriptableObject
{
    [System.Serializable]
    public class Movement
    {
        public Action action_;
        public float execTime_;

        public Movement(Action action, float time)
        {
            action_ = action;
            execTime_ = time;
        }
    };

    public List<Movement> movements_;

    void Awake()
    {
        movements_ = new List<Movement>();    
    }

    public void AddMove(ActionType type, RobotController.Directions directions, float time)
    {
        movements_.Add(new Movement(ActionTypeToAction(type, directions), time));
        
    }

    public Action ActionTypeToAction(ActionType type, RobotController.Directions directions)
    {
        switch (type)
        {
            case ActionType.ActionType_Move:
                switch (directions)
                {
                    case RobotController.Directions.Backwards:
                        return Action.Action_MoveBackwards;
                        
                    case RobotController.Directions.Left:
                        return Action.Action_MoveLeft;

                    case RobotController.Directions.Right:
                        return Action.Action_MoveRight;

                    default:
                        return Action.Action_MoveForwards;
                }
            case ActionType.ActionType_Defend:
                switch (directions)
                {
                    case RobotController.Directions.Backwards:
                        return Action.Action_BlockLow;
                    default: return Action.Action_BlockNormal;
                }
            
            case ActionType.ActionType_Attack:
                switch (directions)
                {
                    case RobotController.Directions.Forward:
                        return Action.Action_AttackUppercut;
                        
                    case RobotController.Directions.Backwards:
                        return Action.Action_AttackLow;
                        
                    case RobotController.Directions.Right:
                        return Action.Action_AttackRight;
                        
                    case RobotController.Directions.Left:
                        return Action.Action_AttackLeft;

                    default: return Action.Action_AttackNormal;
                }

            case ActionType.ActionType_Jump:
                return Action.Action_JumpForward;

            default:
                return Action.Action_None;
        }
    }
}
