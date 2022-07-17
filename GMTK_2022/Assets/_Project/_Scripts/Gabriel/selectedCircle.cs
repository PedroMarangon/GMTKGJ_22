using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK22
{
    public class selectedCircle : MonoBehaviour
    {
        GameManager gm;
        attackEnemy funcaoAtacar;
        [SerializeField] SpriteRenderer targetSprite;

        // Start is called before the first frame update
        void Start()
        {
            gm = FindObjectOfType<GameManager>();
            funcaoAtacar = FindObjectOfType<attackEnemy>();
        }

        // Update is called once per frame
        void Update()
        {
            targetSprite.enabled = false;

            if(!funcaoAtacar.isRunning)
            {
                if(gm.CrntTarget == gameObject.transform)
                {
                    targetSprite.color = Color.red;
                    if(gm.CrntTarget.GetComponent<Alien>() != null)
                    {
                        targetSprite.color = Color.blue;
                    }
                    targetSprite.enabled = true;
                }
                else if(gm.CrntAlien != null && gm.CrntAlien.transform == gameObject.transform)
                {
                    targetSprite.color = Color.green;
                    targetSprite.enabled = true;
                }
            }

        }
    }
}
