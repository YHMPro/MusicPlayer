using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public float ScaleCircle;
    public float MiddleCircleRadius;
    public float MiddleCircleWide;
    public float Scale;
    public float Wide;
    public float Radius;
    [SerializeField]
    private Vector3 m_Origin = Vector3.zero;
    private Vector3[] m_CirclePos = new Vector3[512];
    [SerializeField]
    private LineRenderer m_OutLR;
    [SerializeField]
    private LineRenderer m_InLR;
    [SerializeField]
    private LineRenderer m_MiddleLR;
    [SerializeField]
    private LineRenderer m_ScaleLR;
    private AudioSource m_AS;
    private void Awake()
    {
        m_AS=GetComponent<AudioSource>();   
    }
    // Start is called before the first frame update
    void Start()
    {
        m_InLR.positionCount = 513;
        m_OutLR.positionCount = 513;
        m_MiddleLR.positionCount = 513;
        m_ScaleLR.positionCount = 513;
        StartCoroutine(VisualAudio());
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    IEnumerator VisualAudio()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
        while (true)
        {
            #region 获取音频采集样本的信息
            float[] sampls = new float[512];
            m_AS.GetSpectrumData(sampls, 0, FFTWindow.Triangle);
            //Sort(sampls);//进行排序      
            #endregion
            #region 对m_CirclePos进行一点的放缩计算      
            BuilderCircle(sampls);
            #endregion
            //m_LR.startWidth = Wide;
            //m_LR.endWidth = Wide;
            m_OutLR.startWidth = Wide;
            m_OutLR.endWidth = Wide;
            m_InLR.startWidth = Wide;
            m_InLR.endWidth = Wide;
            m_MiddleLR.startWidth = Wide;
            m_MiddleLR.endWidth = Wide;
            m_ScaleLR.startWidth = Wide;
            m_ScaleLR.endWidth = Wide;
            
            //for(int index=0;index< m_CirclePos)
            //m_LR.SetPositions(m_CirclePos);
            //m_LR.SetPosition(m_CirclePos.Length, m_CirclePos[0]);
            yield return waitForSeconds;
        }
    }


    private void BuilderCircle(float[] smmplResult)
    {
        Vector3 vPos = Vector3.zero;
        for (float index = 0, angle = 0; index < smmplResult.Length; index++)
        {
            vPos.x = Mathf.Cos(angle * Mathf.Deg2Rad);// * (Radius+ smmplResult[(int)index] * Scale);
            vPos.y = Mathf.Sin(angle * Mathf.Deg2Rad);// * (Radius+ smmplResult[(int)index] * Scale);     
            m_OutLR.SetPosition((int)index,vPos* (Radius + smmplResult[(int)index] * Scale) + m_Origin);
            m_InLR.SetPosition((int)index, vPos * (Radius - smmplResult[(int)index] * Scale) + m_Origin);
            m_MiddleLR.SetPosition((int)index, vPos * (Radius* MiddleCircleRadius - MiddleCircleWide - smmplResult[(int)index]* Scale) + m_Origin);
          
            angle += 360f / smmplResult.Length;
        }
        Sort(smmplResult);
     
        for (float index = 0, angle = 0; index < smmplResult.Length; index++)
        {
            vPos.x = Mathf.Cos(angle * Mathf.Deg2Rad)*Radius* smmplResult[0] * Scale* ScaleCircle + m_Origin.x;// * (Radius+ smmplResult[(int)index] * Scale);
            vPos.y = Mathf.Sin(angle * Mathf.Deg2Rad)* Radius * smmplResult[0] * Scale* ScaleCircle + m_Origin.y;// * (Radius+ smmplResult[(int)index] * Scale);     
            m_CirclePos[(int)index] = vPos;
            angle += 360f / smmplResult.Length;
        }
        m_ScaleLR.SetPositions(m_CirclePos);
        m_OutLR.SetPosition(smmplResult.Length, m_OutLR.GetPosition(0));
        m_InLR.SetPosition(smmplResult.Length, m_InLR.GetPosition(0));
        m_MiddleLR.SetPosition(smmplResult.Length, m_MiddleLR.GetPosition(0));
        m_ScaleLR.SetPosition(smmplResult.Length, m_ScaleLR.GetPosition(0));
    }




    private void Sort(float[] array)
    {
        float value;
        for (int index_1 = 0; index_1 < array.Length; index_1++)
        {
            for (int index_2 = index_1 + 1; index_2 < array.Length; index_2++)
            {
                if (array[index_1] < array[index_2])
                {
                    //交换两者的元素值
                    value = array[index_1];
                    array[index_1] = array[index_2];
                    array[index_2] = value;
                }
            }
        }
    }
}
