using NaughtyAttributes;
using PedroUtils;
using UnityEngine;
using System.Collections;

namespace GMTK22
{
	public class characterStats : MonoBehaviour
    {
        [SerializeField] Transform healthBar;

        [SerializeField] int max_health = 20;
        [SerializeField] int health = 20;

        public int valorMinimoTaunt = 0;
        public int valorMinimoHeal = 14;
        public int valorMinimoAtaque = 4;

        int RollD8() => (Mathf.RoundToInt(GetRandom.ValueInRange(new Vector2(1, 8))));
        
        // Função que faz com que o personagem receba dano
        public bool ReceberDano(int valorDado)
        {
            if(valorDado > valorMinimoAtaque)
            {
                health -= RollD8();
                health = Mathf.Clamp(health, 0, max_health);

                if(health <= 0)
                {
                    StartCoroutine(AnimarMorte());
                    Destroy(gameObject, 3);
                }

                //Código para ativar texto de dano aqui...

                return true;
            }

            return false;
        }

        public bool ReceberCura(int valorDado)
        {
            if(valorDado > valorMinimoHeal)
            {
                health += RollD8();
                health = Mathf.Clamp(health, 0, max_health);

                //Código para ativar texto de cura aqui...

                return true;
            }

            return false;
        }
        
		void VidaAcompanharBarra()
        {
            Vector3 barraAtual = healthBar.localScale;
            Vector3 targetBarra = new Vector3((float)health/(float)max_health, healthBar.localScale.y, healthBar.localScale.z);

            healthBar.localScale = Vector3.Lerp(barraAtual, targetBarra, Time.deltaTime * 2);            
        }

        IEnumerator AnimarMorte()
        {
            while(true)
            {
                yield return new WaitForSeconds(0.1f);
                this.Log("First blink!");
            }
        }

        void Update()
        {
            VidaAcompanharBarra();
        }

    }
}
