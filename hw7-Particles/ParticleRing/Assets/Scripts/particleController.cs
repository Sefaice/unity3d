using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleController : MonoBehaviour {

    public ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particlesArray;
    private float[] particleTheta;
    private float[] particleRadius;
    private float[] particleRadiusSmall;
    private float[] particleRadiusLarge;

    public int particleNum = 10000;
    private float minRadius = 2.5f;
    private float maxRadius = 4.5f;
    private float minSpeed = 2f;
    private float maxSpeed = 3f;

    private bool expand;
    public Gradient colorGradient;

    void Start()
    {
        particlesArray = new ParticleSystem.Particle[particleNum];
        particleTheta = new float[particleNum];
        particleRadius = new float[particleNum];
        particleRadiusLarge = new float[particleNum];
        particleRadiusSmall = new float[particleNum];
        particleSystem.maxParticles = particleNum;
        particleSystem.Emit(particleNum);
        particleSystem.GetParticles(particlesArray);
        expand = true;

        // 初始化梯度颜色控制器  
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[5];
        alphaKeys[0].time = 0.0f; alphaKeys[0].alpha = 1.0f;
        alphaKeys[1].time = 0.4f; alphaKeys[1].alpha = 0.4f;
        alphaKeys[2].time = 0.6f; alphaKeys[2].alpha = 1.0f;
        alphaKeys[3].time = 0.9f; alphaKeys[3].alpha = 0.4f;
        alphaKeys[4].time = 1.0f; alphaKeys[4].alpha = 0.9f;
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].time = 0.0f; colorKeys[0].color = Color.white;
        colorKeys[1].time = 1.0f; colorKeys[1].color = Color.white;
        colorGradient.SetKeys(colorKeys, alphaKeys);

        InitialPosition();
    }

    //put them on a circle
    public void InitialPosition()
    {
        for(int i = 0; i < particleNum; i++)
        {
            particleTheta[i] = UnityEngine.Random.Range(0f, 360f);
            float ran = (float)NormalRand();
            while(ran<-0.8f||ran>0.8f)
            {
                ran = (float)NormalRand();
            }
            particleRadius[i] = ran + maxRadius;
            particleRadiusLarge[i] = ran + maxRadius;
            particleRadiusSmall[i] = ran/2 + minRadius;
            particlesArray[i].position = new Vector3(particleRadius[i] * Mathf.Cos(particleTheta[i] / 180 * Mathf.PI), 0, particleRadius[i] * Mathf.Sin(particleTheta[i] / 180 * Mathf.PI));
        }
        particleSystem.SetParticles(particlesArray, particleNum);
    }

    //rotate
    void Update()
    {
        for (int i = 0; i < particleNum; i++)
        {
            /*这种方法通过规定一个小环和大环的半径数值进行切换
            float ran = (float)NormalRand();
            while (ran < -1 || ran > 1)
            {
                ran = (float)NormalRand();
            }
            if (!expand && particleRadius[i]>=minRadius+ran)
            {
                particleRadius[i] -= 0.1f * (particleRadius[i] / 0.1f) * Time.deltaTime;
            }
            else if(expand && particleRadius[i] < minRadius+ran)
            {
                particleRadius[i] += 0.1f * (particleRadius[i] / 0.1f) * Time.deltaTime;
            }
            */

            //另一种方法记录旧位置
            if(expand && particleRadius[i] < particleRadiusLarge[i])
            {
                particleRadius[i] += 10f * (particleRadiusLarge[i] * 0.1f) * Time.deltaTime;
            }
            else if(!expand && particleRadius[i] > particleRadiusSmall[i])
            {
                particleRadius[i] -= 10f * (particleRadiusSmall[i]*0.1f) * Time.deltaTime;
            }

            particlesArray[i].color = colorGradient.Evaluate(particleTheta[i] / 360.0f);

            particleTheta[i] += UnityEngine.Random.Range(0f, 0.5f);
            particleTheta[i] = particleTheta[i] % 360;
            particlesArray[i].position = new Vector3(particleRadius[i] * Mathf.Cos(particleTheta[i] / 180 * Mathf.PI), 0, particleRadius[i] * Mathf.Sin(particleTheta[i] / 180 * Mathf.PI));
        }
        particleSystem.SetParticles(particlesArray, particleNum);
    }

    //normal distribution
    public double NormalRand()
    {
        float u, v;
        int phase = UnityEngine.Random.Range(0,2);
        u = UnityEngine.Random.Range(0f, 1f);
        v = UnityEngine.Random.Range(0f, 1f);
        double z = 0;

        if (phase == 0)
        {
            z = Math.Sqrt(-2.0f * Math.Log(u)) * Mathf.Sin(2.0f * Mathf.PI * v);
        }
        else
        {
            z = Math.Sqrt(-2.0f * Math.Log(u)) * Mathf.Cos(2.0f * Mathf.PI * v);
        }
        //Debug.Log(z);
        return z/4;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect((Screen.width) / 2+200, Screen.height / 2 - 20, 80, 40), "收缩"))
        { 
            if(expand)
            {
                expand = false;
            }
            else
            {
                expand = true;
            }
        }
    }


}
