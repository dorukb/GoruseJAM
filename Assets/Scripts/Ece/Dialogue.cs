using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "newDialog", menuName = "Dialog")]
public class Dialogue : ScriptableObject
{
    public Line[] lines;
    public bool isLast = false;
    
    [System.Serializable]
    public class Line
    {
        [TextArea(3, 10)]
        public string sentence;
        public string speaker;
        public AudioClip clip;
    }
}
