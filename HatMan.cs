using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatMan : Interactable {
    public override void Interact(Transform transform)
    {
        PlayerController playerController = transform.GetComponent<PlayerController>();
        StationUI.Instance.SetHatSelectScreenVisibility(true);
        StationUI.Instance.DetermineRenderTextureToShow(playerController.playerID);
        HatManager.playerControllerToWorkWith = playerController;
    }
}
