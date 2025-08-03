using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VariableSlot : MonoBehaviour, IDropHandler
{

    public MovableBlock currentBlock;
    private RectTransform rectTr_;
    public TextMeshProUGUI text_;

    public void OnDrop(PointerEventData eventData)
    {
        MovableBlock droppedBlock = eventData.pointerDrag.GetComponent<MovableBlock>();
        if (droppedBlock != null && droppedBlock.blockInfo_.blockType_ == BlockType.BlockType_Variable)
        {
            droppedBlock.transform.SetParent(transform, false);

            droppedBlock.transform.localPosition = Vector3.zero;
            droppedBlock.transform.localScale = Vector3.one;

            text_ = droppedBlock.GetComponentInChildren<TextMeshProUGUI>();

            // Right padding
            float padding = -30f;

            // Get parent width
            float parentWidth = ((RectTransform)transform).rect.width;

            // Place variable slot near right side
            rectTr_.anchoredPosition = new Vector2(parentWidth / 2 - rectTr_.rect.width / 2 - padding, rectTr_.anchoredPosition.y);

            currentBlock = droppedBlock;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTr_ = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
