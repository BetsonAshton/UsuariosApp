using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsuariosApp.Message.Settings
{
    public class RabbitMQSettings
    {
        /// <summary>
        /// Endereço do servidor do RabbitMQ (cloudamqp)
        /// </summary>
        public string Host { get => "amqps://hvryhnti:jEGKntxN3QMpnT2zezZtamjxKFengBEC@toad.rmq.cloudamqp.com/hvryhnti"; }

        /// <summary>
        /// Nome da fila que será criada/acessada
        /// </summary>
        public string Queue { get => "mensagens_usuarios"; }
    }
}
