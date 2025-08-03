using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public enum BlockType
{ 
    BlockType_None = 0,
    BlockType_SimpleAction,
    BlockType_ActionWithVariable,
    BlockType_Variable,
};

public enum ActionType
{ 
    ActionType_None = 0,
    ActionType_Move,
    ActionType_Attack,
    ActionType_Defend,
    ActionType_Jump,
};

[CreateAssetMenu(fileName = "New Block Style Set", menuName = "Block/Style Set")]
public class BlockStyle : ScriptableObject
{

    private static BlockStyle instance_;
    public static BlockStyle Instance_
    {
        get
        {
            if (instance_ == null)
            {
                // Try to load it from Resources folder
                instance_ = Resources.Load<BlockStyle>("BlockStyles");
                if (instance_ == null)
                {
                    Debug.Log("ERROR. Write correct path");
                }
            }
            return instance_;
        }
    }

    [System.Serializable]
    public class BlockStyleData
    {
        public BlockType blockType_;
        public ActionType actionType_;
        public Sprite sprite_;
        public Color color_;
    }

    public List<BlockStyleData> styles_;

    private Dictionary<(BlockType, ActionType), (Sprite, Color)> stylesDictionary_;

    public (Sprite, Color) GetStyle(BlockType block_type, ActionType action_type)
    {
        if (stylesDictionary_ == null)
        {
            stylesDictionary_ = new Dictionary<(BlockType, ActionType), (Sprite, Color)>();
            foreach (var style in styles_)
            {
                stylesDictionary_.Add((style.blockType_, style.actionType_), (style.sprite_, style.color_));
            }
        }

        if (stylesDictionary_.ContainsKey((block_type, action_type)))
        {
            var style = stylesDictionary_[(block_type, action_type)];
            return style;
        }
        else return stylesDictionary_[(BlockType.BlockType_None, ActionType.ActionType_None)];
    }
}
