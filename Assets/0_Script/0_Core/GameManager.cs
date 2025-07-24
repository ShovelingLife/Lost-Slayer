using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

public class GameManager : SingletonGlobal<GameManager>
{
    public static Player player
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
        isInitialized = true;
    }
    
    // 두번 초기화 막음
    public bool IsInitialized() => isInitialized;


}
