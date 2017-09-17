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
            string trimmedLine = line.TrimEnd('\r');
            if (string.IsNullOrEmpty(trimmedLine))
            {
                continue;
            }
            parsedSentences.Add(Sentence.ParseLine(trimmedLine));
        }

        return new Dialogue(parsedSentences);
    }

    public Dialogue (List<Sentence> sentences)
    {
        this._sentences = sentences;
    }
}
