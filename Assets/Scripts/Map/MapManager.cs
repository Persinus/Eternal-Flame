using UnityEngine;

namespace EternalFlame
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] maps; // Danh sách các map
        private int currentMapIndex = 0; // Map hiện tại

        public static MapManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            // Khởi tạo, chỉ bật map đầu tiên
            ActivateMap(currentMapIndex);
        }

        public void ActivateMap(int mapIndex)
        {
            if (mapIndex < 0 || mapIndex >= maps.Length)
            {
                Debug.LogError("Map index out of range!");
                return;
            }

            // Tắt tất cả map
            foreach (var map in maps)
            {
                map.SetActive(false);
            }

            // Bật map chỉ định
            maps[mapIndex].SetActive(true);
            currentMapIndex = mapIndex;

            Debug.Log($"Activated map: {maps[mapIndex].name}");
        }

        public void NextMap()
        {
            // Chuyển sang map tiếp theo
            int nextIndex = (currentMapIndex + 1) % maps.Length;
            ActivateMap(nextIndex);
        }

        public void PreviousMap()
        {
            // Chuyển sang map trước đó
            int previousIndex = (currentMapIndex - 1 + maps.Length) % maps.Length;
            ActivateMap(previousIndex);
        }
    }
}
