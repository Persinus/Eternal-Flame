using UnityEngine;

namespace EternalFlame 
{
    public class BulletKurumi : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        [SerializeField] float speed = 10f; // Tốc độ bắn của viên đạn

        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.linearVelocity = Vector2.right * speed; // Bắn sang phải với tốc độ đã định
            Destroy(gameObject, 4f); // Tự hủy sau 4 giây
        }
    }
}
