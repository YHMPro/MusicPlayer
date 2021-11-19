using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Farme.Tool;
using System.IO;
namespace Farme
{
    /// <summary>
    /// AB包加载
    /// </summary>
    public class AssetBundleLoad
    {
        /// <summary>
        /// 主包信息  包含子包之间的配置信息与依赖关系
        /// </summary>
        private static AssetBundleManifest m_MainABInfo = null;
        /// <summary>
        /// 主包
        /// 每个工程在进行资源打包时只会存在一个主包,主包本身不存放资源文件，只包含子包之间的依赖或其他
        /// 同一个主包只能加载一次
        /// </summary>
        private static AssetBundle m_MainAB = null;
        /// <summary>
        /// 包容器
        /// </summary>
        private static Dictionary<string, AssetBundle> m_ABDic = new Dictionary<string, AssetBundle>();     
        /// <summary>
        /// 包目录文件夹地址
        /// </summary>
        private static string m_PackageCatalogueFile_URL = null;
        /// <summary>
        /// 主包名
        /// </summary>
        private static string m_MainABName = null;      
        /// <summary>
        /// 包目录文件夹地址
        /// </summary>
        public static string PackageCatalogueFile_URL
        {
            set
            {
                m_PackageCatalogueFile_URL = value;
            }
            get
            {
                return m_PackageCatalogueFile_URL;
            }
        }
        /// <summary>
        /// 主包名
        /// </summary>
        public static string MainABName
        {
            set
            {
                m_MainABName = value;
            }
            get
            {
                return m_MainABName;
            }
        }
        #region 异步初始化主包与包配置信息
        /// <summary>
        /// 初始化主包
        /// </summary>
        /// <param name="reslutCallback">结果回调</param>
        private static void InitMainABAsync(UnityAction reslutCallback=null)
        {
            if (m_MainAB != null)
            {
                reslutCallback?.Invoke();
                return;
            }
            if (!File.Exists(m_PackageCatalogueFile_URL + m_MainABName))//判定主包文件是否存在
            {
                Debuger.LogError("主包文件路径错误!!!");
                return;
            }          
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEInitMainAB(reslutCallback));
        }
        private static IEnumerator IEInitMainAB(UnityAction reslutCallback=null)
        {          
                AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(m_PackageCatalogueFile_URL + m_MainABName);
                while(true)
                {
                    if(abcr.isDone)
                    {
                        Debuger.Log("主包加载成功");
                        break;
                    }                   
                    yield return abcr.progress;
                }
                m_MainAB = abcr.assetBundle;
                AssetBundleRequest abr = m_MainAB.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
                while(true)
                {
                    if(abr.isDone)
                    {
                        Debuger.Log("包依赖信息获取成功");
                        break;
                    }
                    yield return abr.progress;
                }
                m_MainABInfo = abr.asset as AssetBundleManifest;
                reslutCallback?.Invoke();         
        }
        #endregion
        #region 异步加载资源
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <param name="resultCallback">结果回调</param>
        public static void LoadAssetAsync<T>(string abName, string resName, UnityAction<T> resultCallback) where T : Object
        {
            InitMainABAsync(() => {
                InitAssetBundleDependenciesAsync(abName, (ab) => {
                    if(ab==null)
                    {
                        resultCallback?.Invoke(null);
                        return;
                    }
                    MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IELoadAsset(ab, resName, resultCallback));
                });            
            });        
        }
        private static IEnumerator IELoadAsset<T>(AssetBundle ab, string resName, UnityAction<T> resultCallback) where T : Object
        {
            AssetBundleRequest abr = ab.LoadAssetAsync<T>(resName);
            while (true)
            {
                if (abr.isDone)
                {
                    break;
                }
                yield return abr.progress;
            }
            T t = abr.asset as T;
            if (t==null)
            {
                Debuger.LogError("资源:" + resName + "不存在");
            }
            resultCallback?.Invoke(abr.asset as T);         
        }
        /// <summary>
        /// 初始化包的依赖包并加载该包
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="reslutCallback">结果回调</param>
        private static void InitAssetBundleDependenciesAsync(string abName, UnityAction<AssetBundle> reslutCallback)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEInitAssetBundleDependencies(abName,reslutCallback));
        }
        private static IEnumerator IEInitAssetBundleDependencies(string abName,UnityAction<AssetBundle> reslutCallback)
        {
            AssetBundle ab;
            AssetBundleCreateRequest abcr;
            //获取存在依赖关系的包   
            string[] dependenciesABundle = m_MainABInfo.GetAllDependencies(abName);
            //逐个遍历所有与自身存在依赖关系的包
            for (int Index = 0; Index < dependenciesABundle.Length; Index++)
            {
                //判断依赖包是否已经加载过
                if (!m_ABDic.ContainsKey(dependenciesABundle[Index]))
                {
                    if(!File.Exists(m_PackageCatalogueFile_URL + dependenciesABundle[Index]))//判定包文件是否存在
                    {
                        Debuger.LogError(dependenciesABundle[Index] + "包文件不存在");
                        break;
                    }
                    abcr = AssetBundle.LoadFromFileAsync(m_PackageCatalogueFile_URL + dependenciesABundle[Index]);
                    while (true)
                    {
                        if (abcr.isDone)
                        {
                            break;
                        }
                        yield return abcr.progress;
                    }
                    ab = abcr.assetBundle;
                    //添加到包容器当中
                    m_ABDic.Add(dependenciesABundle[Index], ab);
                }
            }
            if(m_ABDic.TryGetValue(abName,out ab))
            {
                reslutCallback?.Invoke(ab);
            }
            else
            {
                if (!File.Exists(m_PackageCatalogueFile_URL + abName))//判定包文件是否存在
                {
                    Debuger.LogError(abName + "包文件不存在");
                    reslutCallback?.Invoke(null);
                }
                else
                {
                    abcr = AssetBundle.LoadFromFileAsync(m_PackageCatalogueFile_URL + abName);
                    while (true)
                    {
                        if (abcr.isDone)
                        {
                            break;
                        }
                        yield return abcr.progress;
                    }
                    ab = abcr.assetBundle;
                    //将子包添加到包容器当中
                    m_ABDic.Add(abName, ab);
                    reslutCallback?.Invoke(ab);
                }             
            }                 
        }
        #endregion
        #region 初始化主包与包配置信息
        /// <summary>
        /// 初始化主包
        /// </summary>
        /// <returns>是否成功</returns>
        private static bool InitMainAB()
        {
            if (m_MainAB != null)
            {
                return true;
            }
            if (!File.Exists(m_PackageCatalogueFile_URL + m_MainABName))
            {
                Debuger.LogError("主包文件路径错误!!!");
                return false;
            }
            m_MainAB = AssetBundle.LoadFromFile(m_PackageCatalogueFile_URL + m_MainABName);//加载主包
            m_MainABInfo = m_MainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");//加载主包配置信息
            return true;
        }
        #endregion
        #region 同步加载       
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <returns>资源</returns>
        public static T LoadAsset<T>(string abName, string resName) where T : Object
        {
            if (InitMainAB())//初始化AB包
            {
                AssetBundle ab = InitAssetBundleDependencies(abName);
                if(ab==null)
                {
                    return null;
                }
                T t = ab.LoadAsset<T>(resName);
                if(t==null)
                {
                    Debuger.LogError("资源:" + resName + "不存在");
                }
                return t;
            }
            return null;
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <param name="result">结果</param>
        /// <returns>是否加载成功</returns>
        public static bool LoadAsset<T>(string abName, string resName,out T result) where T : Object
        {
            result = LoadAsset<T>(abName, resName);
            return result == null ? false : true;
        }
        /// <summary>
        /// 初始化AB包的依赖关系并加载包
        /// </summary>
        /// <param name="abName">包名</param>
        /// <returns>AB包</returns>
        private static AssetBundle InitAssetBundleDependencies(string abName)
        {
            AssetBundle ab;
            //获取存在依赖关系的包   
            string[] dependenciesABundle = m_MainABInfo.GetAllDependencies(abName);
            //逐个遍历所有与自身存在依赖关系的包
            for (int Index = 0; Index < dependenciesABundle.Length; Index++)
            {
                //判断依赖包是否已经加载过
                if (!m_ABDic.ContainsKey(dependenciesABundle[Index]))
                {
                    if (!File.Exists(m_PackageCatalogueFile_URL + dependenciesABundle[Index]))//判定包文件是否存在
                    {
                        Debuger.LogError(dependenciesABundle[Index] + "包文件不存在");
                        break;
                    }
                    //加载包
                    ab = AssetBundle.LoadFromFile(m_PackageCatalogueFile_URL + dependenciesABundle[Index]);
                    //添加到包容器当中
                    m_ABDic.Add(dependenciesABundle[Index], ab);
                }
            }
            if(m_ABDic.TryGetValue(abName,out ab))
            {
                return ab;
            }
            else
            {
                if (!File.Exists(m_PackageCatalogueFile_URL + abName))//判定包文件是否存在
                {
                    Debuger.LogError(abName + "包文件不存在");
                    return null;
                }
                else
                {
                    ab = AssetBundle.LoadFromFile(m_PackageCatalogueFile_URL + abName);                    
                    //将子包添加到包容器当中
                    m_ABDic.Add(abName, ab);
                    return ab;
                }
            }
        }
        #endregion
        #region 卸载资源
        /// <summary>
        /// 卸载主包
        /// </summary>
        /// <param name="unloadAllLoadedObjects">是否卸载所有场景内已加载的资源对象</param>
        public static void UnLoadMainAB(bool unloadAllLoadedObjects = false)
        {
            if(m_MainAB==null)
            {
                Debuger.LogError("主包为NULL。");
                return;
            }
            m_MainAB.Unload(unloadAllLoadedObjects);
            m_MainAB = null;
            m_MainABInfo = null;
            m_MainABName = null;
        }
        /// <summary>
        /// 卸载包(主包的卸载已提供专门的函数(UnLoadMainAB)进行处理)
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="unloadAllLoadedObjects">是否卸载所有场景内已加载的资源对象</param>
        public static void UnLoadAB(string abName,bool unloadAllLoadedObjects=false)
        {
            if(m_ABDic.TryGetValue(abName,out AssetBundle ab))
            {
                ab.Unload(unloadAllLoadedObjects);
                m_ABDic.Remove(abName);
                return;
            }
            Debuger.LogWarning("卸载的      " + abName + "      包不存在。");
        }
        #endregion
    }
}
