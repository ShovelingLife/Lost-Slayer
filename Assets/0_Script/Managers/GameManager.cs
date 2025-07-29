using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

public class GameManager : SingletonGlobal<GameManager>
{
    public static Player player
    {
        get; 
        private set;
    }
    
    public static SidePlayer sidePlayer
    {
        get; 
        private set;
    }

    bool isInitialized = false;

    private void Awake()
    {
        Init();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        if (isInitialized)
            return;

        player = FindAnyObjectByType<Player>();
        sidePlayer = FindAnyObjectByType<SidePlayer>();
        isInitialized = true;
    }
    
    // 
    public bool IsInitialized() => isInitialized;


}
