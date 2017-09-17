using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue {
    
    public List<Sentence> _sentences;

    public static Dialogue DialogueFactory(TextAsset asset)
    {
        string[] lines = asset.text.Split('\n');
        List<Sentence> parsedSentences = new List<Sentence>();
        foreach(string line in lines)
        {
            parsedSentences.Add(Sentence.ParseLine(line));
        }

        return new Dialogue(parsedSentences);
    }

    public Dialogue (List<Sentence> sentences)
    {
        this._sentences = sentences;
    }
}
