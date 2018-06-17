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
        private readonly IClientDialogsService _clientDialogsService;

        public DialogConditionsService(
            IDialogConditionsRepository conditionsRepository,
            IClientDialogsService clientDialogsService)
        {
            _conditionsRepository = conditionsRepository;
            _clientDialogsService = clientDialogsService;
        }
        
        public async Task<IEnumerable<IClientDialog>> GetDialogsWithPreTradeConditionAsync(string clientId, string assetId)
        {
            IEnumerable<IDialogCondition> conditions = await _conditionsRepository.GetDialogConditionsByTypeAsync(DialogConditionType.Pretrade);
            var clientDialogs = await _clientDialogsService.GetClientDialogsAsync(clientId);

            return clientDialogs.Where(item =>
                conditions.Any(x => x.DialogId == item.Id && x.Data.GetConditionParameters<PreTradeParameters>().AssetId == assetId));
        }

        public Task<IEnumerable<IDialogCondition>> GetDialogConditionsAsync(string dialogId)
        {
            return _conditionsRepository.GetDialogConditionsAsync(dialogId);
        }

        public Task<IEnumerable<IDialogCondition>> GetDialogConditionsByTypeAsync(DialogConditionType type)
        {
            return _conditionsRepository.GetDialogConditionsByTypeAsync(type);
        }

        public Task<IDialogCondition> GetDialogConditionAsync(string dialogId, string id)
        {
            return _conditionsRepository.GetDialogConditionAsync(dialogId, id);
        }

        public Task AddDialogConditionAsync(IDialogCondition condition)
        {
            return _conditionsRepository.AddDialogConditionAsync(condition);
        }
        
        public Task DeleteDialogConditionAsync(string dialogId, string id, DialogConditionType type)
        {
            return _conditionsRepository.DeleteDialogConditionAsync(dialogId, id, type);
        }

        public Task DeleteDialogConditionsAsync(string dialogId)
        {
            return _conditionsRepository.DeleteDialogConditionsAsync(dialogId);
        }
    }
}
