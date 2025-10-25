using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface IChannelRecordRepository
    {
        bool Add(ChannelRecord channelRecordModel, Dictionary<int, int> channelDoseData);
        bool Edit(ChannelRecord channelRecordModel, Dictionary<int, int> channelDoseData);
        bool Remove(int id);
        ChannelRecord GetByID(int id);
        IEnumerable<ChannelRecordView> GetAll(string searchWord);
    }
}
