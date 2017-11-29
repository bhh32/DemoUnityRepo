using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour 
{
    public SecTag[] secondaryTags = new SecTag[2];

    void Start()
    {
        secondaryTags[0] = SecTag.Friendly; 
        secondaryTags[1] = SecTag.Enemy;
    }

    public bool HasTag(SecTag checkSecTag)
    {
        foreach (SecTag tag in secondaryTags)
        {
            if (tag == checkSecTag)
                return true;
        }

        return false;
    }
}

public enum SecTag
{
    Friendly,
    Enemy
}
