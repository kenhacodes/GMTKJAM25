using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaletteBlock : MonoBehaviour, IPointerDownHandler
{
    public GameObject blockPrefab;
    public Block blockInfo_;
    private CanvasGroup canvasGroup_;
    public Image image_;
    private TextMeshProUGUI text_;

    private WhileBlock _whileBlock;

    private RectTransform rectTr_;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup_ = gameObject.AddComponent<CanvasGroup>();
        rectTr_ = GetComponent<RectTransform>();

        image_ = GetComponentInChildren<Image>();
        var style = BlockStyle.Instance_.GetStyle(blockInfo_.blockType_, blockInfo_.actionType_);
        image_.sprite = style.Item1;
        image_.color = style.Item2;
        rectTr_.sizeDelta = new Vector2(blockInfo_.width_, blockInfo_.height_);

        text_ = GetComponentInChildren<TextMeshProUGUI>();
        text_.text = blockInfo_.text_;
        text_.ForceMeshUpdate();

        _whileBlock = FindObjectOfType<WhileBlock>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_whileBlock.eventZone_.transform.childCount > 15)
        {
            return;
        }
        
        GameObject clone = Instantiate(blockPrefab, transform.position, Quaternion.identity);

        clone.transform.SetParent(transform.parent, true);

        clone.transform.localScale = Vector3.one;
        //clone.transform.localRotation = Quaternion.identity;
        clone.transform.localPosition = new Vector2(clone.transform.localPosition.x, clone.transform.localPosition.y) + image_.rectTransform.sizeDelta * 0.025f;

        var block = clone.GetComponent<MovableBlock>();
        block.SetBlockStyle(blockInfo_);
        block.VariableSlotUpdate();
        _whileBlock.SetNewBlockInList(block);
    }
}
