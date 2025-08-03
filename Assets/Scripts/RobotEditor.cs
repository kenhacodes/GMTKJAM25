using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCustomizer : MonoBehaviour
{
    [Header("Slots")]
        public Transform headSlot;
        public Transform trunkSlot;
        public Transform armsSlot;
        public Transform legsSlot;
        public Transform keySlot;

        [Header("Parts")]
        public GameObject[] headOptions;
        public GameObject[] trunkOptions;
        public GameObject[] armOptions;
        public GameObject[] legOptions;
        public GameObject[] keyOptions;

        private GameObject currentHead;
        private GameObject currentTrunk;
        private GameObject currentArms;
        private GameObject currentLegs;
        private GameObject currentKey;

        private int headIndex = 0, trunkIndex = 0, armsIndex = 0, legsIndex = 0, keyIndex = 0;

        public enum PartType { Head, Trunk, Arms, Legs, Key }

        void Start()
        {
            UpdateAllParts();
        }

        public void PreviousHead()
        {
            headIndex = (headIndex - 1 + headOptions.Length) % headOptions.Length;
            ReplacePart(ref currentHead, headOptions[headIndex], headSlot);
        }

        public void PreviousTrunk()
        {
            trunkIndex = (trunkIndex - 1 + trunkOptions.Length) % trunkOptions.Length;
            ReplacePart(ref currentTrunk, trunkOptions[trunkIndex], trunkSlot);
        }

        public void PreviousArms()
        {
            armsIndex = (armsIndex - 1 + armOptions.Length) % armOptions.Length;
            ReplacePart(ref currentArms, armOptions[armsIndex], armsSlot);
        }

        public void PreviousLegs()
        {
            legsIndex = (legsIndex - 1 + legOptions.Length) % legOptions.Length;
            ReplacePart(ref currentLegs, legOptions[legsIndex], legsSlot);
        }

        public void PreviousKey()
        {
            keyIndex = (keyIndex - 1 + keyOptions.Length) % keyOptions.Length;
            ReplacePart(ref currentKey, keyOptions[keyIndex], keySlot);
        }

        public void NextHead()
        {
            headIndex = (headIndex + 1) % headOptions.Length;
            ReplacePart(ref currentHead, headOptions[headIndex], headSlot);
        }

        public void NextTrunk()
        {
            trunkIndex = (trunkIndex + 1) % trunkOptions.Length;
            ReplacePart(ref currentTrunk, trunkOptions[trunkIndex], trunkSlot);
        }

        public void NextArms()
        {
            armsIndex = (armsIndex + 1) % armOptions.Length;
            ReplacePart(ref currentArms, armOptions[armsIndex], armsSlot);
        }

        public void NextLegs()
        {
            legsIndex = (legsIndex + 1) % legOptions.Length;
            ReplacePart(ref currentLegs, legOptions[legsIndex], legsSlot);
        }

        public void NextKey()
        {
            keyIndex = (keyIndex + 1) % keyOptions.Length;
            ReplacePart(ref currentKey, keyOptions[keyIndex], keySlot);
        }

        private void ReplacePart(ref GameObject currentPart, GameObject newPartPrefab, Transform parent)
        {
            if (currentPart != null)
                Destroy(currentPart);

            currentPart = Instantiate(newPartPrefab, parent);
            currentPart.transform.localPosition = Vector3.zero;
            currentPart.transform.localRotation = Quaternion.identity;
        }

        private void UpdateAllParts()
        {
            NextHead();
            NextTrunk();
            NextArms();
            NextLegs();
            NextKey();
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
