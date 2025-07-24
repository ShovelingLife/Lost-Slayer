using UnityEngine;
using System.ComponentModel.DataAnnotations;

public class LevelData
{
    [Key]
    public int Id
    {
        get;
        set;
    }
    
    [StringLength(100)]
    public string Name
    {
        get;
        set;
    }
    
    [System.ComponentModel.DataAnnotations.Range(1,10)]
    public int Difficulty
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }
}
