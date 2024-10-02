using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingPlayer : MonoBehaviour {
    static private InteractingPlayer _S;
    
	// Use this for initialization
	void Awake () {
        S = this;
	}


    static public InteractingPlayer S
    {
        get { return _S; }
        set
        {
            if (_S != null)
            {
                Debug.LogError("InteractingPlayer:S - Attempt to set Singleton when it has already been set.\n"+
                    "Old Singleton: " + _S.gameObject.name + "\tNew Singleton: " + value.gameObject.name);
            }
            _S = value;
        }
    }


    static public void SetPosition(Vector3 pos, Quaternion rot) {
        S.transform.position = pos;
        S.transform.rotation = rot;
    }
}
