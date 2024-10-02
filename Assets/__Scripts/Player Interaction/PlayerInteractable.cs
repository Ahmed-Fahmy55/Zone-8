using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInteractable : MonoBehaviour {
    [Tooltip("If set to None, this will trigger when the player enters the trigger.")]
    public KeyCode      triggeringKey = KeyCode.None;

    List<PlayerAction>  playerActions = new List<PlayerAction>();

    [SerializeField]
    protected bool _playerWithinTrigger;
    public bool playerWithinTrigger
    {
        get
        {
            return (playerActions.Count > 0) ? _playerWithinTrigger : false;
        }
        protected set
        {
            _playerWithinTrigger = value;
        }
    }


    protected virtual void Awake()
    {
        Collider coll = GetComponent<Collider>();
        if (coll != null && !coll.isTrigger) {
            Debug.LogWarning("PlayerInteractable:Awake – If there is a Collider on " +
                             "this GameObject, it must be set to isTrigger! " +
                             "Setting to isTrigger now.");
            coll.isTrigger = true;
        }
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractingPlayer>() != null)
        {
            playerWithinTrigger = true;
        }
    }


    protected void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractingPlayer>() != null)
        {
            playerWithinTrigger = false;
        }
    }


    protected virtual void Update()
    {
        if (playerWithinTrigger)
        {
            if (triggeringKey == KeyCode.None)
            {
                ExecuteActions();
            }
            else if (Input.GetKeyDown(triggeringKey))
            {
                // Execute actions!
                ExecuteActions();
            }
        }
    }


    protected void ExecuteActions()
    {

        PlayerAction ipa;
        for (int i=playerActions.Count-1; i>=0; i--)
        {
            ipa = playerActions[i];
            ipa.Act();
            if (ipa.timesLeftToActivate == 0)
            {
                playerActions.Remove(ipa);
            }
        }
    }


    public void Register(PlayerAction ipa) 
    {
        if (!playerActions.Contains(ipa))
        {
            playerActions.Add(ipa);
        }
    }

}
