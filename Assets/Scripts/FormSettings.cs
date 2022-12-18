using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowSettings;
using WindowSettings.Linux;
using WindowSettings.MacOS;
using WindowSettings.Windows;

public class FormSettings : MonoBehaviour {

    public WindowsSettings Windows;
    public MacOSSettings MacOS;
    public LinuxSettings Linux;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    void Awake() {
#if UNITY_STANDALONE_WIN
        Windows.Set();
#endif
#if UNITY_STANDALONE_OSX
        MacOS.Set();
#endif
#if UNITY_STANDALONE_LINUX
        Linux.Set();
#endif
    }

}

namespace WindowSettings {

    namespace MacOS {
        [System.Serializable]
        public class MacOSSettings : IWindowSetting {
            public void Set() { }


        }
    }

    namespace Windows {
        using System;
        using System.Runtime.InteropServices;
        [System.Serializable]
        public class WindowsSettings : IWindowSetting {
#if UNITY_STANDALONE_WIN

            [DllImport("user32.dll")]
            public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

            [DllImport("user32.dll")]
            public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
            static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

            static int GWL_STYLE = -16;
            static int WS_MINIMIZEBOX = 0x00020000;
#endif

            public void Set() {
#if UNITY_STANDALONE_WIN
                IntPtr host = FindWindowByCaption(IntPtr.Zero, Application.productName);
                SetWindowLong(host, GWL_STYLE, GetWindowLong(host, GWL_STYLE) & ~WS_MINIMIZEBOX);
#endif
            }

        }
    }

    namespace Linux {
        [System.Serializable]
        public class LinuxSettings : IWindowSetting {
            public void Set() { }
        }
    }

    public interface IWindowSetting {
        public void Set();
    }
}