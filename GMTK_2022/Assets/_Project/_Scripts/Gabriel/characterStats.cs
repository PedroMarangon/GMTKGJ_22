using System.Collections;
using System.Collections.Generic;
using PedroUtils;
using UnityEngine;

namespace GMTK22
{
    public class characterStats : MonoBehaviour
    {
        [SerializeField] Transform healthBar;
        [SerializeField] float m_health = 100;

        public float health
        {
            get
            {
                return m_health;
            }
            set
            {
                m_health = value;
                healthBar.localScale = new Vector3(healthBar.localScale.x - 0.5f, healthBar.localScale.y, healthBar.localScale.z);
            }
        }




        // // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
