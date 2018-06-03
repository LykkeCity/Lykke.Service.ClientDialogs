using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage.Tables;
using Lykke.Service.ClientDialogs.AzureRepositories.ClientDialog;
using Lykke.Service.ClientDialogs.AzureRepositories.ClientDialogSubmit;
using Lykke.Service.ClientDialogs.Core.Domain;
using Lykke.Service.ClientDialogs.Services;
using Xunit;

namespace Lykke.Service.ClientDialogs.Tests
{
    public class ClientDialogsServiceTests
    {
        private readonly ClientDialogsService _service;
        private const string ClientId = "client1";

        public ClientDialogsServiceTests()
        {
            var dialogsRepository = new ClientDialogsRepository(new NoSqlTableInMemory<ClientDialogEntity>());
            var submitDialogsRepository = new ClientDialogSubmitsRepository(new NoSqlTableInMemory<ClientDialogSubmitEntity>());
            _service = new ClientDialogsService(dialogsRepository, submitDialogsRepository);
        }

        [Fact]
        public async Task Is_Client_Dialog_Added()
        {
            var dialog = CreateDialog("1", ClientId);

            await _service.AddDialogAsync(dialog);

            IClientDialog[] clientDialogs = (await _service.GetDialogsAsync(ClientId)).ToArray();
            IClientDialog[] commonDialogs = (await _service.GetCommonDialogsAsync()).ToArray();
            
            Assert.True(clientDialogs.Length == 1);
            Assert.True(commonDialogs.Length == 0);
            Assert.Equal("1", clientDialogs[0].Id);
        }
        
        [Fact]
        public async Task Is_Client_Dialog_Removed()
        {
            var dialog = CreateDialog("1", ClientId);

            await _service.AddDialogAsync(dialog);

            IClientDialog[] clientDialogs = (await _service.GetDialogsAsync(ClientId)).ToArray();
            
            Assert.True(clientDialogs.Length == 1);
            Assert.Equal("1", clientDialogs[0].Id);

            await _service.DeleteDialogAsync(ClientId, "1");
            
            clientDialogs = (await _service.GetDialogsAsync(ClientId)).ToArray();
            
            Assert.Empty(clientDialogs);
        }
        
        [Fact]
        public async Task Is_Common_Dialog_Added()
        {
            var dialog = CreateDialog("1", null);

            await _service.AddDialogAsync(dialog);

            IClientDialog[] clientDialogs = (await _service.GetDialogsAsync(ClientId)).ToArray();
            IClientDialog[] commonDialog = (await _service.GetCommonDialogsAsync()).ToArray();

            Assert.True(clientDialogs.Length == 1);
            Assert.True(commonDialog.Length == 1);
            Assert.Contains(clientDialogs, item => item.Id == "1");
            Assert.Contains(commonDialog, item => item.Id == "1");
        }
        
        [Fact]
        public async Task Is_Common_Dialog_Removed()
        {
            var dialog = CreateDialog("1", null);

            await _service.AddDialogAsync(dialog);

            IClientDialog[] clientDialogs = (await _service.GetDialogsAsync(ClientId)).ToArray();
            IClientDialog[] commonDialog = (await _service.GetCommonDialogsAsync()).ToArray();

            Assert.True(clientDialogs.Length == 1);
            Assert.True(commonDialog.Length == 1);
            Assert.Contains(clientDialogs, item => item.Id == "1");
            Assert.Contains(commonDialog, item => item.Id == "1");
            
            await _service.DeleteDialogAsync(null, "1");
            
            clientDialogs = (await _service.GetDialogsAsync(ClientId)).ToArray();
            commonDialog = (await _service.GetCommonDialogsAsync()).ToArray();

            Assert.Empty(clientDialogs);
            Assert.Empty(commonDialog);
        }
        
        [Fact]
        public async Task Is_Dialog_Returned()
        {
            var dialog = CreateDialog("1", ClientId);

            await _service.AddDialogAsync(dialog);

            IClientDialog clientDialog = await _service.GetDialogAsync(ClientId, "1");

            Assert.NotNull(clientDialog);
            Assert.Equal("1", clientDialog.Id);
            Assert.Equal(ClientId, clientDialog.ClientId);
        }
        
        [Fact]
        public async Task Is_Client_Dialog_Submitted_And_Removed_For_Client()
        {
            var dialog = CreateDialog("1", ClientId);

            await _service.AddDialogAsync(dialog);

            await _service.SubmitDialogAsync(ClientId, "1", "1");

            var submittedDialogs = await _service.GetSubmittedDialogsAsync(ClientId);
            IClientDialog[] clientDialogs = (await _service.GetDialogsAsync(ClientId)).ToArray();

            Assert.Contains(submittedDialogs, item => item.ClientId == ClientId && item.DialogId == "1" && item.ActionId == "1");
            Assert.Empty(clientDialogs);
        }
        
        [Fact]
        public async Task Is_Common_Dialog_Submitted_And_Removed_For_Client()
        {
            var dialog = CreateDialog("1", null);

            await _service.AddDialogAsync(dialog);
            
            IClientDialog[] clientDialogs = (await _service.GetDialogsAsync(ClientId)).ToArray();
            
            Assert.Contains(clientDialogs, item => item.Id == "1");

            await _service.SubmitDialogAsync(ClientId, "1", "1");

            var submittedDialogs = await _service.GetSubmittedDialogsAsync(ClientId);
            clientDialogs = (await _service.GetDialogsAsync(ClientId)).ToArray();

            Assert.Contains(submittedDialogs, item => item.ClientId == ClientId && item.DialogId == "1" && item.ActionId == "1");
            Assert.Empty(clientDialogs);
        }
        
        private IClientDialog CreateDialog(string id, string clientId, DialogType type = DialogType.Info, int actionsCount = 1)
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
                ClientId = clientId,
                Header = "test",
                Text = "Test dialog",
                Type = type,
                Actions = actions.ToArray()
            };
        }
    }
}
