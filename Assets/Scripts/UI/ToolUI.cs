/**==============================
 * 作者：Snowe （斯诺）
 * E-mail：snowe0517@gmail.com
 * QQ：275273997
 *================================*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class ToolUI : MonoBehaviour
    {
        public GameObject[] Windows;

        private MenuBtn[] btns;

        private void Start()
        {
            btns = GetComponentsInChildren<MenuBtn>();
        }
        private void Update()
        {
            bool MouseUI = EventSystem.current.IsPointerOverGameObject();
            if (!MouseUI && Input.GetMouseButtonDown(0))
            {
                Refreash();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Refreash();
            }
        }

        void Refreash()
        {
            foreach (var w in Windows)
            {
                w.SetActive(false);
            }
            foreach (var b in btns)
            {
                b.status = false;
            }
        }

        public void CloseTarget(MenuBtn btn)
        {
            foreach (var w in Windows)
            {
                if (w == btn.target) continue;
                w.SetActive(false);
            }
            foreach (var b in btns)
            {
                if (b == btn) continue;
                b.status = false;
            }
        }
    }
}
