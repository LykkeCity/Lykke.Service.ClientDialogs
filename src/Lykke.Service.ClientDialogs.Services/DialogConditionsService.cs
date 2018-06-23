using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.Core.Domain;
using Lykke.Service.ClientDialogs.Core.Domain.ConditionParameters;
using Lykke.Service.ClientDialogs.Core.Extensions;
using Lykke.Service.ClientDialogs.Core.Services;

namespace Lykke.Service.ClientDialogs.Services
{
    public class DialogConditionsService : IDialogConditionsService
    {
        private readonly IDialogConditionsRepository _conditionsRepository;
        private readonly IClientDialogsRepository _clientDialogsRepository;

        public DialogConditionsService(
            IDialogConditionsRepository conditionsRepository,
            IClientDialogsRepository clientDialogsRepository)
        {
            _conditionsRepository = conditionsRepository;
            _clientDialogsRepository = clientDialogsRepository;
        }
        
        public async Task<IEnumerable<IClientDialog>> GetDialogsWithPreTradeConditionAsync(string clientId, string assetId)
        {
            IEnumerable<IDialogCondition> conditions = await _conditionsRepository.GetDialogConditionsAsync(DialogConditionType.Pretrade);
            var clientDialogs = await _clientDialogsRepository.GetClientDialogsAsync(clientId);

            return clientDialogs.Where(item =>
                conditions.Any(x => x.DialogId == item.Id && x.Data.GetConditionParameters<PreTradeParameters>().AssetId == assetId));
        }

        public Task<IDialogCondition> GetDialogConditionAsync(string dialogId)
        {
            return _conditionsRepository.GetDialogConditionAsync(dialogId);
        }

        public Task AddDialogConditionAsync(IDialogCondition condition)
        {
            var tasks = new List<Task>
            {
                _conditionsRepository.AddDialogConditionAsync(condition),
                _clientDialogsRepository.SetDialogConditionTypeAsync(condition.DialogId, condition.Type)
            };

            return Task.WhenAll(tasks);
        }

        public Task DeleteDialogConditionAsync(string dialogId)
        {
            var tasks = new List<Task>
            {
                _conditionsRepository.DeleteDialogConditionAsync(dialogId),
                _clientDialogsRepository.SetDialogConditionTypeAsync(dialogId, null)
            };
            
            return Task.WhenAll(tasks);
        }
    }
}
