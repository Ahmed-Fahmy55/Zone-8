using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Renderer) )]
public class AlertModeColorModifier : MonoBehaviour {
	[Header("Set in Inspector")]
	public int materialElementNumber = 0;
    public string colorName = "_EmissionColor";
    public Color colorToSwitchTo = Color.red;

    [Header("Set Dynamically")]
	public Material material;
	public Color originalColor;

	private bool inited = false;

	// Use this for initialization
	void Start () {
        // Find the Material by index
		material = null;
		Material[] mats = GetComponent<Renderer>().materials;
		if (materialElementNumber < mats.Length) {
			material = mats[materialElementNumber];
		}

		if (material == null) {
			Debug.LogError("GameObject "+gameObject.name+" does not have a material element #: "
                           +materialElementNumber);
            return;
		} else if (!material.HasProperty(colorName)) {
			Debug.LogError("Material "+material.name+" does not have color named: "+colorName);
			return;
		}

        originalColor = material.GetColor(colorName);

        AlertModeManager.alertModeStatusChangeDelegate += AlertModeStatusChange;

		inited = true;
	}

    private void OnDestroy()
    {
        AlertModeManager.alertModeStatusChangeDelegate -= AlertModeStatusChange;
    }
	
    void AlertModeStatusChange(bool alertMode) {
        if (!inited) {
            return;
        }
        material.SetColor(colorName, (alertMode) ? colorToSwitchTo : originalColor );
    }
	
}
