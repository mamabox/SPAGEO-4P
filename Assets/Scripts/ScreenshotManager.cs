using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{
    private GameManager gameManager;
    private RouteManager routeManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        routeManager = GetComponent<RouteManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeScreenshot()
    {

        if (!System.IO.Directory.Exists(gameManager.screenshotPath))    //if save directory does not exist, create it
        {
            System.IO.Directory.CreateDirectory(gameManager.screenshotPath);
        }

        //var screenshotName = "Screenshot_" + System.DateTime.Now.ToString("HH-mm-ss") + ".png";
        var screenshotName = gameManager.inputCoordX + routeManager.coordSeparator + gameManager.inputCoordY + gameManager.inputDir + ".png";

        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(gameManager.screenshotPath, screenshotName));
        Debug.Log(gameManager.screenshotPath + screenshotName);
    }

    [ContextMenu ("Screenshot All")]
    public void screenshotAllIntersections()
    {
        // 1. for each element in validCood - for each element in validDir - call goto and take screenshot
    }
}
