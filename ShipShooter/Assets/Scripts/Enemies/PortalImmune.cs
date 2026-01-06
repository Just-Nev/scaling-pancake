using UnityEngine;

public class PortalImmune : MonoBehaviour
{
    [Tooltip("If true, object will not be destroyed when it reaches the portal center.")]
    public bool doNotDestroy = true;

    [Tooltip("If true, once this object reaches the center, the portal becomes 'occupied' and stops pulling others.")]
    public bool occupyPortal = true;
}

