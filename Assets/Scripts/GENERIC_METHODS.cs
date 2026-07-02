using UnityEditor;
using UnityEngine;

// Here contains a list of generic methods that may be called at multiple points in the code
public static class GENERIC_METHODS
{
    public static void QuitGame()
    {
        EditorApplication.isPlaying = false;
        //Application.Quit();
    }
}
