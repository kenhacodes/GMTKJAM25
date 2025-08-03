using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCustomizer : MonoBehaviour
{
    [Header("Slots (Padres opcionales, no usados en esta versión)")]
    public Transform headSlot;
    public Transform trunkSlot;
    public Transform armsSlot;
    public Transform legsSlot;
    public Transform keySlot;

    [Header("Parts (Todos los modelos ya deben estar en la escena como hijos del robot y desactivados)")]
    public GameObject[] headOptions;
    public GameObject[] trunkOptions;
    public GameObject[] armOptions;
    public GameObject[] legOptions;
    public GameObject[] keyOptions;

    private int headIndex = 0, trunkIndex = 0, armsIndex = 0, legsIndex = 0, keyIndex = 0;

    public enum PartType { Head, Trunk, Arms, Legs, Key }

    void Start()
    {
        UpdateAllParts();
    }

    public void PreviousHead()
    {
        headIndex = (headIndex - 1 + headOptions.Length) % headOptions.Length;
        ActivatePart(headOptions, headIndex);
    }

    public void PreviousTrunk()
    {
        trunkIndex = (trunkIndex - 1 + trunkOptions.Length) % trunkOptions.Length;
        ActivatePart(trunkOptions, trunkIndex);
    }

    public void PreviousArms()
    {
        armsIndex = (armsIndex - 1 + armOptions.Length) % armOptions.Length;
        ActivatePart(armOptions, armsIndex);
    }

    public void PreviousLegs()
    {
        legsIndex = (legsIndex - 1 + legOptions.Length) % legOptions.Length;
        ActivatePart(legOptions, legsIndex);
    }

    public void PreviousKey()
    {
        keyIndex = (keyIndex - 1 + keyOptions.Length) % keyOptions.Length;
        ActivatePart(keyOptions, keyIndex);
    }

    public void NextHead()
    {
        headIndex = (headIndex + 1) % headOptions.Length;
        ActivatePart(headOptions, headIndex);
    }

    public void NextTrunk()
    {
        trunkIndex = (trunkIndex + 1) % trunkOptions.Length;
        ActivatePart(trunkOptions, trunkIndex);
    }

    public void NextArms()
    {
        armsIndex = (armsIndex + 1) % armOptions.Length;
        ActivatePart(armOptions, armsIndex);
    }

    public void NextLegs()
    {
        legsIndex = (legsIndex + 1) % legOptions.Length;
        ActivatePart(legOptions, legsIndex);
    }

    public void NextKey()
    {
        keyIndex = (keyIndex + 1) % keyOptions.Length;
        ActivatePart(keyOptions, keyIndex);
    }

    private void ActivatePart(GameObject[] options, int activeIndex)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i] != null)
                options[i].SetActive(i == activeIndex);
        }
    }

    private void UpdateAllParts()
    {
        ActivatePart(headOptions, headIndex);
        ActivatePart(trunkOptions, trunkIndex);
        ActivatePart(armOptions, armsIndex);
        ActivatePart(legOptions, legsIndex);
        ActivatePart(keyOptions, keyIndex);
    }

    public string GetCurrentPartName(PartType part)
    {
        switch (part)
        {
            case PartType.Head:
                return headOptions[headIndex].name;
            case PartType.Trunk:
                return trunkOptions[trunkIndex].name;
            case PartType.Arms:
                return armOptions[armsIndex].name;
            case PartType.Legs:
                return legOptions[legsIndex].name;
            case PartType.Key:
                return keyOptions[keyIndex].name;
            default:
                return "";
        }
    }
}