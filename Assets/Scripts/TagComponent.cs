using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagComponent : MonoBehaviour 
{
    [SerializeField] SecTag tag;

    void Awake()
    {
        tag = SecTag.Enemy;
    }

    public void SetTagType(SecTag newTag)
    {
        tag = newTag;
    }

    public SecTag GetTagType()
    {
        return tag;
    }
}
