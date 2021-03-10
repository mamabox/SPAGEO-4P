using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class ScreenshotManager : MonoBehaviour
{
    private GameManager gameManager;
    private RouteManager routeManager;
    private Canvas canvas;
    private string savePath;


    private string[] routeDir2 = { "1_1N","1_2N","1_2E","2_2E","2_2N","2_3N" };
    private string[] currentRouteDir;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        routeManager = GetComponent<RouteManager>();
        canvas = FindObjectOfType<Canvas>();

        savePath = Path.Combine(gameManager.screenshotPath, "/route/");
        currentRouteDir = routeDir2;
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

        //canvas.enabled = false;
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(gameManager.screenshotPath, screenshotName));
        //canvas.enabled = true;
        Debug.Log(gameManager.screenshotPath + screenshotName);
    }

    [ContextMenu("Screenshot All")]
    public void screenshotAllIntersections()
    {
        // 1. for each element in validCood - for each element in validDir - call goto and take screenshot
    }


    //[ContextMenu ("Screenshot Route")]
    //public void ScreenschotCurrentRoute()
    //{
    //    List<string> _route = currentRouteDir.ToList();


    //    if (!System.IO.Directory.Exists(savePath))    //if save directory does not exist, create it
    //    {
    //        System.IO.Directory.CreateDirectory(savePath);
    //    }

    //    for (int i=1; i < _route.Count(); i++)
    //    {

    //    }
    //}

    //public void ScreenshotCoor(string coord, string dir)
    //{
    //    var screenshotName = gameManager.inputCoordX + routeManager.coordSeparator + gameManager.inputCoordY + gameManager.inputDir + ".png";
    //    ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(savePath, screenshotName));
    //}
}