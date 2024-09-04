using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace NativeInApps
{

    public static class NativeInApp_IDS
    {
        public const string NO_ADS_PRODUCT_ID = "com.tedrasoft.drive.traffic.control.noads";
        public const string NO_ADS_PRODUCT_NAME = "No Ads";

        public const string PACK_HEAVY_2_PRODUCT_ID = "com.tedrasoft.drive.traffic.control.heavy2";
        public const string PACK_HEAVY_2_NAME = "Pack Heavy 2";

        public const string LEVEL_SOLUTIONS_PRODUCT_ID = "com.tedrasoft.drive.traffic.control.solutions";
        public const string LEVEL_SOLUTIONS_PRODUCT_NAME = "Unlock Solutions";

        public const string PACK_MEDIUM_2_PRODUCT_ID = "com.tedrasoft.drive.traffic.control.rush2";
        public const string PACK_MEDIUM_2_NAME = "Pack Rush 2";

        public static NativeInAppItem[] Items =
        {
            new NativeInAppItem(NO_ADS_PRODUCT_NAME, NO_ADS_PRODUCT_ID, ProductType.NonConsumable),
            new NativeInAppItem(PACK_HEAVY_2_NAME, PACK_HEAVY_2_PRODUCT_ID, ProductType.NonConsumable),
            new NativeInAppItem(LEVEL_SOLUTIONS_PRODUCT_NAME, LEVEL_SOLUTIONS_PRODUCT_ID, ProductType.NonConsumable),
            new NativeInAppItem(PACK_MEDIUM_2_NAME, PACK_MEDIUM_2_PRODUCT_ID, ProductType.NonConsumable)
        };

        public static string GetItemName(string sku) {
            for (int i = 0; i < Items.Length; i++)
                if (Items[i].ProductID.Equals(sku))
                    return Items[i].Name;
            return null;
        }

        public static string GetItemId(string name) {
            for (int i = 0; i < Items.Length; i++)
                if (Items[i].Name.Equals(name))
                    return Items[i].ProductID;
            return null;
        }

    }
}
