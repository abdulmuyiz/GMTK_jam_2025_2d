using System.Collections;
using UnityEngine;

public class AutoAutoGen : MonoBehaviour
{
    private GameObject duplicate;
    private int count = 0;

    public GameObject parent;

    private void Update()
    {
        if(count == 0)
        {
            StartCoroutine(auto());
        }
        else
        {
            StopCoroutine(auto());
            this.enabled = false;
        }

    }

    private IEnumerator auto()
    {
        if (count == 0)
        {
            yield return new WaitForSeconds(0.1f);
            duplicate = GameObject.FindGameObjectWithTag("ChestGroup");
            GameObject one = Instantiate(duplicate, new Vector3(-27, 0, 0), Quaternion.identity);
            GameObject two =Instantiate(duplicate, new Vector3(-27,-30,0), Quaternion.identity);
            GameObject three = Instantiate(duplicate, new Vector3(0, -30, 0), Quaternion.identity);
            one.transform.SetParent(parent.transform);
            two.transform.SetParent(parent.transform);
            three.transform.SetParent(parent.transform);
        }
        count++;
    }
}
