using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed;

    [Header("Points")]
    [SerializeField] private Transform[] points;

    private int targetIndex;

    private Player player;

    private void Start()
    {
        targetIndex = 0;
        transform.position = points[targetIndex].position;
    }

    private void FixedUpdate()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, points[targetIndex].position,speed * Time.fixedDeltaTime);

        if (0.05f > Vector2.Distance(transform.position, points[targetIndex].position))
        {
            if (targetIndex == 0) targetIndex = 1;
            else if (targetIndex == 1) targetIndex = 0;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            foreach(ContactPoint2D contact in collision.contacts)
            {
                if(contact.normal.y < -0.5f)
                {
                    player = collision.gameObject.GetComponent<Player>();
                    player.physicsControl.setExtrapolate();
                    collision.transform.SetParent(this.transform);
                    break;
                }
            }
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(gameObject.activeInHierarchy == true)
            {
                if(player  != null)
                    player.physicsControl.setInterpolate();
                collision.transform.SetParent(null);
                player = null;
            }
        }
    }
}
