﻿using Rg.Plugins.Popup.Extensions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.Views.Dialogs
{
    public partial class ConfirmMessageDialog
    {
        public ConfirmMessageDialog(string title, string message, string positiveText = "", string negativeText = "")
        {
            InitializeComponent();

            PopupTitle = title;
            PopupMessage = message;
            PositiveText = positiveText;
            NegativeText = negativeText;
        }

        public static readonly BindableProperty PopupTitleProperty = BindableProperty.Create(
            nameof(PopupTitle),
            typeof(string),
            typeof(ConfirmMessageDialog));

        public static readonly BindableProperty PopupMessageProperty = BindableProperty.Create(
            nameof(PopupMessage),
            typeof(string),
            typeof(ConfirmMessageDialog));

        public static readonly BindableProperty PositiveTextProperty = BindableProperty.Create(
            nameof(PositiveText),
            typeof(string),
            typeof(ConfirmMessageDialog));

        public static readonly BindableProperty NegativeTextProperty = BindableProperty.Create(
            nameof(NegativeText),
            typeof(string),
            typeof(ConfirmMessageDialog));

        public string PopupTitle
        {
            get => (string)GetValue(PopupTitleProperty);
            set => SetValue(PopupTitleProperty, value);
        }

        public string PopupMessage
        {
            get => (string)GetValue(PopupMessageProperty);
            set => SetValue(PopupMessageProperty, value);
        }

        public string PositiveText
        {
            get => (string)GetValue(PositiveTextProperty);
            set => SetValue(PositiveTextProperty, value);
        }

        public string NegativeText
        {
            get => (string)GetValue(NegativeTextProperty);
            set => SetValue(NegativeTextProperty, value);
        }

        private TaskCompletionSource<bool>? confirmTaskCompletionSource;

        public async Task<bool> ShowAsync()
        {
            confirmTaskCompletionSource = new TaskCompletionSource<bool>();
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushPopupAsync(this);
            return await confirmTaskCompletionSource.Task;
        }

        private async void PositiveHandlerClicked(object sender, EventArgs e)
        {
            await DismissAsync();
            confirmTaskCompletionSource?.SetResult(true);
        }

        private async void NegativeHandlerClicked(object sender, EventArgs e)
        {
            await DismissAsync();
            confirmTaskCompletionSource?.SetResult(false);
        }

        private static async Task DismissAsync()
            => await Xamarin.Forms.Application.Current.MainPage.Navigation.PopPopupAsync();
    }
}