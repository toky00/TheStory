using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Footstep_Reference", menuName = "FootstepSO", order = 1)]
public class FootstepSCCreator : ScriptableObject
{
    [Serializable] public struct FootstepsData
    {
        public enum SurfaceType
        {
            Wood,
            Concrete,
            Metal,
            Gravel,
            Wet,
            Trash,
        }
        public SurfaceType surface;
        public List<AudioClip> footstepSoundClips;
        [Range(0, 0.2f)] public float maxPitchShift;
    }
    public FootstepsData[] footstepsData;
}
