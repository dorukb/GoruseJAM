using UnityEngine;

[System.Serializable]
public class Dialogue 
{
    public Line[] lines;
    
    [System.Serializable]
    public class Line
    {
        [TextArea(3, 10)]
        public string sentence;
        public string speaker;
        public AudioClip clip;
    }
}
