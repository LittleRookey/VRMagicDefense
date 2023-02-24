using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterContainer : MonoBehaviour
{
    public MonsterInfo[] m_Container;

    [System.Serializable]
    public struct MonsterInfo
    {
        public string m_name;
        public Monster monster;
    }

    public Monster GetMonster(string name)
    {
        foreach (MonsterInfo mInfo in m_Container)
        {
            if (mInfo.m_name == name)
                return mInfo.monster;
        }
        return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
