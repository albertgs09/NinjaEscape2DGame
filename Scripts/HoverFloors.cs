using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverFloors : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    bool startPath = true;
    bool endPath;
    bool timeStart;
    float timeLeft = 3;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = start.position;

    }

    // Update is called once per frame
    void Update()
    {
        Path();
        if (timeStart && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }

        if (timeLeft < 0)
        {
            timeStart = false;
            timeLeft = 5;
            endPath = !endPath;
            startPath = !startPath;
        }
    }
    void Path()
    {
        if (startPath)
        {
            transform.position = Vector2.Lerp(transform.position, end.position, Time.deltaTime * 1);
            timeStart = true;
        }
        if (endPath)
        {
            transform.position = Vector2.Lerp(transform.position, start.position, Time.deltaTime * 1);
            timeStart = true;

        }
    }
}
