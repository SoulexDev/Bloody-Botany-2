using UnityEngine;

[CreateAssetMenu(menuName = "Bloody Botany/Gun Data")]
public class GunData : ScriptableObject
{
    public bool automatic = false;

    public InventoryItem gunItem;
    public float fireRate = 0.2f;
    public float spread = 0.01f;
    public int damage = 1;
    public AnimationCurve damageFalloff;

    public int bulletsPerShot = 1;
    public int magazineSize = 16;
    public AudioClip fireSound;
}