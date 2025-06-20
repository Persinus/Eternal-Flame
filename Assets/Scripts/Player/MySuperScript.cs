using LoM.Super;
using UnityEngine;

namespace EternalFlame 
{
    [SuperIcon("Assets/Gizmos/a.jpg")] 
    public class AnhTraiHoangPhan : SuperBehaviour
    {
        [SerializeField] private float IQ = 0;
        [SerializeField] private float level_of_Beauty = 0;
        [SerializeField] private float EQ = 0;
        [SerializeField] private float Hardworking = 0;
        [SerializeField] private float level_of_Kindness = 0;
        [SerializeField] private float level_of_Wealth = 0;
        [SerializeField] private float level_of_Health = 0;


        public float Rotation { get; set; }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {


        }
    }
}
