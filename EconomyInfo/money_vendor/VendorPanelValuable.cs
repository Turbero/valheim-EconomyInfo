using EconomyInfo.tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EconomyInfo
{
    public class VendorPanelValuable
    {
        private readonly GameObject vendorPanelValuableGameObject;

        public VendorPanelValuable(Transform storeTransform, string valuableName, string spriteName, Vector2 anchoredPosition,
            Vector2? anchoredPositionIcon = null, Vector2? sizeDeltaIcon = null)
        {
            GameObject coins = storeTransform.Find("coins").gameObject;
            
            vendorPanelValuableGameObject = GameObject.Instantiate(coins, storeTransform);
            vendorPanelValuableGameObject.name = valuableName;
            vendorPanelValuableGameObject.SetActive(true);
            vendorPanelValuableGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
            vendorPanelValuableGameObject.transform.GetChild(0).name = "icon";
            vendorPanelValuableGameObject.transform.GetChild(1).name = "amount";

            //Change icon
            Sprite sprite = ModUtils.getSprite(spriteName);
            vendorPanelValuableGameObject.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            if (anchoredPositionIcon != null)
                vendorPanelValuableGameObject.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = anchoredPositionIcon.Value;
            if (sizeDeltaIcon != null)
                vendorPanelValuableGameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = sizeDeltaIcon.Value;

            //Value to 0 to start
            vendorPanelValuableGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0 (0)";
        }

        public void updateValue(int amount, int  value)
        {
            vendorPanelValuableGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amount + " (" + value + ")";
        }
    }

}
