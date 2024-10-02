using UnityEngine;

public class LookAtOnDrawGizmos : MonoBehaviour
{
    [Header("Inscribed")]
    [Tooltip("The Transform this should look at")]
    public Transform lookAt;

    private void OnDrawGizmos()
    {
        if (lookAt != null)
        {
            transform.LookAt(lookAt);
        }
    }
}
