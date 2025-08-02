using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    private CinemachineCamera playerCam;
    private CinemachineCamera panCam;
    private void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<CinemachineCamera>();
        panCam = GameObject.FindGameObjectWithTag("PanCam").GetComponent<CinemachineCamera>();
    }
    public void PanOut()
    {
        if (playerCam.Priority > panCam.Priority)
        {
            playerCam.Priority = -playerCam.Priority;
        }
    }

    public void PanIn()
    {
        if (playerCam.Priority < panCam.Priority)
        {
            playerCam.Priority = -playerCam.Priority;
        }
    }


}
