using UnityEditor.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Player Related

    public float speed = 1.0f;
    public PlayerStat stat;
    Vector3 pos;
    
    public PlayerStat Stat
    {
        get => stat;
    }

    #endregion
    
    #region  Animation Related

    EPlayerState state;
    Animator animator;

        #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var newPos = transform.position + (pos * speed * Time.deltaTime);
        transform.position = newPos;
        animator?.SetInteger("State", (int)state);
    }

    public void Move(Vector3 _dir)
    {
        //Debug.Log($"direction : {_direction}");
        // Debug.Log(state.ToString());
        pos = _dir;
        state = (_dir != Vector3.zero) ? EPlayerState.MOVE : EPlayerState.IDLE;
    }
}
