using UnityEngine;

public class Elevator : MonoBehaviour
{

    private bool canMove;
    public float speed;
    public int startPoint;
    public Transform[] points;
    private int i;
    private bool reverse;

    // Start is called before the first frame update
    void Start()
    {
        canMove = false;
        transform.position = points[startPoint].position;
        i = startPoint;
    }

    // Update is called once per frame
    void Update()
    {

        if(Vector3.Distance(transform.position, points[i].position) < 0.01f)
        {
            canMove = false;
            if (i == points.Length - 1)
            {

                reverse = true;
                i--;
                return;

            }else if(i == 0)
            {
                reverse = false;
                i++;
                return;

            }

            if (reverse)
                i--;
            else
                i++;
        }

        if(canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            canMove = true;
        }

    }

    public bool GetState()
    {
        return reverse;
    }

    public void Move()
    {
        canMove = true;
    }

}
