using UnityEngine;

public class ColorToggleInteraction : MonoBehaviour
{
    [Tooltip("Color to toggle to when interaction occurs")]
    public Color targetColor = Color.red;

    [Tooltip("Enable detailed logging for debugging purposes")]
    public bool enableDebugLogging = false;

    // Serialized for visibility in inspector, but private for encapsulation
    [SerializeField]
    private bool isUsingTargetColor = false;

    // Cached component references
    private Renderer objectRenderer;
    private Color originalColor;

    void Awake()
    {
        // Initialize renderer and original color in Awake for earlier setup
        InitializeRendererAndColor();
    }

    /// <summary>
    /// Initializes the renderer component and stores the original color
    /// </summary>
    private void InitializeRendererAndColor()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
            LogMessage($"Original color captured: {originalColor}", LogType.Log);
        }
        else
        {
            LogMessage("No Renderer component found. Color toggling will not work.", LogType.Warning);
        }
    }

    /// <summary>
    /// Handles mouse click interaction
    /// </summary>
    void OnMouseDown()
    {
        ToggleColor();
    }

    /// <summary>
    /// Toggles the object's color between original and target color
    /// </summary>
    private void ToggleColor()
    {
        // Early exit if no renderer is available
        if (objectRenderer == null)
        {
            LogMessage("Cannot toggle color - no renderer found.", LogType.Error);
            return;
        }

        // Determine and apply the new color
        Color newColor = isUsingTargetColor ? originalColor : targetColor;
        objectRenderer.material.color = newColor;

        // Log the color change
        LogMessage($"Color changed to: {newColor}", LogType.Log);

        // Update color state
        isUsingTargetColor = !isUsingTargetColor;
    }

    /// <summary>
    /// Centralized logging method with configurable output
    /// </summary>
    /// <param name="message">Message to log</param>
    /// <param name="logType">Type of log message</param>
    private void LogMessage(string message, LogType logType = LogType.Log)
    {
        if (!enableDebugLogging) return;

        switch (logType)
        {
            case LogType.Warning:
                Debug.LogWarning($"[ColorToggle] {message}");
                break;
            case LogType.Error:
                Debug.LogError($"[ColorToggle] {message}");
                break;
            default:
                Debug.Log($"[ColorToggle] {message}");
                break;
        }
    }

    /// <summary>
    /// Optional method to reset to original color programmatically
    /// </summary>
    public void ResetToOriginalColor()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
            isUsingTargetColor = false;
            LogMessage("Color reset to original.", LogType.Log);
        }
    }
}