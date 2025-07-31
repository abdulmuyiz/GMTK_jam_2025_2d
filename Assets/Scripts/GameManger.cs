using TMPro;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    [SerializeField]
    public Canvas canvas;
    public PlayerController playerController;
    public TextMeshProUGUI bulletnumber;

    // Update is called once per frame
    void Update()
    {
        bulletnumber.SetText(playerController.bulletCount.ToString());
    }

}
