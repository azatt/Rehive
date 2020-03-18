using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Start is called before the first frame update
    public int speed;
    public int size;
    public int camo;
        
    public enum Type {Speed, Size, Camo};

    public Stats(int size, int speed, int camo)
    {
        this.size = size;
        this.speed = speed;
        this.camo = camo;
    }
    
    public void AddStats(Type type, int amount)
    {

        switch (type)
        {
            case Type.Speed:
                speed += amount;
                break;
            case Type.Size:
                size += amount;
                break;
            case Type.Camo:
                camo += amount;
                break;
        }
    }
    
}
