using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentence {

    public string _text;
    public float _speed;
    public float _waitTime;

    public static Sentence ParseLine(string text)
    {
        string[] splitLine = text.Split('~');

        float parsedSpeed = float.Parse(splitLine[0]);
        float parsedWaitTime = float.Parse(splitLine[1]);
        string parsedText = splitLine[2];

        return new Sentence(parsedText, parsedSpeed, parsedWaitTime);
    }

    public Sentence (string text, float speed, float waitTime) {
        this._text = text;
        this._speed = speed;
        this._waitTime = waitTime;
    }
}
