using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.Infra.Messages.Settings
{
    public class RabbitMQSettings
    {
        public string Host => "localhost";
        public int Port => 5672;
        public string User => "guest";
        public string Pass => "guest";
        public string Queue => "registered_users";
        public string VirtualHost => "/";
    }
}
