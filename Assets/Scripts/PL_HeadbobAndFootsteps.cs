using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FootstepSCCreator.FootstepsData;
using static FootstepSCCreator;

public class PL_HeadbobAndFootsteps : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] LayerMask walkableLayers;
    CapsuleCollider capsuleCollider;
    PL_Movement pl_Movement;
    SurfaceType surface;
    FootstepsData footstepsData;
    public FootstepSCCreator footstepRef;
    int previousFootstepIndex;
    float landingSoundVolume;
    bool flag;
    Dictionary<string, SurfaceType> physMatNameDict = new Dictionary<string, SurfaceType>() 
    {
        { "phys_concrete (Instance)", SurfaceType.Concrete },
        { "phys_metal (Instance)", SurfaceType.Metal },
        { "phys_trash (Instance)", SurfaceType.Trash },
        { "phys_wet (Instance)", SurfaceType.Wet },
        { "phys_gravel (Instance)", SurfaceType.Gravel },
        { "phys_wood (Instance)", SurfaceType.Wood },
    };

    void FixedUpdate()
    {
        if (pl_Movement.isGrounded && !pl_Movement.landed)
        {
            pl_Movement.landed = true;
            PlayLandingSound();
        }
        else if (!pl_Movement.isGrounded && pl_Movement.landed)
        {
            pl_Movement.landed = false;
            PlayLandingSound();
        }
    }

    void Start()
    {
        pl_Movement = GetComponentInParent<PL_Movement>();
        capsuleCollider = GetComponentInParent<CapsuleCollider>();
    }

    void PlayLandingSound()
    {
        float temp = Mathf.Abs(pl_Movement.rig.velocity.magnitude)/10;
        if (temp < 0.2f)
        {
            return;
        }
        else if(temp >= 0.2f && temp < 0.5f)
        {
            landingSoundVolume = 0.25f;
        }
        else if(temp >= 0.5f && temp < 1f)
        {
            landingSoundVolume = 0.5f;
        }
        else if (temp >= 1f)
        {
            landingSoundVolume = temp;
        }
        audioSource.volume = landingSoundVolume;
        audioSource.clip = GetAudioClip();
        audioSource.Play();
    }

    void PlayFootstepSound()
    {
        if (!pl_Movement.isGrounded) return;
        audioSource.volume = pl_Movement.footstepVolume;
        audioSource.clip = GetAudioClip();
        audioSource.Play();
    }

    AudioClip GetAudioClip()
    {
        CheckSurface();
        foreach (var data in footstepRef.footstepsData)
        {
            if (data.surface == surface)
            {
                footstepsData = data;
                break;
            }
        }
        int clipNum = footstepsData.footstepSoundClips.Count;
        int randomClipIndex;
        AudioClip clip;
        if (clipNum == 1)
        {
            randomClipIndex = 0;
        }
        else
        {
            randomClipIndex = UnityEngine.Random.Range(0, footstepsData.footstepSoundClips.Count);
            if (randomClipIndex == previousFootstepIndex)
            {
                randomClipIndex = (randomClipIndex + 1) % clipNum;
            }
        }
        var pitch = 1 + UnityEngine.Random.Range(-footstepsData.maxPitchShift, footstepsData.maxPitchShift);
        clip = footstepsData.footstepSoundClips[randomClipIndex];
        previousFootstepIndex = randomClipIndex;
        return clip;
    }

    void CheckSurface()
    {
        Ray ray = new Ray(capsuleCollider.transform.position, -capsuleCollider.transform.up);
        if (Physics.SphereCast(ray, 0.2f, out var hit, 1f, walkableLayers))
        {
            foreach (var name in physMatNameDict.Keys)
            {
                if(hit.collider.material.name == name)
                {
                    physMatNameDict.TryGetValue(name, out surface);
                    return;
                }
                physMatNameDict.TryGetValue("phys_concrete (Instance)", out surface);
            }
        }
    }
}
