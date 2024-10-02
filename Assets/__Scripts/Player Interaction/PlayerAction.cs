using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerInteractable))]
public abstract class PlayerAction : PlayerActionAbstract
{
    
    [SerializeField]
    [Tooltip("Number of times that this Interactable can be activated. If set to" +
             "-1, there is no limit to the number of activations.")]
    private int _timesLeftToActivate = -1;
    public int timesLeftToActivate
    {
        get { return _timesLeftToActivate; }
        private set { _timesLeftToActivate = value; }
    }

    void Awake()
    {
        PlayerInteractable playInt = GetComponent<PlayerInteractable>();
        playInt.Register(this);
    }


    public override sealed void Act()
    {
        if (timesLeftToActivate > 0)
        {
            timesLeftToActivate--;
        }

        Action();
    }

    public abstract void Action();
}

