using NaughtyAttributes;
using PedroUtils;
using UnityEngine;

namespace GMTK22
{
	public class characterStats : MonoBehaviour
    {
        [SerializeField] Transform healthBar;

        [SerializeField] int max_health = 20;
        [SerializeField] int health = 20;

        // Verificar isso dps...
        [MinMaxSlider(1, 8), SerializeField] private Vector2 damageRange = new Vector2(1, 2);
        private int _damage;
        public int Damage => _damage;
        public void SetDamage() => _damage = (Mathf.RoundToInt(GetRandom.ValueInRange(damageRange)) * multiplier);
        public int multiplier = 1;

        public int valorMinimoTaunt = 0;
        public int valorMinimoHeal = 14;
        public int valorMinimoAtaque = 4;

        // Função que faz com que o personagem receba dano
        public bool ReceberDano(int valorDado)
        {
            if(valorDado > valorMinimoAtaque)
            {
                health -= valorDado;
                health = Mathf.Clamp(health, 0, max_health);

                //Código para ativar texto de dano aqui...

                return true;
            }

            return false;
        }

        public bool ReceberCura(int valorDado)
        {
            if(valorDado > valorMinimoHeal)
            {
                health = max_health;
                health = Mathf.Clamp(health, 0, max_health);

                //Código para ativar texto de cura aqui...

                return true;
            }

            return false;
        }

        public bool ReceberTaunt(int valorDado)
        {
            if(valorDado > valorMinimoTaunt)
            {
                multiplier = 2;
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

        void Update()
        {
            VidaAcompanharBarra();
        }

    }
}
