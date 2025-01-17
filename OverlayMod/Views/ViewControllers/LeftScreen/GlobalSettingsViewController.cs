﻿using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using OverlayMod.Configuration;
using OverlayMod.Stat.Preview;
using OverlayMod.Stat.Preview.PreviewStats;
using OverlayMod.Views.ViewControllers.CenterScreen;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;


namespace OverlayMod.Views.ViewControllers.LeftScreen
{
    [ViewDefinition("OverlayMod.Views.ViewControllers.LeftScreen.GlobalSettingsViewController.bsml")]
    internal class GlobalSettingsViewController : BSMLAutomaticViewController
    {
        [Inject] private readonly ConfigViewController _otherViewController;
        [Inject] private readonly GithubUpdateFetcher _githubUpdateFetcher;

        [Inject] private readonly PreviewCanvasController _previewCanvasController;

        [UIParams] private BSMLParserParams parserParams;


        [UIValue("decimalFormatValue")]
        private object decimalFormatValue
        {
            get => nameof(PluginConfig.Instance.prefDecimalFormatting);
            set
            {
                if ((string)value == nameof(PluginConfig.decimalFormat.Reigonal))
                    PluginConfig.Instance.prefDecimalFormatting = PluginConfig.decimalFormat.Reigonal;
                else
                    PluginConfig.Instance.prefDecimalFormatting = PluginConfig.decimalFormat.Unified;

                _previewCanvasController.updateStats();
            }
        }

        [UIValue("decimalFormatOptions")]
        private List<object> decimalFormatOptions = new List<object>() { nameof(PluginConfig.decimalFormat.Reigonal), nameof(PluginConfig.decimalFormat.Unified) };

        [UIValue("zenModeDisable")]
        private bool zenModeDisable
        {
            get => PluginConfig.Instance.zenModeDisable;
            set => PluginConfig.Instance.zenModeDisable = value;
        }

        [UIValue("globalEnable")]
        private bool globalEnable
        {
            get => PluginConfig.Instance.globalEnable;
            set => PluginConfig.Instance.globalEnable = value;
        }

        [UIAction("resetAllButtonOnClick")]
        private void resetAllButtonOnClick()
        {
            parserParams.EmitEvent("resetSettingsModalShow");
        }

        [UIAction("resetSettingsDeny")]
        private void resetButtonDeny()
        {
            parserParams.EmitEvent("resetSettingsModalHide");
        }

        [UIAction("resetSettingsConfirm")]
        private void resetSettingsConfirm()
        {
            parserParams.EmitEvent("resetSettingsModalHide");
            StatConfig.clearConfig();
            _otherViewController.notifyPropertyChanged();
            _previewCanvasController.updateStats();
            parserParams.EmitEvent("resetSettingsModalCompletedShow");
        }

        [UIAction("resetSettingsModalCompletedOkButton")]
        private void resetSettingsModalCompletedOkButton()
        {
            parserParams.EmitEvent("resetSettingsModalCompletedHide");
        }

        [UIComponent("versionText")]
        private TextMeshProUGUI versionText;

        [UIComponent("updateText")]
        private TextMeshProUGUI updateText;

        [UIAction("updateTextOnClick")]
        private void updateTextOnClick()
        {
            System.Diagnostics.Process.Start("https://github.com/NotBela/OverlayMod/releases");

            if (updateText.text == "Mod version is up to date!") updateText.color = Color.green;
            else updateText.color = Color.red;
        }

        [UIAction("#post-parse")]
        private async void postParse()
        {
            versionText.text = $"OverlayMod v{IPA.Loader.PluginManager.GetPluginFromId("OverlayMod").HVersion}";

            if (await _githubUpdateFetcher.IsModVersionUpToDateAsync())
            {
                updateText.text = "Mod version is up to date!";
                updateText.color = Color.green;
                return;
            }

            updateText.text = "Mod version is not up to date! Click here to go to the update page!";
            updateText.color = Color.red;
        }
    }
}