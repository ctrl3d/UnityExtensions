using UnityEngine;
using UnityEngine.UI;
using work.ctrl3d;

public class GameManager : MonoBehaviour
{
    public Button button;
 
    async void Start()
    {
        await button.onClick.AsTask();
        Debug.Log("Hello World!");
    }

  
}
