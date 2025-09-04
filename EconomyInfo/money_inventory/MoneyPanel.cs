using EconomyInfo.tools;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EconomyInfo.money_inventory
{
    public class MoneyPanel
    {
        public enum MoneyPanelType
        {
            Inventory, Container
        }
        
        private readonly GameObject moneyPanelGameObject;
        private readonly Transform weightTransform;
        private TextMeshProUGUI moneyPanelValueComponentText;

        public MoneyPanel(MoneyPanelType moneyPanelType, Transform parentTransform)
        {
            if (moneyPanelType == MoneyPanelType.Inventory)
                weightTransform = InventoryGui.instance.m_inventoryRoot.transform.Find("Player").transform.Find("Weight");
            else
                weightTransform = InventoryGui.instance.m_inventoryRoot.transform.Find("Container").transform.Find("Weight");
            
            // Panel
            moneyPanelGameObject = new GameObject("MoneyPanel", typeof(RectTransform));
            moneyPanelGameObject.SetActive(true);
            moneyPanelGameObject.transform.SetParent(parentTransform);
            RectTransform panelRect = moneyPanelGameObject.GetComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(80, 64);
            if (moneyPanelType == MoneyPanelType.Inventory) 
                panelRect.anchoredPosition = new Vector2(317, -7);
            else if (moneyPanelType == MoneyPanelType.Container) 
                panelRect.anchoredPosition = new Vector2(319, -30);

            // print in UI after Weight to have same effect of partially hidde under the inventory
            moneyPanelGameObject.transform.SetSiblingIndex(weightTransform.GetSiblingIndex() + 1); // other children are moved automatically, csharp magic!

            moneyPanelBackground();
            moneyPanelIcon();
            moneyPanelValue();

            moneyPanelValueComponentText.text = "0";
        }

        private void moneyPanelBackground()
        {
            GameObject bkgOriginal = weightTransform.Find("bkg").gameObject;
            GameObject clonedBkg = GameObject.Instantiate(bkgOriginal, moneyPanelGameObject.transform);
            clonedBkg.name = "moneypanel_bkg";
        }

        private void moneyPanelIcon()
        {
            GameObject moneyPanelIconComponent = new GameObject("moneypanel_icon");
            moneyPanelIconComponent.transform.SetParent(moneyPanelGameObject.transform);
            Image moneyPanelIconComponentImage = moneyPanelIconComponent.AddComponent<Image>();
            moneyPanelIconComponentImage.sprite = ModUtils.getSprite("coins");
            moneyPanelIconComponentImage.type = Image.Type.Sliced;
            RectTransform moneyPanelIconComponentImageRect = moneyPanelIconComponent.GetComponent<RectTransform>();
            moneyPanelIconComponentImageRect.sizeDelta = new Vector2(32, 32);
            moneyPanelIconComponentImageRect.anchoredPosition = new Vector2(2, 25);
        }

        private void moneyPanelValue()
        {
            GameObject textOriginal = weightTransform.Find("weight_text").gameObject;
            GameObject clonedText = GameObject.Instantiate(textOriginal, moneyPanelGameObject.transform);
            clonedText.name = "moneypanel_value";
            RectTransform textRect = clonedText.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(80, 64);
            textRect.anchoredPosition = new Vector2(0, 0);
            
            moneyPanelValueComponentText = clonedText.GetComponent<TextMeshProUGUI>();
        }

        public void updateMoneyValue(string value)
        {
            moneyPanelValueComponentText.text = value;
        }

        public GameObject getGameObject() { return moneyPanelGameObject; }
    }

}
