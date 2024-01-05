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
                //Debug.Log("Player hold sth");
                //player is carrying sth
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Debug.Log("Player hold Plate");

                    //player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        //Debug.Log("Player take ingredient");

                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //player is not carrying Plate but sth else
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //counter has a Plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // player is not carrying thing
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
