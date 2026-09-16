using UnityEngine;

public class TransparentNavigationBar : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating(nameof(Apply), 0f, 0.5f);
        Application.targetFrameRate = 60;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) Apply();
    }

    private void Apply()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var window = activity.Call<AndroidJavaObject>("getWindow"))
            {
                window.Call("addFlags", unchecked((int)0x80000000));
                using (var colorClass = new AndroidJavaClass("android.graphics.Color"))
                using (var transparent = colorClass.CallStatic<AndroidJavaObject>("argb", 0, 0, 0, 0))
                {
                    window.Call("setNavigationBarColor", transparent);
                    window.Call("setStatusBarColor", transparent);
                }

                int sdk = new AndroidJavaClass("android.os.Build$VERSION").GetStatic<int>("SDK_INT");
                if (sdk >= 29)
                {
                    window.Call("setNavigationBarContrastEnforced", false);
                    window.Call("setStatusBarContrastEnforced", false);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("NavBar error: " + e.Message);
        }
#endif
    }
}