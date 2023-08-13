using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightningVFX : MonoBehaviour
{
    public Color onColor;
    public Color offColor;

    public Light dirLight;
    public Camera cam;
    public Material skyboxMat;

    public int onIntensity, offIntensity;

    private void Start()
    {
        StartCoroutine("LightningTrigger");
    }

    IEnumerator LightningTrigger() 
    {
        while(true) 
        {
            yield return new WaitForSeconds(Random.Range(15, 60));
            UnityEngine.RenderSettings.ambientLight = onColor;
            cam.backgroundColor = onColor;
            dirLight.intensity = onIntensity; dirLight.enabled = true;
            dirLight.gameObject.transform.rotation = Quaternion.Euler(75, Random.Range(-360, 360),0);
            skyboxMat.SetFloat("_Exposure", 8f);

            yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
            UnityEngine.RenderSettings.ambientLight = offColor;
            cam.backgroundColor = offColor;
            dirLight.intensity = offIntensity; dirLight.enabled = false;
            skyboxMat.SetFloat("_Exposure", 0.1f);

            yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
            UnityEngine.RenderSettings.ambientLight = onColor;
            cam.backgroundColor = onColor;
            dirLight.intensity = onIntensity; dirLight.enabled = true;
            dirLight.gameObject.transform.rotation = Quaternion.Euler(75, Random.Range(-360, 360), 0);
            skyboxMat.SetFloat("_Exposure", 8f);

            yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
            UnityEngine.RenderSettings.ambientLight = offColor;
            cam.backgroundColor = offColor;
            dirLight.intensity = onIntensity; dirLight.enabled = false;
            skyboxMat.SetFloat("_Exposure", 0.1f);
        }
    }
}
