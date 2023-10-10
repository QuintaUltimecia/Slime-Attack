using UnityEngine;

public class BoostWallet : BaseBoost
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.Wallet.AddWallet(1);
        }
    }
}
