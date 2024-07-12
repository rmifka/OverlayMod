﻿using OverlayMod.Stat.Stats;
using OverlayMod.Views.ViewControllers.CenterScreen;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Zenject;

namespace OverlayMod.Stat.Preview
{
    internal abstract class IPreviewStat : IInitializable
    {
        [Inject] private PreviewCanvasController _canvasController;

        protected abstract IStat parentStat { get; }

        protected bool enabled => parentStat.enabled;
        protected int posX => parentStat.posX;
        protected int posY => parentStat.posY;
        protected float size => parentStat.size;

        private GameObject textObject = new GameObject();
        protected TextMeshProUGUI textMesh;

        protected abstract string text { get; }

        public void Initialize()
        {
            textObject = new GameObject();
            textObject.transform.parent = _canvasController.canvasGameObj.transform;
            textMesh = textObject.AddComponent<TextMeshProUGUI>();

            this.textMesh.text = text;

            this.textObject.SetActive(enabled);
            this.textMesh.fontSize = size * ((Plugin.scaleX + Plugin.scaleY) / 2); // last part averages the difference in text size incase the display ratio isnt 16:9
            this.textMesh.alignment = parentStat.optionalAllignmentOverride ?? TextAlignmentOptions.Center;
            this.textMesh.enableWordWrapping = false;

            this.textObject.GetComponent<RectTransform>().localPosition = parentStat.getNormalizedPosition(posX, posY);

            doExtraThings();
        }

        protected virtual void doExtraThings() { return; }

        #region notifypropertychanged garbage

        public virtual void Update()
        {
            this.textMesh.fontSize = size * ((Plugin.scaleX + Plugin.scaleY) / 2);
            this.textObject.transform.localPosition = parentStat.getNormalizedPosition(posX, posY);
            this.textObject.SetActive(enabled);
        }
        #endregion
    }
}
