using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static string playerName = "Player";
    public int startingHealth = 100;
    public int currentHealth;
    public int maxHealth = 500;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;                                                
    bool damaged;                                               

    Text healthText;

    void Awake()
    {
        // Mendapatkan reference component.
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();

        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = startingHealth;
        
        // Get the healthText from hudcanvas.
        healthText = GameObject.Find("Health").GetComponent<Text>();
        
        GameObject.Find("PlayerName").GetComponent<TMP_Text>().text = playerName;
    }


    void Update()
    {
        // Jika terkena damage.
        if (damaged)
        {
            // Mengubah warna gambar menjadi value dari flashColour.
            damageImage.color = flashColour;
        }
        else
        {
            // Fade out damage image.
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        healthText.text = currentHealth.ToString();
        damaged = false;
    }


    // Fungsi Untuk Mendapatkan damage. 
    public void TakeDamage(int amount)
    {
        damaged = true;

        // Mengurangi currentHealth dengan amount.
        currentHealth -= amount;

        // Update health slider and text.
        if(currentHealth < 0){
            currentHealth = 0;
        }
        healthText.text = currentHealth.ToString();
        healthSlider.value = currentHealth;

        // Memaindkan audio ketika terkena damage.
        playerAudio.Play();

        // Jika currentHealth kurang dari atau sama dengan 0 dan belum mati maka memanggil
        // method death.
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void PickupOrb(int amount)
    {
        // Menambahkan currentHealth dengan amount.
        if(currentHealth + amount >= maxHealth){
            currentHealth = maxHealth;
        }else{
            currentHealth += amount;
        }
        
        // Update health slider and health data.
        if(currentHealth > healthSlider.maxValue){
            healthSlider.maxValue = currentHealth;
        }
        healthSlider.value = currentHealth;
        healthText.text = currentHealth.ToString();
    }


    void Death()
    {
        isDead = true;

        //playerShooting.DisableEffects();

        // Mentrigger animasi die.
        anim.SetTrigger("Die");

        // Memaindkan audio ketika mati.
        playerAudio.clip = deathClip;
        playerAudio.Play();

        // Mematikan movement.
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }
}
