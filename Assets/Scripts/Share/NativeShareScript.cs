using UnityEngine;
using System.IO;
using System.Collections;
using EasyMobile;

public class NativeShareScript : MonoBehaviour
{
    public GameObject CanvasShareObj;
    private bool isProcessing = false;
    private bool isFocus = false;

    public void ShareBtnPress()
    {
        if (!isProcessing)
        {
            CanvasShareObj.SetActive(true);
            StartCoroutine(ShareScreenshot());
        }
    }

    //Precisamos esperar o render ter finalizado sua transição, por isso usei Coroutine, para garantir que isso tenha acontecido;
    IEnumerator ShareScreenshot()
    {
        isProcessing = true;

        //Aqui garantimos o final do Render
        yield return new WaitForEndOfFrame();


        // The SaveScreenshot() method returns the path of the saved image
        // The provided file name will be added a ".png" extension automatically
        string path = Sharing.SaveScreenshot("screenshot");

        //ScreenCapture.CaptureScreenshot("screenshot.png", 2);
        //string destination = Path.Combine(Application.persistentDataPath, "screenshot.png");


        // Share a text
        Sharing.ShareText("Hello from Easy Mobile!");

        // Share a URL
        Sharing.ShareURL("www.sglibgames.com");

        yield return new WaitForSecondsRealtime(0.3f);


        /*
        if (!Application.isEditor)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.CallStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Olha esse jogo que incrível!");
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share to earn coins!");
            currentActivity.Call("startActivity", chooser);

            yield return new WaitForSecondsRealtime(1f);

        }
        */


        yield return new WaitUntil(() => isFocus);
        CanvasShareObj.SetActive(false);
        isProcessing = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }

}
