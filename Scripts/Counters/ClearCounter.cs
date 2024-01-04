using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject()) // player is carrying thing
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // player has nothing

            }
        }
        else
        {
            if(player.HasKitchenObject())
            {
                //player is carrying sth
            }
            else
            {
                // player is not carrying thing
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
