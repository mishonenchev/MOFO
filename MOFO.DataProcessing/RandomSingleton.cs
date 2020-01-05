using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.DataProcessing
{
    public sealed class RandomSingleton
    {
        private static Random instance = null;
        private static readonly object padlock = new object();


        public static Random Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Random();
                        }
                    }
                }
                return instance;
            }
        }
    }
}
