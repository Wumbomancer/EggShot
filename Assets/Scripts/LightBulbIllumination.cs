using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulbIllumination : MonoBehaviour {

    public Light[] oddLights = new Light[8];
    public Light[] evenLights = new Light[8];
    public float intensity;
    public bool reset;
    public bool exitPauseMenu;

    private int seed = 0;
    private int seedLimit = 3;

    public void Start()
    {
        reset = false;
        seed = (int)Random.Range(0, 2.99f);
        exitPauseMenu = false;
    }

    public void Update()
    {
        if(reset)
        {
            int Temp = seed;
            seed++;
            if (seed > seedLimit)
                seed = 0;
            switch(Temp)
            {
                case 0: StartCoroutine(SwitchLightShow());
                    break;
                case 1: StartCoroutine(RoundaboutLightShow());
                    break;
                case 2: StartCoroutine(SwitchLightShow());
                    break;
                case 3: StartCoroutine(RoundaboutBackwardsLightShow());
                    break;
                default: break;
            }
            
            reset = false;
        }

        
    }

    

    public IEnumerator SwitchLightShow()
    {
        int count = 0;
        while (count < 3)
        {
            for (int i = 0; i < 8; i++)
            {
                oddLights[i].intensity = intensity;
            }
            for (int i = 0; i < 8; i++)
            {
                evenLights[i].intensity = 0;
            }

            yield return new WaitForSeconds(1.2f);

            for (int i = 0; i < 8; i++)
            {
                oddLights[i].intensity = 0;
            }
            for (int i = 0; i < 8; i++)
            {
                evenLights[i].intensity = intensity;
            }

            yield return new WaitForSeconds(1.2f);
            count++;
        }
        
        reset = true;

        yield break;


    }

    public IEnumerator RoundaboutLightShow()
    {

        for (int i = 0; i < 8; i++)
        {
            oddLights[i].intensity = 0;
        }
        for (int i = 0; i < 8; i++)
        {
            evenLights[i].intensity = 0;
        }

        
        for (int i = 0; i < 8; i++)
        {
            oddLights[i].intensity = intensity*1.7f;
            
            yield return new WaitForSeconds(.1f);
            oddLights[i].intensity = 0;
            evenLights[i].intensity = intensity*1.7f;
            yield return new WaitForSeconds(.1f);
            evenLights[i].intensity = 0;
        }

        
        for (int i = 0; i < 8; i++)
        {
            oddLights[i].intensity = intensity*1.7f;
            
            yield return new WaitForSeconds(.1f);
            oddLights[i].intensity = 0;
            evenLights[i].intensity = intensity*1.7f;
            yield return new WaitForSeconds(.1f);
            evenLights[i].intensity = 0;
        }
        reset = true;

    }

    public IEnumerator RoundaboutBackwardsLightShow()
    {
        for (int i = 0; i < 8; i++)
        {
            oddLights[i].intensity = 0;
        }
        for (int i = 0; i < 8; i++)
        {
            evenLights[i].intensity = 0;
        }


        for (int i = 7; i > -1; i--)
        {
            evenLights[i].intensity = intensity*1.7f;

            yield return new WaitForSeconds(.1f);
            evenLights[i].intensity = 0;
            oddLights[i].intensity = intensity*1.7f;

            yield return new WaitForSeconds(.1f);
            oddLights[i].intensity = 0;

        }


        for (int i = 7; i > -1; i--)
        {
            evenLights[i].intensity = intensity * 1.7f;

            yield return new WaitForSeconds(.1f);
            evenLights[i].intensity = 0;
            oddLights[i].intensity = intensity * 1.7f;

            yield return new WaitForSeconds(.1f);
            oddLights[i].intensity = 0;

        }
        reset = true;
    }
}
