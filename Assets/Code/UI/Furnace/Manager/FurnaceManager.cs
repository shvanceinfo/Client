using UnityEngine;
using System.Collections;
using MVC.entrance.gate;

namespace manager
{
    public class FurnaceManager
    {


        public static void OpenWindow()
        {
            UIManager.Instance.openWindow(UiNameConst.ui_furnace);
        }
    }
}
