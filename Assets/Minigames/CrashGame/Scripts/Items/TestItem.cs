using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class TestItem : Item
    {
        public override string GetName() => "TestItem";
        public override void ActivateItem()
        {
            Debug.Log("Item Acquired!");
        }
    }
}
