using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common;
using Lykke.Service.ClientDialogs.AzureRepositories.ClientDialog;
using Lykke.Service.ClientDialogs.AzureRepositories.ClientDialogSubmit;
using Lykke.Service.ClientDialogs.AzureRepositories.DialogCondition;
using Lykke.Service.ClientDialogs.Core.Domain;
using Lykke.Service.ClientDialogs.Core.Domain.ConditionParameters;
using Lykke.Service.ClientDialogs.Services;
using Xunit;

namespace Lykke.Service.ClientDialogs.Tests
{
    public class ClientDialogsServiceTests
    {
        private readonly ClientDialogsService _service;
        private readonly DialogConditionsService _conditionsService;
        private const string ClientId = "client1";

        public ClientDialogsServiceTests()
        {
            var dialogsRepository = new ClientDialogsRepository(
                new NoSqlTableInMemory<ClientDialogEntity>(),
                new NoSqlTableInMemory<AzureIndex>(),
                new NoSqlTableInMemory<AzureIndex>());
            var submitDialogsRepository = new ClientDialogSubmitsRepository(new NoSqlTableInMemory<ClientDialogSubmitEntity>());
            var dialogsConditionRepository = new DialogConditionsRepository(new NoSqlTableInMemory<DialogConditionEntity>(),
                new NoSqlTableInMemory<AzureIndex>());
            _service = new ClientDialogsService(dialogsRepository, submitDialogsRepository, dialogsConditionRepository);
            _conditionsService = new DialogConditionsService(dialogsConditionRepository, dialogsRepository);
        }

        [Fact]
        public async Task Is_Dialog_Added_And_Deleted()
        {
            const string dialogId = "1";
            
            await AddDialogAsync(dialogId);

            var dialog = await _service.GetDialogAsync(dialogId);
            var dialogs = (await _service.GetDialogsAsync()).ToArray();
            
            Assert.NotNull(dialog);
            Assert.NotEmpty(dialogs);
            Assert.Contains(dialogs, x => x.Id == dialogId);

            await _service.DeleteDialogAsync(dialogId);
            
            dialog = await _service.GetDialogAsync(dialogId);
            dialogs = (await _service.GetDialogsAsync()).ToArray();
            
            Assert.Null(dialog);
            Assert.Empty(dialogs);
        }
        
        [Fact]
        public async Task Is_Common_Dialog_Added_And_Removed()
        {
            const string dialogId = "1";
            await AddDialogAsync(dialogId);

            await _service.AssignDialogToAllAsync(dialogId);

            IClientDialog[] commonDialogs = (await _service.GetCommonDialogsAsync()).ToArray();

            Assert.True(commonDialogs.Length == 1);
            Assert.Contains(commonDialogs, item => item.Id == dialogId);

            await _service.UnAssignCommonDialogAsync(dialogId);
            
            commonDialogs = (await _service.GetCommonDialogsAsync()).ToArray();

            Assert.Empty(commonDialogs);
        }
        
        [Fact]
        public async Task Is_Client_Dialog_Assigned_And_Removed()
        {
            const string dialogId = "1";
            
            await AddDialogAsync(dialogId);

            await _service.AssignDialogToClientAsync(ClientId, dialogId);

            IClientDialog clientDialog = await _service.GetClientDialogAsync(ClientId, dialogId);
            
            Assert.NotNull(clientDialog);
            Assert.Equal(dialogId, clientDialog.Id);

            await _service.DeleteDialogAsync(ClientId, dialogId);

            clientDialog = await _service.GetClientDialogAsync(ClientId, dialogId);
            
            Assert.Null(clientDialog);
        }
        
        [Fact]
        public async Task Is_Client_Dialog_UnAssigned_When_Deleted()
        {
            const string dialogId = "1";
            
            await AddDialogAsync(dialogId);

            await _service.AssignDialogToClientAsync(ClientId, dialogId);
            await _service.AssignDialogToAllAsync(dialogId);

            IClientDialog clientDialog = await _service.GetClientDialogAsync(ClientId, dialogId);
            IClientDialog[] commonDialogs = (await _service.GetCommonDialogsAsync()).ToArray();
            
            Assert.NotNull(clientDialog);
            Assert.Equal(dialogId, clientDialog.Id);
            Assert.NotEmpty(commonDialogs);
            Assert.Contains(commonDialogs, x => x.Id == dialogId);

            await _service.DeleteDialogAsync(dialogId);

            clientDialog = await _service.GetClientDialogAsync(ClientId, dialogId);
            commonDialogs = (await _service.GetCommonDialogsAsync()).ToArray();
            
            Assert.Null(clientDialog);
            Assert.Empty(commonDialogs);
        }
        
        [Fact]
        public async Task Is_Client_Dialog_Submitted_And_Removed_For_Client()
        {
            const string dialogId = "1";
            
            await AddDialogAsync(dialogId);

            await _service.SubmitDialogAsync(ClientId, dialogId, "1");

            var submittedDialogs = await _service.GetSubmittedDialogsAsync(ClientId);
            IClientDialog[] clientDialogs = (await _service.GetClientDialogsAsync(ClientId)).ToArray();

            Assert.Contains(submittedDialogs, item => item.ClientId == ClientId && item.DialogId == dialogId && item.ActionId == "1");
            Assert.Empty(clientDialogs);
        }
        
        [Fact]
        public async Task Is_Common_Dialog_Submitted_And_Removed_For_Client()
        {
            const string dialogId = "1";

            await AddDialogAsync(dialogId);
            await _service.AssignDialogToAllAsync(dialogId);
            
            IClientDialog[] clientDialogs = (await _service.GetClientDialogsAsync(ClientId)).ToArray();
            
            Assert.Contains(clientDialogs, item => item.Id ==dialogId);

            await _service.SubmitDialogAsync(ClientId, dialogId, "1");

            var submittedDialogs = await _service.GetSubmittedDialogsAsync(ClientId);
            clientDialogs = (await _service.GetClientDialogsAsync(ClientId)).ToArray();

            Assert.Contains(submittedDialogs, item => item.ClientId == ClientId && item.DialogId == dialogId && item.ActionId == "1");
            Assert.Empty(clientDialogs);
        }

        [Fact]
        public async Task Is_Dialog_Condition_Added_And_Removed()
        {
            const string dialogId = "1";

            await AddDialogAsync(dialogId);
            await _conditionsService.AddDialogConditionAsync(new DialogCondition
            {
                DialogId = dialogId,
                Type = DialogConditionType.Pretrade,
                Data = new PreTradeParameters {AssetId = "BTC"}.ToJson()
            });

            var condition = await _conditionsService.GetDialogConditionAsync(dialogId);
            var dialog = await _service.GetDialogAsync(dialogId);
            
            Assert.Equal(dialogId, condition.DialogId);
            Assert.Equal(DialogConditionType.Pretrade, dialog.ConditionType);

            await _conditionsService.DeleteDialogConditionAsync(dialogId);
            
            condition = await _conditionsService.GetDialogConditionAsync(dialogId);
            dialog = await _service.GetDialogAsync(dialogId);
            
            Assert.Null(condition);
            Assert.Null(dialog.ConditionType);
        }
        
        private IClientDialog CreateDialog(string id, DialogType type = DialogType.Info, int actionsCount = 1)
        {
            List<DialogAction> actions = new List<DialogAction>();
            
            for (var i = 0; i < actionsCount; i++)
            {
                actions.Add(new DialogAction
                {
                    Id = $"{i + 1}",
                    Text = $"Button{i}",
                    Type = ActionType.Submit
                });
            }
            
            return new ClientDialog
            {
                Id = id,
                Header = "test",
                Text = "Test dialog",
                Type = type,
                Actions = actions.ToArray()
            };
        }

        private Task AddDialogAsync(string dialogId)
        {
            var dialog = CreateDialog(dialogId);
            return _service.AddDialogAsync(dialog);
        }
    }
}
