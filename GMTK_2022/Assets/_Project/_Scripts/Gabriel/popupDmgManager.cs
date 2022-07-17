using System.Collections;
using System.Collections.Generic;
using TMPro;
using PedroUtils;
using UnityEngine;

namespace GMTK22
{
    public class popupDmgManager : MonoBehaviour
    {
        [SerializeField] GameObject prefab_popup;
        
        public void MostrarDano(int dano, Transform targetReceiver)
        {
            Transform temp;

            Vector3 v3Temp = targetReceiver.position;
            v3Temp.y += 1;
            v3Temp.x -= 1;

            temp = Instantiate(prefab_popup.transform, v3Temp, Quaternion.identity, gameObject.transform);

            temp.GetComponent<TMP_Text>().color = (dano > 0 ? Color.red : Color.green);
            temp.GetComponent<TMP_Text>().text = (dano > 0 ? "-" + dano.ToString() : "+" + dano.ToString());
        }

        // Start is called before the first frame update
        void Start()
        {
            //MostrarDano(-10);
        }

        void Update()
        {

        }
    }
}
