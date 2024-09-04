using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NativeInApps{

public class InAppWrapper : MonoBehaviour {

    public void Restore(){
        NativeInApp.Instance.RestorePurchases();
    }

    public void Buy(string sku){
        NativeInApp.Instance.BuyProductID(sku);
    }
}
}
