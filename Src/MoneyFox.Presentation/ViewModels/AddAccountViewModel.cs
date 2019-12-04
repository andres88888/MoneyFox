﻿using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Accounts.Commands.CreateAccount;
using MoneyFox.Application.Accounts.Queries.GetIfAccountWithNameExists;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class AddAccountViewModel : ModifyAccountViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AddAccountViewModel(IMediator mediator,
                                   IMapper mapper,
                                   ISettingsFacade settingsFacade,
                                   IBackupService backupService,
                                   IDialogService dialogService,
                                   INavigationService navigationService)
            : base(settingsFacade, backupService, dialogService, navigationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            Title = Strings.AddAccountTitle;
        }

        protected override Task Initialize()
        {
            SelectedAccount = new AccountViewModel();

            return Task.CompletedTask;
        }

        protected override async Task SaveAccount()
        {
            if (await mediator.Send(new GetIfAccountWithNameExistsQuery {AccountName = SelectedAccount.Name}))
            {
                await DialogService.ShowMessage(Strings.DuplicatedNameTitle, Strings.DuplicatedNameTitle);
                return;
            }

            await mediator.Send(new CreateAccountCommand {AccountToSave = mapper.Map<Account>(SelectedAccount)});
            NavigationService.GoBack();
        }
    }
}
