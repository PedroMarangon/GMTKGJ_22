using System.Collections;
using System.Collections.Generic;
using TMPro;
using PedroUtils;
using UnityEngine;

namespace GMTK22
{
    public class popupDmg : MonoBehaviour
    {
        Vector3 desiredPos;
        TMP_Text text;

        void Start()
        {
            text = transform.GetComponent<TMP_Text>();

            desiredPos = transform.position;
            desiredPos.y += 1;
        }

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * 2);

            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), Time.deltaTime * 3);

            float tempDistancia = Vector3.Distance(transform.position, desiredPos);
            //this.Log(tempDistancia);
            if(tempDistancia < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
