using UnityEngine;


// NOTE: This script was modified by Jeremy Gibson Bond for the CPP Stealth Demo
// NOTE: Where lines have been modified, they are marked with "// JGB"
namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        public bool useCameraOrientation = true;  // JGB

        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;                   // the world-relative desired move direction, calculated from the camForward and user input.
        private bool m_Jump = false;              // JGB - Jump is always disabled


        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            // JGB - Jump is always disabled
            //if (!m_Jump)
            //{
            //    m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            //}
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (!enabled) Debug.LogWarning("FixedUpdate is still called");
            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null && useCameraOrientation) // JGB
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
