using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WhileBlock : MonoBehaviour, IDropHandler
{

    private RectTransform tr_;

    public GameObject eventZone_;

    public MovementList playerMoveSet_;

    // Start is called before the first frame update
    void Start()
    {
        tr_ = GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        // Find a way to optimize
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(tr_);

        if (Input.GetKeyDown(KeyCode.Space)) ReadBlocks();
    }

    public void ReadBlocks()
    {
        playerMoveSet_.movements_.Clear();

        foreach (Transform child in eventZone_.transform)
        {
            MovableBlock block = child.GetComponent<MovableBlock>();
            if (block != null)
            {
                RobotController.Directions dir = RobotController.Directions.Forward;
                if (block.blockInfo_.blockType_ == BlockType.BlockType_ActionWithVariable)
                {
                    VariableSlot variable = block.GetComponentInChildren<VariableSlot>();

                    if (variable != null)
                    {
                        string dir_txt = variable.text_.text;
                        Debug.Log(dir_txt);
                        switch (dir_txt)
                        {
                            case "Backwards":
                                dir = RobotController.Directions.Backwards;
                                break;

                            case "Right":
                                dir = RobotController.Directions.Right;
                                break;

                            case "Left":
                                dir = RobotController.Directions.Left;
                                break;

                            default:
                                dir = RobotController.Directions.Forward;
                                break;
                        }
                    }
                }

                playerMoveSet_.AddMove(block.blockInfo_.actionType_, dir, block.blockInfo_.executionTime_);
            }
        }

    }

    public void OnDrop(PointerEventData eventData)
    {

        MovableBlock block = eventData.pointerDrag?.GetComponent<MovableBlock>();

        if (block != null)
        {
            if (block.blockInfo_.blockType_ != BlockType.BlockType_Variable)
            {
                block.transform.SetParent(eventZone_.transform);

                int newIndex = eventZone_.transform.childCount;

                Vector3 worldMousePos = eventData.pointerCurrentRaycast.worldPosition;

                foreach (Transform child in eventZone_.transform)
                {
                    if (worldMousePos.y > child.position.y) 
                    {
                        newIndex = child.GetSiblingIndex();
                        break;
                    }
                }

                block.transform.SetSiblingIndex(newIndex);

                // let the layout system handle position, don't zero it manually
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(eventZone_.GetComponent<RectTransform>());
            }

        }
    }
}
