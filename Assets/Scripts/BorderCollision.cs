using UnityEngine;

public class BorderCollision : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Player(Clone)")
        {
            Destroy(collision.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name != "BorderRight" && collision.gameObject.name != "BorderTop" && collision.gameObject.name != "BorderBot")
            if (collision.gameObject.name == "Spikes(Clone)" 
                || collision.gameObject.name == "Rocks(Clone)"
                || collision.gameObject.name == "Crate(Clone)"
                || collision.gameObject.name == "Skulls(Clone)"
                || collision.gameObject.name == "BackTree(Clone)")
            {
                Destroy(collision.gameObject);
            }
    }
}
