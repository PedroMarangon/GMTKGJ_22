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

        public bool isRunning = false;

        //float lerpDeltaTime = 0.0f;

        Transform attacker;
        Transform receiver;
		private characterStats attackerStats;
		private characterStats receiverStats;
        private GameManager manager;
		Vector3 attackerStartPos;

        Animator attackerAnimador;

        int valorDado = 0;
        
        int defaultLayer = 0;
        // Função que chama para atacar inimigo
        public void AtacarInimigo(Transform inputAttacker, Transform inputReceiver, int inputValorDado)
        {
            if(!isRunning)
            {
                valorDado = inputValorDado;
                isRunning = true;

                attacker = inputAttacker;
                receiver = inputReceiver;

                attackerStats = inputAttacker?.GetComponent<characterStats>();
                receiverStats = inputReceiver?.GetComponent<characterStats>();

                attackerStartPos = attacker.position;

                defaultLayer = inputAttacker.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder;
                inputAttacker.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 999;

                attackerAnimador = inputAttacker.GetChild(0).GetComponent<Animator>();
                attackerAnimador.Play("Walking");
                atacando = true;
            }
        }

        public void CurarAmigo(Transform inputCurandeiro, Transform inputCurado, int inputValorDado)
        {
            if(!isRunning)
            {
                valorDado = inputValorDado;
                isRunning = true;

                attacker = inputCurandeiro;
                receiver = inputCurado;

                attackerStats = inputCurandeiro.GetComponent<characterStats>();
                receiverStats = inputCurado.GetComponent<characterStats>();

                receiverStats.ReceberCura(valorDado);
                isRunning = false;
            }
        }

        [Button]
        public void TestarAtk()
        {
            Transform bonecoTeste1;
            Transform bonecoTeste2;

            bonecoTeste2 = GameObject.Find("Enemy").transform;
            bonecoTeste1 = GameObject.Find("Character").transform;

            AtacarInimigo(bonecoTeste1, bonecoTeste2, 20);
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

		private void Start()
		{
            manager = FindObjectOfType<GameManager>();
		}


		bool podeDarDano = true;
        // Update is called once per frame
        void Update()
        {
            if(atacando)
            {
                float tempDistancia = Vector3.Distance(attacker.position, receiver.position);
                if(tempDistancia > 1.2f)
                {
                    attacker.position = Vector3.Lerp(attacker.position, receiver.position, Time.deltaTime * AttackSpeedMultiplier);
                }
                else
                {
                    attackerAnimador.Play("Attack");

                    if(attackerStats.IsRobot) manager?.PlayAtkRobotSound();
                    else manager?.PlayAtkAlienSound();

                    atacando = false;
                }
            }
            else if(isRunning && !attackerAnimador.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if(podeDarDano)
                {
                    podeDarDano = false;
                    receiverStats.ReceberDano(valorDado);
                    attacker.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = defaultLayer;
                }

                float tempDistancia = Vector3.Distance(attacker.position, attackerStartPos);
                if(tempDistancia > 0.1f)
                {
                    attacker.position = Vector3.Lerp(attacker.position, attackerStartPos, Time.deltaTime * AttackSpeedMultiplier);
                }
                else if(isRunning)
                {
                    attackerAnimador.Play("Idle");
                    podeDarDano = true;
                    isRunning = false;
                }
            }
        }
    }
}
