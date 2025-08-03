using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using System;
using TMPro;

public class MovableBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    private RectTransform rectTr_;
    private Canvas canvas_;
    private CanvasGroup canvasGroup_;
    public Image image_;
    private TextMeshProUGUI text_;
    public Block blockInfo_;

    void Awake()
    {
        rectTr_ = GetComponent<RectTransform>();

        // Make sure the block always has a CanvasGroup
        canvasGroup_ = GetComponent<CanvasGroup>();
        if (canvasGroup_ == null)
            canvasGroup_ = gameObject.AddComponent<CanvasGroup>();

        // Find parent canvas so we can move block to it while dragging
        canvas_ = GetComponentInParent<Canvas>();

        // set block style
        //SetBlockStyle(blockInfo_);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBlockStyle(Block block)
    {
        image_ = GetComponentInChildren<Image>();
        var style = BlockStyle.Instance_.GetStyle(block.blockType_, block.actionType_);
        image_.sprite = style.Item1;
        image_.color = style.Item2;
        rectTr_.sizeDelta = new Vector2(block.width_, block.height_);

        text_ = GetComponentInChildren<TextMeshProUGUI>();
        text_.text = block.text_;
        text_.ForceMeshUpdate();

        blockInfo_ = block;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas_ == null) canvas_ = GetComponentInParent<Canvas>();

        canvasGroup_.blocksRaycasts = false;
        transform.SetParent(canvas_.transform, true);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTr_.anchoredPosition += eventData.delta / canvas_.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup_.blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Destroy(gameObject);
        }
    }

    public void VariableSlotUpdate()
    { 
        Transform slotTransform = transform.Find("VariableSlot");

        if (slotTransform != null)
        {
            if (blockInfo_.blockType_ != BlockType.BlockType_ActionWithVariable)
                Destroy(slotTransform.gameObject);
        }
    }
}
