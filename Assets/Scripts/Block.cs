using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;



[CreateAssetMenu(fileName = "New Block", menuName = "Block/Type")]
public class Block : ScriptableObject
{
    public BlockType blockType_;
    public ActionType actionType_;
    public int width_;
    public int height_;
    public string text_;

    public float executionTime_;
}
