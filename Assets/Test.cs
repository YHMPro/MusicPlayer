using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = "F:\\MyMusic\\";
        if (Directory.Exists(path))
        {
            string[] strArray=  Directory.GetFiles(path,"*.mp3");
            foreach (var str in strArray)
            {
               
               
            }
        }
        //string []str = DirectoryInfo.
        // File.Op("C:\\Users\\XiaoHeTao\\Desktop\\新建文件夹\\");


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
