using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PL_MouseLook : MonoBehaviour
{
    [SerializeField] GameObject headCam;
    [SerializeField] GameObject tiltControl;
    [SerializeField] AudioSource tiltAudioSource;
    [SerializeField] AudioClip tiltSFX;
    [SerializeField, Range(0.1f, 10f)] float sensitivity = 1;
    float minX = -90, maxX = 90;
    float x, y;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        tiltAudioSource.clip = tiltSFX;
    }

    void Update()
    {
        MouseLook();
    }

    void MouseLook()
    {
        x += Input.GetAxis("Mouse Y") * sensitivity;
        x = Mathf.Clamp(x, minX, maxX);
        y += Input.GetAxis("Mouse X") * sensitivity;

        float tilt = -Input.GetAxis("Mouse X") * 4;
        float tiltVolume = Mathf.Abs(Input.GetAxis("Mouse X")/25);

        tiltVolume = Mathf.Clamp(tiltVolume, 0, 0.25f);
        tilt = Mathf.Clamp(tilt, -15, 15);

        gameObject.transform.rotation = Quaternion.Euler(0, y, 0);
        headCam.transform.localRotation = Quaternion.Euler(-x, headCam.transform.localRotation.y, headCam.transform.localRotation.z);

        tiltControl.transform.localRotation = Quaternion.Lerp(tiltControl.transform.localRotation, Quaternion.Euler(tiltControl.transform.localRotation.y, tiltControl.transform.localRotation.y, tilt), Time.deltaTime * 5);

        tiltAudioSource.loop = true;
        tiltAudioSource.volume = Mathf.Lerp(tiltAudioSource.volume, tiltVolume, Time.deltaTime * 5);
        if(tiltVolume <= 0.01f)
        {
            tiltAudioSource.Pause();
        }
        else if(tiltVolume >= 0.01f)
        {
            if (!tiltAudioSource.isPlaying) tiltAudioSource.Play();
            tiltAudioSource.UnPause();
        }
    }
}
