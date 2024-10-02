
using UnityEngine;

public class AlertModeManager : MonoBehaviour
{
    static private AlertModeManager S;

    public delegate void AlertModeStatusChangeDelegate(bool alertMode);
    public static AlertModeStatusChangeDelegate alertModeStatusChangeDelegate;

    static private bool _ALERT_MODE = false;
    static public bool ALERT_MODE
    {
        get
        {
            return _ALERT_MODE;
        }
        private set
        {
            bool announce = (value != _ALERT_MODE);
            _ALERT_MODE = value;
            if (announce && alertModeStatusChangeDelegate != null)
            {
                alertModeStatusChangeDelegate(_ALERT_MODE);
            }
        }
    }

    [Tooltip("Check this to turn the Alert on/off manually (next Update).")]
    public bool testAlertTrigger = false;

    // Use this for initialization
    void Awake()
    {
        S = this;

#if DEBUG_AlertModeManager_ConsoleLogAlertModeChange

        alertModeStatusChangeDelegate += LogToConsole;
#endif


        ALERT_MODE = false;
    }


    void Update()
    {
        if (!ALERT_MODE && testAlertTrigger)
        {
            ALERT_MODE = true;
        }
        if (ALERT_MODE && !testAlertTrigger)
        {
            ALERT_MODE = false;
        }
    }

#if DEBUG_AlertModeManager_ConsoleLogAlertModeChange
    private void LogToConsole(bool tf)
    {
        Debug.Log("ALERT_MODE changed to " + tf);
    }
#endif

    /// <summary>
    /// This is the method to call to turn on Alert mode, and it should be used
    /// by the ACS-17 Enemy, the SecurityCamera, and the SecurityGate
    /// </summary>
    /// <param name="newAlertMode">If set to <c>true</c> new alert mode.</param>
    public static void SwitchToAlertMode(bool newAlertMode = true)
    {
        if (S == null)
        {
            Debug.LogError("AlertModeManager:SwitchToAlertMode() - Method called " +
                           "with no AlertModeManager in Scene!");
            return;
        }

        if (ALERT_MODE != newAlertMode)
        {
            S.testAlertTrigger = newAlertMode;
            ALERT_MODE = newAlertMode;
        }
    }


}