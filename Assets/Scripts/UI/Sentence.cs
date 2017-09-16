using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentence {

    public string text;
    public float speed;
    public float waitTime;

    public Sentence (string text, float speed, float waitTime) {
        this.text = text;
        this.speed = speed;
        this.waitTime = waitTime;
    }
}
