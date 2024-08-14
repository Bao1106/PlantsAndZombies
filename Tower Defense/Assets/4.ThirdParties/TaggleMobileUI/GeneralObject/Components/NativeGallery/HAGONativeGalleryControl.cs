using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NativeGallery;

public class HAGONativeGalleryControl
{
    private static HAGONativeGalleryControl m_api;
    public static HAGONativeGalleryControl Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = new HAGONativeGalleryControl();
            }
            return m_api;
        }
    }

    public void CheckPermission(Action callback)
    {
        NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read);
        Debug.Log("NativeGalleryControl - Check camera permission result: " + permission.ToString());
        switch (permission)
        {
            case NativeGallery.Permission.Denied:
            case NativeGallery.Permission.ShouldAsk:
                NativeGallery.Permission result =  NativeGallery.RequestPermission(NativeGallery.PermissionType.Read);
                if (result == NativeGallery.Permission.Granted)
                {
                    Debug.Log("NativeGalleryControl - RequestPermission: " + result);
                    callback?.Invoke();
                }
                break;
            case NativeGallery.Permission.Granted:
                callback?.Invoke();
                break;
            default:
                break;
        }
    }

    public void PickImage(Action<Texture2D> callback)
    {
        CoroutineHelper.Call(IERunPickImage((texture, path) => callback?.Invoke(texture)));
    }

    public void PickImage(Action<Texture2D, string> callback)
    {
        CoroutineHelper.Call(IERunPickImage(callback));
    }

    private IEnumerator IERunPickImage(Action<Texture2D, string> callback)
    {
        yield return new WaitForSeconds(0.3f);

        int maxSize = 512;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false, false,false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                }
                callback?.Invoke(texture, path);
            }
        }, "Select a jpg image", "image/jpg");

        if (permission == NativeGallery.Permission.Denied)
        {
            Debug.Log("Permission result: " + permission);
            callback?.Invoke(null, string.Empty);
        }
    }

    public IEnumerator TakeScreenshotAndSave(Action<Texture, string> callback)
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();
        
        // Save the screenshot to Gallery/Photos
        string path = $"My img { DateTime.Now.ToString() }.png";
        //
        Permission permissionResult = NativeGallery.SaveImageToGallery(ss, "Gallery_HAP", path);
        
        Debug.Log("Permission result: " + permissionResult);

        if(permissionResult == Permission.Granted)
        {
            callback?.Invoke(ss, path);
        }
        else
        {
            callback?.Invoke(ss, null);
        }
    }

    public void PickVideo(Action callback)
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            Debug.Log("Video path: " + path);
            if (path != null)
            {
                // Play the selected video
                Handheld.PlayFullScreenMovie("file://" + path);
            }

            callback?.Invoke();
        }, "Select a video");

        Debug.Log("Permission result: " + permission);
        callback?.Invoke();
    }
}