using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelManagement;
public class MoveByTouch : MonoBehaviour {

    public float moveRange;//能移動的上下範圍
    public float Yincrement;//移動的距離
    //touch movement
    protected Vector2 startPos;
    protected Vector2 direction;
    protected Vector2 targetPos;
    
    public float timeToMove;
    public bool directionChosen;
    private bool isMoving;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update () 
    {
        if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);
            switch(touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    directionChosen = false;
                    break;
                case TouchPhase.Moved:
                    direction = touch.position - startPos;//判定方向:往上還是往下
                    break;
                case TouchPhase.Ended:
                    directionChosen = true;//選好方向了
                    break;
            }
        }
        if(directionChosen)
        {
            if(direction.y>0f)
            {
                StopAllCoroutines();
                targetPos = new Vector2(transform.position.x , transform.position.y+Yincrement);
                direction = Vector2.zero;
                StartCoroutine(MoveRountine(targetPos));
            }
            else if(direction.y<0f)
            {
                StopAllCoroutines();
                targetPos = new Vector2(transform.position.x , transform.position.y-Yincrement);
                direction = Vector2.zero;
                StartCoroutine(MoveRountine(targetPos));
            }
        }
    }
    public bool Overdistance()
    {
        return ((Mathf.Abs(targetPos.y) > moveRange));//限制在固定的範圍移動
    }
    IEnumerator MoveRountine(Vector2 destination)
    {        
        if(Overdistance())
        {
            yield break;//移動超出範圍就停止
        }
        float elasptime = 0;
        bool reachTheDes = false;
        while (!reachTheDes)
        {
            if (Vector2.Distance(transform.position, destination) < 0.01f)
            {
                reachTheDes = true;
                transform.position = destination;
                isMoving = false;
            }
            elasptime += Time.deltaTime;
            float t = Mathf.Clamp(elasptime / timeToMove, 0, 1);
            t = t * t * (3 - 2 * t);//smoothstep 讓移動平滑
            transform.position = Vector2.Lerp(transform.position, destination, t);//物體移動
            yield return null;
        }
    }
}
