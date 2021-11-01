using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public Image backgroundImg;
    public Sprite[] imageList;

    void Start()
    {
        SetImage();
    }

    //Inicializa imagen de fondo
    private void SetImage()
    {
        Image bg = GameObject.Find("Background").GetComponent<Image>();
    }
    void Awake()
    { 
    }

    //Cambio de imagen background
   public void ChangeImage()
    {
        Sprite newimg = imageList[Random.Range(0, imageList.Length)];
        if (newimg != GameObject.Find("Background").GetComponent<Image>().sprite)
            GameObject.Find("Background").GetComponent<Image>().sprite = newimg;
        else
            ChangeImage();
    }

  


}
