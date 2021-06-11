using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Helpers
{
    public class Global
    {
        public enum RequestType
        {
            [DisplayName("multipart/form-data")]
            FileWithForm,

            [DisplayName("application/json")]
            JsonType
        }
    }
}
