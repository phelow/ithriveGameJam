using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentence {

    public string _text;
    public float _speed;
    public float _waitTime;
    public float _spookiness;
    public string[] _words;

    public static Sentence ParseLine(string text)
    {
        string[] splitLine = text.Split('~');

        float parsedSpeed = float.Parse(splitLine[0]);
        float parsedWaitTime = float.Parse(splitLine[1]);
        float spookiness = float.Parse(splitLine[2]);  
        string parsedText = splitLine[3];
        string [] extraWords = splitLine.Length > 0 ? splitLine[4].Split('*') : new string [] { };

        return new Sentence(parsedText, parsedSpeed, parsedWaitTime, spookiness, extraWords);
    }

    public Sentence (string text, float speed, float waitTime, float spookiness, string [] extraWords)
    {
        this._text = text;
        this._speed = speed;
        this._waitTime = waitTime;
        this._spookiness = spookiness;
        this._words = extraWords;
    }
}
