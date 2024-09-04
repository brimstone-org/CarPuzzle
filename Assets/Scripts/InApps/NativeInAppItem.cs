﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace NativeInApps
{

    public class NativeInAppItem
    {

        public ProductType Type { get; private set; }

        public string ProductID { get; private set; }

        public string Name { get; private set; }

        public NativeInAppItem(string name, string sku, ProductType type)
        {
            Name = name;
            ProductID = sku;
            Type = type;
        }

    }
}
