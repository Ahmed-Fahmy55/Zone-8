using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(EnemyNav) )]
public class AlertModeEnemyStateModifier : MonoBehaviour {
	private EnemyNav        enemy;
    
    // Use this for initialization
    void Start () {
		enemy = GetComponent<EnemyNav>();
        AlertModeManager.alertModeStatusChangeDelegate += AlertModeStatusChange;
    }

    private void OnDestroy()
    {
        AlertModeManager.alertModeStatusChangeDelegate -= AlertModeStatusChange;
    }

    void AlertModeStatusChange(bool alertMode) {
		enemy.mode = alertMode ? EnemyNav.eMode.chase : EnemyNav.eMode.stopChase;
    }
}
