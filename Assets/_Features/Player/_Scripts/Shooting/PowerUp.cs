using TopDown.Shooting;
using UnityEngine;

public class PowerUp : Item
{
    public enum PowerUpType { TripleShot, AutoFire }
    public PowerUpType powerUpType;
    public float duration = 10f; // Duração do efeito do power-up

    public void ActivatePowerUp(GunController gun)
    {
        gun.ActivatePowerUp(powerUpType, duration);
        Destroy(this.gameObject, 0.1f); // Pequeno atraso para evitar problemas de referência
    }
}
