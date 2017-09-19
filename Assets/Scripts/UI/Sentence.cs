using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentence {

    public string _text;
    public float _speed;
    public float _waitTime;
    public float _spookiness;
    public string[] _words;
    public int _soundIndex;

    public static Sentence ParseLine(string text)
    {
        string[] splitLine = text.Split('~');

        float parsedSpeed = float.Parse(splitLine[0]);
        float parsedWaitTime = float.Parse(splitLine[1]);
        float spookiness = float.Parse(splitLine[2]);  
        string parsedText = splitLine[3];
        string [] extraWords = splitLine.Length > 0 ? splitLine[4].Split('*') : new string [] { };

        int soundIndex = 0;
        if(splitLine.Length > 5) {
            soundIndex = int.Parse(splitLine[5]);
        }
        

        return new Sentence(parsedText, parsedSpeed, parsedWaitTime, spookiness, extraWords, soundIndex);
    }

    public Sentence (string text, float speed, float waitTime, float spookiness, string [] extraWords, int soundIndex)
    {
        this._text = text;
        this._speed = speed;
        this._waitTime = waitTime;
        this._spookiness = spookiness;
        this._words = extraWords;
        this._soundIndex = soundIndex;
    }
}
