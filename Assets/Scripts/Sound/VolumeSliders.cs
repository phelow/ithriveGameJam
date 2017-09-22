using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeSliders : MonoBehaviour {

    public Slider master;
    public Slider music;
    public Slider effects;

    void Update () {
        Global.soundManager.SetVolumes(master.value, music.value, effects.value);
    }
}
