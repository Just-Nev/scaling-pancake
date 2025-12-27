using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RendererToggle : MonoBehaviour
{
    [Header("Camera to change (leave empty = Camera.main)")]
    [SerializeField] private Camera targetCamera;

    [Header("URP Renderer indices (from your URP Pipeline Asset)")]
    [SerializeField] private int rendererWhenOff = 0;
    [SerializeField] private int rendererWhenOn = 1;

    private UniversalAdditionalCameraData camData;

    private void Awake()
    {
        if (targetCamera == null) targetCamera = Camera.main;

        if (targetCamera == null)
        {
            Debug.LogError("RendererToggle: No camera found. Assign Target Camera.");
            enabled = false;
            return;
        }

        camData = targetCamera.GetUniversalAdditionalCameraData();
        if (camData == null)
        {
            Debug.LogError("RendererToggle: Could not get UniversalAdditionalCameraData (is this a URP camera?).");
            enabled = false;
        }
    }

    // Hook this to Toggle -> On Value Changed (bool)
    public void SetRenderer(bool isOn)
    {
        if (camData == null) return;

        int index = isOn ? rendererWhenOn : rendererWhenOff;
        camData.SetRenderer(index);
    }

    // Optional: if you want to switch by index from a dropdown/button
    public void SetRendererIndex(int index)
    {
        if (camData == null) return;
        camData.SetRenderer(index);
    }
}




