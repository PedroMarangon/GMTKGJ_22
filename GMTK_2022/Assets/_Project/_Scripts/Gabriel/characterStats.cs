using NaughtyAttributes;
using PedroUtils;
using UnityEngine;
using System.Collections;

namespace GMTK22
{
	public class characterStats : MonoBehaviour
    {
        [SerializeField] Transform healthBar;
		[SerializeField] private bool isRobot;

		[SerializeField] int max_health = 20;
        [SerializeField] int health = 20;
        [SerializeField] SpriteRenderer sprite;

        [SerializeField] popupDmgManager DmgManager;

        public int valorMinimoHeal = 14;
        public int valorMinimoAtaque = 4;
        private GameManager gameManager;

        public bool IsRobot => isRobot;
        int RollD8() => (Mathf.RoundToInt(GetRandom.ValueInRange(new Vector2(1, 8))));
        
        // Função que faz com que o personagem receba dano
        public bool ReceberDano(int valorDado)
        {
            if(valorDado >= valorMinimoAtaque)
            {
                int dmg = RollD8();

                health -= dmg;
                DmgManager.MostrarDano(-dmg, transform);
                health = Mathf.Clamp(health, 0, max_health);

                if(health <= 0)
                {
                    StartCoroutine(AnimarMorte());
                }

                //Código para ativar texto de dano aqui...

                return true;
            }
            else
            {
                DmgManager.MostrarDano(0, transform);
                gameManager.PlayNoEffectSound();
            }

            return false;
        }

        public bool ReceberCura(int valorDado)
        {
            if(valorDado >= valorMinimoHeal)
            {
                //this.LogSucess("Foi curado!");
                int healValue = RollD8();

                health += healValue;
                gameManager.PlayHealSound();
                DmgManager.MostrarDano(healValue, transform);
                health = Mathf.Clamp(health, 0, max_health);

                return true;
            }
            else
            {
                DmgManager.MostrarDano(0, transform);
                gameManager.PlayNoEffectSound();
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
            var delay = new WaitForSeconds(0.15f);
			for (int i = 0; i < 7; i++)
			{
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
                yield return delay;
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
                yield return delay;
            }
            GameManager.UnitIsDead.Invoke(isRobot, transform);

            Destroy(gameObject);
        }

        void Start()
        {
            DmgManager = FindObjectOfType<popupDmgManager>();
            gameManager = FindObjectOfType<GameManager>();
        }

        void Update()
        {
            VidaAcompanharBarra();
        }
    }
}
