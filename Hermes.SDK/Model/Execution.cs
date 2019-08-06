using System;
namespace Hermes.SDK.Model
{
    public class Execution
    {

        public Guid Id { get; set; }

        public long RobotId { get; set; }

        public DateTime StartedAt { get; set; }



        public Execution()
        {
        }
    }
}
