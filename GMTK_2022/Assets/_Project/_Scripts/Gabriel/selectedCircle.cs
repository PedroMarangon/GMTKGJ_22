using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK22
{
    public class selectedCircle : MonoBehaviour
    {
        GameManager gm;
        [SerializeField] SpriteRenderer targetSprite;

        // Start is called before the first frame update
        void Start()
        {
            gm = FindObjectOfType<GameManager>();
        }

        // Update is called once per frame
        void Update()
        {
            targetSprite.enabled = false;

            if(gm.CrntTarget == gameObject.transform)
            {
                targetSprite.enabled = true;
            }
            // else if(gm.CrntAlien.gameObject.transform == gameObject.transform)
            // {
            //     targetSprite.GetComponent<SpriteRenderer>().enabled = true;
            // }
        }
    }
}
