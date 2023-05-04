using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update

    public GameObject MyEspTooltipBox;
    public GameObject OpEspTooltipBox;

    public void OnPointerEnter(PointerEventData eventData)
    {
        string mouse_on_object_name = this.name;
        print(mouse_on_object_name + " in");
        if (mouse_on_object_name == "my esp 1")
            MyEspTooltipBox.transform.GetChild(0).gameObject.SetActive(true);
        else if (mouse_on_object_name == "my esp 2")
            MyEspTooltipBox.transform.GetChild(1).gameObject.SetActive(true);
        else if (mouse_on_object_name == "op esp 1" && this.transform.GetChild(0).gameObject.activeSelf)
            OpEspTooltipBox.transform.GetChild(0).gameObject.SetActive(true);
        else if (mouse_on_object_name == "op esp 2" && this.transform.GetChild(0).gameObject.activeSelf)
            OpEspTooltipBox.transform.GetChild(1).gameObject.SetActive(true);
        else if (mouse_on_object_name == "mimic")
            MyEspTooltipBox.transform.GetChild(2).gameObject.SetActive(true);
        else if (mouse_on_object_name == "bomb")
            print("bomb in");
    }

        public void OnPointerExit(PointerEventData eventData)
    {
        string mouse_on_object_name = this.name;
        print(mouse_on_object_name + " out");
        if (mouse_on_object_name == "my esp 1")
            MyEspTooltipBox.transform.GetChild(0).gameObject.SetActive(false);
        else if (mouse_on_object_name == "my esp 2")
            MyEspTooltipBox.transform.GetChild(1).gameObject.SetActive(false);
        else if (mouse_on_object_name == "op esp 1" && this.transform.GetChild(0).gameObject.activeSelf)
            OpEspTooltipBox.transform.GetChild(0).gameObject.SetActive(false);
        else if (mouse_on_object_name == "op esp 2" && this.transform.GetChild(0).gameObject.activeSelf)
            OpEspTooltipBox.transform.GetChild(1).gameObject.SetActive(false);
        else if (mouse_on_object_name == "mimic")
            MyEspTooltipBox.transform.GetChild(2).gameObject.SetActive(false);
        else if (mouse_on_object_name == "bomb")
            print("bomb out");
    }
}
