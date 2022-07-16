using System.Collections;
using System.Collections.Generic;
using PedroUtils;
using NaughtyAttributes;
using UnityEngine;

namespace GMTK22
{
    public class attackEnemy : MonoBehaviour
    {
        [SerializeField] bool atacando = false;
        [SerializeField] float AttackSpeedMultiplier = 1.0f;

        //float lerpDeltaTime = 0.0f;

        Transform attacker;
        Transform receiver;
		private characterStats attackerStats;
		private characterStats receiverStats;
		Vector3 attackerStartPos;
        

        // Função que chama para atacar inimigo
        public void AtacarInimigo(Transform inputAttacker, Transform inputReceiver, int valorDado)
        {
            attacker = inputAttacker;
            receiver = inputReceiver;

            attackerStats = inputAttacker.GetComponent<characterStats>();
            receiverStats = inputReceiver.GetComponent<characterStats>();

            attackerStartPos = attacker.position;

            atacando = true;
        }

        public void CurarAmigo(Transform inputCurandeiro, Transform inputCurado, int valorDado)
        {
            attacker = inputCurandeiro;
            receiver = inputCurado;

            attackerStats = inputCurandeiro.GetComponent<characterStats>();
            receiverStats = inputCurado.GetComponent<characterStats>();

            receiverStats.ReceberCura(valorDado);
        }

        public void TimeTaunt(List<Transform> amigos, int valorDado)
        {

        }

        [Button]
        public void TestarAtk()
        {
            Transform bonecoTeste1;
            Transform bonecoTeste2;

            bonecoTeste2 = GameObject.Find("Enemy").transform;
            bonecoTeste1 = GameObject.Find("Character").transform;

            AtacarInimigo(bonecoTeste1, bonecoTeste2, 0);
        }

        [Button]
        public void TestarCura()
        {
            Transform bonecoTeste1;
            Transform bonecoTeste2;

            bonecoTeste2 = GameObject.Find("Enemy").transform;
            bonecoTeste1 = GameObject.Find("Character").transform;

            CurarAmigo(bonecoTeste1, bonecoTeste2, 20);
        }

        

        // Start is called before the first frame update
        void Start()
        {
            Transform bonecoTeste1;
            Transform bonecoTeste2;

            bonecoTeste2 = GameObject.Find("Enemy").transform;
            bonecoTeste1 = GameObject.Find("Character").transform;

            AtacarInimigo(bonecoTeste1, bonecoTeste2, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if(atacando)
            {
                float tempDistancia = Vector3.Distance(attacker.position, receiver.position);
                if(tempDistancia > 1)
                {
                    attacker.position = Vector3.Lerp(attacker.position, receiver.position, Time.deltaTime * AttackSpeedMultiplier);
                }
                else
                {
                    atacando = false;

                    receiverStats.ReceberDano(10);
                }
            }
            else
            {
                float tempDistancia = Vector3.Distance(attacker.position, attackerStartPos);
                if(tempDistancia > 0.1f)
                {
                    attacker.position = Vector3.Lerp(attacker.position, attackerStartPos, Time.deltaTime * AttackSpeedMultiplier);
                }
            }
        }
    }
}
