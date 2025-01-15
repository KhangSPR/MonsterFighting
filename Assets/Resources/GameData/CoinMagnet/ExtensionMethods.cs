using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// It is common to create a class to contain all of your
// extension methods. This class must be static.
public static class ExtensionMethods
{
    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    // Returns the world space position that corresponds to the center of a RectTransform
    public static Vector3 GetWorldPosition(this RectTransform rectTransform, Camera camera = null, float zDepth = 10f)
    {
        if (camera == null)
            camera = Camera.main;

        Vector3 worldPos = Vector3.zero;

        if (camera == null || rectTransform == null)
            return worldPos;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, rectTransform.position);
        return screenPos.ScreenPosToWorldPos(camera, zDepth);
    }

    // Converts a screen position to world space
    public static Vector3 ScreenPosToWorldPos(this Vector2 screenPos, Camera camera = null, float zDepth = 10f)
    {
        if (camera == null)
            camera = Camera.main;

        if (camera == null)
            return Vector2.zero;

        float xPos = screenPos.x;
        float yPos = screenPos.y;
        Vector3 worldPos = Vector3.zero;

        if (!float.IsNaN(screenPos.x) && !float.IsNaN(screenPos.y))
        {
            // Flip y-coordinate; in UnityEngine.UI, (0,0) is bottom-left.
            yPos = camera.pixelHeight - yPos;

            // Convert to world space position using Camera class
            Vector3 screenCoord = new Vector3(xPos, yPos, zDepth);
            worldPos = camera.ScreenToWorldPoint(screenCoord);
        }
        return worldPos;
    }
}
