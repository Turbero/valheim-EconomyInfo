using EconomyInfo.tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EconomyInfo.money_vendor
{
    public class VendorPanelValuable
    {
        private readonly GameObject vendorPanelValuableGameObject;

        public VendorPanelValuable(Transform storeTransform, string valuableName, string spriteName, bool configActive,
            Vector2 anchoredPosition, Vector2? anchoredPositionIcon = null, Vector2? sizeDeltaIcon = null)
        {
            GameObject coins = storeTransform.Find("coins").gameObject;
            
            vendorPanelValuableGameObject = GameObject.Instantiate(coins, storeTransform);
            vendorPanelValuableGameObject.name = valuableName;
            vendorPanelValuableGameObject.SetActive(configActive);
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
            if (amount == 0)
            {
                vendorPanelValuableGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().faceColor =
                    new Color(255, 0, 0, 255);
            }
            else
            {
                vendorPanelValuableGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().faceColor =
                    new Color(255, 255, 255, 255);
            }
        }

        public GameObject getMainPanel()
        {
            return vendorPanelValuableGameObject; 
        }
    }

}
