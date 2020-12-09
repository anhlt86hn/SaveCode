using System;
using System.Collections.Generic;
using System.Text;

namespace RicoCore.Utilities.SAVE
{
    class BackgroundJobHangfire
    {
        //recurringJobs.AddOrUpdate("LiveTruyenThong", Job.FromExpression<ISendHubService>(x => x.UpdateTruyenThong()), "42-35/1 22 * * *", TimeZoneInfo.Local);
        //recurringJobs.AddOrUpdate("LiveLottery", Job.FromExpression<ILotteryService>(x => x.SetLastestTraditionalResult(sendToSignalR)), "14-35/1 18 * * *", TimeZoneInfo.Local);
        //recurringJobs.AddOrUpdate("LiveLottery", Job.FromExpression<ILotteryService>(x => x.SetLastestTraditionalResult()), "25-59/1 00 * * *", TimeZoneInfo.Local); 
    }
}
