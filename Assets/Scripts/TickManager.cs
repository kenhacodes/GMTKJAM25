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

    private TickManager() { }

    public MovementList playerMoveSet_;
    public MovementList enemyMoveSet_;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
