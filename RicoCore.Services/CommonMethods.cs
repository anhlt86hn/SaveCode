using RicoCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RicoCore.Services
{
    public static class CommonMethods
    {
       public static int GetOrder(List<int> list)
        {        
            var order = 0;
            if (list.Count() == 0)
                order = 1;
            for(int i = 1; i <= list.Count(); i++)
            {
                if (!list.Contains(i))
                {
                    order = i;
                    break;
                }
                if (i == list.Count())
                {
                    order = i + 1;
                    break;
                }
            }
            return order;
        }


       public static string GenerateCode()
        {
            return CommonHelper.GenerateRandomCode();
        }
    }
}
