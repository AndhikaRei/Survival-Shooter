using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    //PlayerShooting playerShooting;
    bool isDead;                                                
    bool damaged;                                               


    void Awake()
    {
        // Mendapatkan reference component.
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();

        //playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = startingHealth;
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

        damaged = false;
    }


    // Fungsi Untuk Mendapatkan damage. 
    public void TakeDamage(int amount)
    {
        damaged = true;

        // Mengurangi currentHealth dengan amount.
        currentHealth -= amount;

        // Update health slider.
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
        //playerShooting.enabled = false;
    }

    public void RestartLevel()
    {
        // Meload ulang scene dengan index 0 pada build setting.
        SceneManager.LoadScene(0);
    }
}
