using UnityEngine;

public class ShowStatusBar : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating(nameof(Apply), 0f, 0.5f);
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
                // Убираем FLAG_FULLSCREEN (0x400) и ставим FLAG_FORCE_NOT_FULLSCREEN (0x800)
                window.Call("clearFlags", 0x00000400);
                window.Call("addFlags", 0x00000800);

                // Разрешаем рисовать под системными панелями
                window.Call("addFlags", unchecked((int)0x80000000)); // FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS
                window.Call("addFlags", 0x00000200);                 // FLAG_LAYOUT_NO_LIMITS

                // Прозрачные цвета
                using (var colorClass = new AndroidJavaClass("android.graphics.Color"))
                using (var transparent = colorClass.CallStatic<AndroidJavaObject>("argb", 0, 0, 0, 0))
                {
                    window.Call("setStatusBarColor", transparent);
                    window.Call("setNavigationBarColor", transparent);
                }

                // Флаги decorView
                using (var decorView = window.Call<AndroidJavaObject>("getDecorView"))
                {
                    int flags = decorView.Call<int>("getSystemUiVisibility");
                    flags |= 0x00000100; // LAYOUT_STABLE
                    flags |= 0x00000200; // LAYOUT_HIDE_NAVIGATION
                    flags |= 0x00000400; // LAYOUT_FULLSCREEN
                    flags |= 0x00000010; // LIGHT_NAVIGATION_BAR (тёмные иконки нижней панели)
                    flags |= 0x00002000; // LIGHT_STATUS_BAR (тёмные иконки верхней панели)
                    decorView.Call("setSystemUiVisibility", flags);
                }

                // Отключаем контрастные подложки (Android 10+)
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
            Debug.LogWarning("ShowStatusBar error: " + e.Message);
        }
#endif
    }
}