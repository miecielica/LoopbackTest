using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopbackTest
{
    public interface IAndroidMethods
    {
        void sendVerificationEmail(string address, string id, string verificationToken);
    }
}
